using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using NHibernate.Cache;
using NHibernate.Util;
using System.Net.Sockets;
using StackExchange.Redis;
using System.Runtime.Caching;
using System.Threading;

namespace NHibernate.Caches.Redis
{
  /// <summary>
  /// 
  /// </summary>
  public class RedisCache : ICache
  {
    #region private
    //private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(RedisCache));

    private static readonly INHibernateLogger Log = NHibernateLogger.For(typeof(RedisCache));

    // The acquired locks do not need to be distributed into Redis because
    // the same ISession will lock/unlock an object.
    private readonly MemoryCache _acquiredLocks = new MemoryCache("NHibernate.Caches.Redis.RedisCache");

    private static readonly LuaScript GetScript = LuaScript.Prepare(@"
if redis.call('sismember', @setOfActiveKeysKey, @key) == 1 then
    local result = redis.call('get', @key)
    if not result then
        redis.call('srem', @setOfActiveKeysKey, @key)
    end
    return result
else
    redis.call('del', @key)
    return nil
end
");
    private static readonly LuaScript SlidingExpirationScript = LuaScript.Prepare(@"
local pttl = redis.call('pttl', @key)
if pttl <= tonumber(@slidingExpiration) then
    redis.call('pexpire', @key, @expiration)
    return true
else
    return false
end
");

    private static readonly LuaScript PutScript = LuaScript.Prepare(@"
redis.call('sadd', @setOfActiveKeysKey, @key)
redis.call('set', @key, @value, 'PX', @expiration)
");
    private static readonly LuaScript RemoveScript = LuaScript.Prepare(@"
redis.call('srem', @setOfActiveKeysKey, @key)
redis.call('del', @key)
");
    private readonly LuaScript _unlockScript = LuaScript.Prepare(@"
if redis.call('get', @lockKey) == @lockValue then
    return redis.call('del', @lockKey)
else
    return 0
end
");

    // Help with debugging scripts since exceptions are swallowed with FireAndForget.
#if DEBUG
    private const CommandFlags FireAndForgetFlags = CommandFlags.None;
#else
        private const CommandFlags fireAndForgetFlags = CommandFlags.FireAndForget;
#endif

    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly RedisCacheProviderOptions _options;
    private readonly TimeSpan _expiration;
    private readonly TimeSpan _slidingExpiration;
    private readonly TimeSpan _lockTimeout;
    private readonly TimeSpan _acquireLockTimeout;
    #endregion

    private class LockData
    {
      public string Key { get; private set; }
      public string LockKey { get; private set; }
      public string LockValue { get; private set; }

      public LockData(string key, string lockKey, string lockValue)
      {
        this.Key = key;
        this.LockKey = lockKey;
        this.LockValue = lockValue;
      }

      public override string ToString()
      {
        return "{ Key='" + Key + "', LockKey='" + LockKey + "', LockValue='" + LockValue + "' }";
      }
    }
    private bool TryAcquireLock(LockData lockData)
    {
      var db = GetDatabase();


      // Don't use IDatabase.LockTake() because we don't use the matching
      // LockRelease(). So, avoid any confusion. Besides, LockTake() just
      // calls this anyways.
      var wasLockAcquired = db.StringSet(lockData.LockKey, lockData.LockValue, _lockTimeout, When.NotExists);

      if (wasLockAcquired)
      {
        // It's ok to use Set() instead of Add() because the lock in 
        // Redis will cause other clients to wait.
        _acquiredLocks.Set(lockData.Key, lockData, absoluteExpiration: DateTime.UtcNow.Add(_lockTimeout));
      }

      return wasLockAcquired;
    }
    private IDatabase GetDatabase()
    {
      var dd = _connectionMultiplexer.IsConnected;


      return _connectionMultiplexer.GetDatabase(_options.Database);

    }




    public string RegionName { get; private set; }
    internal RedisNamespace CacheNamespace { get; private set; }

    public int Timeout { get { return Timestamper.OneMs * (int)_lockTimeout.TotalMilliseconds; } }
    //###########################################################################################
    // 
    //
    //
    //
    //
    //###########################################################################################



    /// <summary>
    /// 
    /// </summary>
    /// <param name="regionName"></param>
    /// <param name="connectionMultiplexer"></param>
    /// <param name="options"></param>

    public RedisCache(string regionName, ConnectionMultiplexer connectionMultiplexer, RedisCacheProviderOptions options)
        : this(new RedisCacheConfiguration(regionName), connectionMultiplexer, options)
    {

    }

    public RedisCache(RedisCacheConfiguration configuration, ConnectionMultiplexer connectionMultiplexer, RedisCacheProviderOptions options)
    {
      configuration.ThrowIfNull("configuration")
          .Validate();
      RegionName = configuration.RegionName;
      _expiration = configuration.Expiration;
      _slidingExpiration = configuration.SlidingExpiration;
      _lockTimeout = configuration.LockTimeout;
      _acquireLockTimeout = configuration.AcquireLockTimeout;

      this._connectionMultiplexer = connectionMultiplexer.ThrowIfNull("connectionMultiplexer");
      this._options = options.ThrowIfNull("options")
          .ShallowCloneAndValidate();


      Log.Debug("creating cache: regionName='{0}', expiration='{1}', lockTimeout='{2}', acquireLockTimeout='{3}'",
        RegionName, _expiration, _lockTimeout, _acquireLockTimeout
      );

      options.KeyPrefix = options.KeyPrefix.LastIndexOf(":", StringComparison.Ordinal) > -1
        ? options.KeyPrefix
        : options.KeyPrefix + ":";

      CacheNamespace = new RedisNamespace(options.KeyPrefix + RegionName);
    }

    public long NextTimestamp()
    {

      return Timestamper.Next();
    }

    public virtual void Put(object key, object value)
    {
      key.ThrowIfNull("key");
      value.ThrowIfNull("value");


      Log.Debug("put in cache: regionName='{0}', key='{1}'", RegionName, key);
      try
      {
        var serializedValue = _options.Serializer.Serialize(value);

        var cacheKey = CacheNamespace.GetKey(key);
        var setOfActiveKeysKey = CacheNamespace.GetSetOfActiveKeysKey();
        var db = GetDatabase();
        //TODO 临时不处理，这里缓存了实体
        db.ScriptEvaluate(PutScript, new
        {
          key = cacheKey,
          setOfActiveKeysKey = setOfActiveKeysKey,
          value = serializedValue,
          expiration = _expiration.TotalMilliseconds
        }, FireAndForgetFlags);
      }
      catch (Exception e)
      {
        Log.Error("could not put in cache: regionName='{0}', key='{1}'", RegionName, key);

        var evtArg = new ExceptionEventArgs(RegionName, RedisCacheMethod.Put, e);
        _options.OnException(this, evtArg);
        if (evtArg.Throw)
        {
          throw new RedisCacheException(RegionName, "Failed to put item in cache. See inner exception.", e);
        }
      }
    }

    public virtual object Get(object key)
    {
      key.ThrowIfNull();

      Log.Debug("get from cache: regionName='{0}', key='{1}'", RegionName, key);

      try
      {
        var cacheKey = CacheNamespace.GetKey(key);
        var setOfActiveKeysKey = CacheNamespace.GetSetOfActiveKeysKey();

        var db = GetDatabase();

        var resultValues = (RedisValue[])db.ScriptEvaluate(GetScript, new
        {
          key = cacheKey,
          setOfActiveKeysKey = setOfActiveKeysKey
        });

        if (resultValues[0].IsNullOrEmpty)
        {
          Log.Debug("cache miss: regionName='{0}', key='{1}'", RegionName, key);
          return null;
        }
        else
        {
          var serializedResult = resultValues[0];

          var deserializedValue = _options.Serializer.Deserialize(serializedResult);

          if (deserializedValue != null && _slidingExpiration != RedisCacheConfiguration.NoSlidingExpiration)
          {
            db.ScriptEvaluate(SlidingExpirationScript, new
            {
              key = cacheKey,
              expiration = _expiration.TotalMilliseconds,
              slidingExpiration = _slidingExpiration.TotalMilliseconds
            }, FireAndForgetFlags);
          }

          return deserializedValue;
        }
      }
      catch (Exception e)
      {
        Log.Error("could not get from cache: regionName='{0}', key='{1}'", RegionName, key);

        var evtArg = new ExceptionEventArgs(RegionName, RedisCacheMethod.Get, e);
        _options.OnException(this, evtArg);
        if (evtArg.Throw)
        {
          throw new RedisCacheException(RegionName, "Failed to get item from cache. See inner exception.", e);
        }

        return null;
      }
    }

    public virtual void Remove(object key)
    {
      key.ThrowIfNull();

      Log.Debug("remove from cache: regionName='{0}', key='{1}'", RegionName, key);

      try
      {
        var cacheKey = CacheNamespace.GetKey(key);
        var setOfActiveKeysKey = CacheNamespace.GetSetOfActiveKeysKey();
        var db = GetDatabase();

        db.ScriptEvaluate(RemoveScript, new
        {
          key = cacheKey,
          setOfActiveKeysKey = setOfActiveKeysKey
        }, FireAndForgetFlags);
      }
      catch (Exception e)
      {
        Log.Error("could not remove from cache: regionName='{0}', key='{1}'", RegionName, key);

        var evtArg = new ExceptionEventArgs(RegionName, RedisCacheMethod.Remove, e);
        _options.OnException(this, evtArg);
        if (evtArg.Throw)
        {
          throw new RedisCacheException(RegionName, "Failed to remove item from cache. See inner exception.", e);
        }
      }
    }

    public virtual void Clear()
    {
      Log.Debug("clear cache: regionName='{0}'", RegionName);

      try
      {
        var setOfActiveKeysKey = CacheNamespace.GetSetOfActiveKeysKey();
        var db = GetDatabase();
        db.KeyDelete(setOfActiveKeysKey, FireAndForgetFlags);
      }
      catch (Exception e)
      {
        Log.Error("could not clear cache: regionName='{0}'", RegionName);

        var evtArg = new ExceptionEventArgs(RegionName, RedisCacheMethod.Clear, e);
        _options.OnException(this, evtArg);
        if (evtArg.Throw)
        {
          throw new RedisCacheException(RegionName, "Failed to clear cache. See inner exception.", e);
        }
      }
    }

    public virtual void Destroy()
    {
      // No-op since Redis is distributed.
      Log.Debug("destroying cache: regionName='{0}'", RegionName);
    }

    public virtual void Lock(object key)
    {
      Log.Debug("acquiring cache lock: regionName='{0}', key='{1}'", RegionName, key);

      try
      {
        var lockKey = CacheNamespace.GetLockKey(key);
        var shouldRetry = _options.AcquireLockRetryStrategy.GetShouldRetry();

        var wasLockAcquired = false;
        var shouldTryAcquireLock = true;

        while (shouldTryAcquireLock)
        {
          var lockData = new LockData(
              key: Convert.ToString(key),
              lockKey: lockKey,
              // Recalculated each attempt to ensure a unique value.
              lockValue: _options.LockValueFactory.GetLockValue()
          );

          if (TryAcquireLock(lockData))
          {
            wasLockAcquired = true;
            shouldTryAcquireLock = false;
          }
          else
          {
            var shouldRetryArgs = new ShouldRetryAcquireLockArgs(
                RegionName, lockData.Key, lockData.LockKey,
                lockData.LockValue, _lockTimeout, _acquireLockTimeout
            );
            shouldTryAcquireLock = shouldRetry(shouldRetryArgs);
          }
        }

        if (!wasLockAcquired)
        {
          var lockFailedArgs = new LockFailedEventArgs(
              RegionName, key, lockKey,
              _lockTimeout, _acquireLockTimeout
          );
          _options.OnLockFailed(this, lockFailedArgs);
        }
      }
      catch (Exception e)
      {
        Log.Error("could not acquire cache lock: regionName='{0}', key='{1}'", RegionName, key);

        var evtArg = new ExceptionEventArgs(RegionName, RedisCacheMethod.Lock, e);
        _options.OnException(this, evtArg);
        if (evtArg.Throw)
        {
          throw new RedisCacheException(RegionName, "Failed to lock item in cache. See inner exception.", e);
        }
      }
    }


    public virtual void Unlock(object key)
    {
      // Use Remove() instead of Get() because we are releasing the lock
      // anyways.
      var lockData = _acquiredLocks.Remove(Convert.ToString(key)) as LockData;
      if (lockData == null)
      {
        Log.Warn("attempted to unlock '{0}' but a previous lock was not acquired or timed out", key);
        var unlockFailedEventArgs = new UnlockFailedEventArgs(
            RegionName, key, lockKey: null, lockValue: null
        );
        _options.OnUnlockFailed(this, unlockFailedEventArgs);
        return;
      }

      Log.Debug("releasing cache lock: regionName='{0}', key='{1}', lockKey='{2}', lockValue='{3}'",
          RegionName, lockData.Key, lockData.LockKey, lockData.LockValue
      );

      try
      {
        var db = GetDatabase();

        // Don't use IDatabase.LockRelease() because it uses watch/unwatch
        // where we prefer an atomic operation (via a script).
        var wasLockReleased = (bool)db.ScriptEvaluate(_unlockScript, new
        {
          lockKey = lockData.LockKey,
          lockValue = lockData.LockValue
        });

        if (!wasLockReleased)
        {
          Log.Warn("attempted to unlock '{0}' but it could not be released (it maybe timed out or was cleared in Redis)", lockData);

          var unlockFailedEventArgs = new UnlockFailedEventArgs(
              RegionName, key, lockData.LockKey, lockData.LockValue
          );
          _options.OnUnlockFailed(this, unlockFailedEventArgs);
        }
      }
      catch (Exception e)
      {
        Log.Error("could not release cache lock: regionName='{0}', key='{1}', lockKey='{2}', lockValue='{3}'",
            RegionName, lockData.Key, lockData.LockKey, lockData.LockValue
        );

        var evtArg = new ExceptionEventArgs(RegionName, RedisCacheMethod.Unlock, e);
        _options.OnException(this, evtArg);
        if (evtArg.Throw)
        {
          throw new RedisCacheException(RegionName, "Failed to unlock item in cache. See inner exception.", e);
        }
      }
    }



    public virtual Task<object> GetAsync(object key, CancellationToken cancellationToken)
    {
      object value = null;
      if (!cancellationToken.IsCancellationRequested)
      {
        value = this.Get(key);
      }
      return Task.FromResult<object>(value);
    }

    public virtual Task PutAsync(object key, object value, CancellationToken cancellationToken)
    {
      if (!cancellationToken.IsCancellationRequested)
      {
        this.Put(key, value);
      }
      return Task.FromResult<object>(cancellationToken);
    }

    public virtual Task RemoveAsync(object key, CancellationToken cancellationToken)
    {
      if (!cancellationToken.IsCancellationRequested)
      {
        this.Remove(key);
      }
      return Task.FromResult<object>(cancellationToken);
    }

    public virtual Task ClearAsync(CancellationToken cancellationToken)
    {
      if (!cancellationToken.IsCancellationRequested)
      {
        this.Clear();
      }
      return Task.FromResult<object>(cancellationToken);
    }

    public virtual Task LockAsync(object key, CancellationToken cancellationToken)
    {
      if (!cancellationToken.IsCancellationRequested)
      {
        this.Lock(key);
      }
      return Task.FromResult<object>(cancellationToken);
    }

    public virtual Task UnlockAsync(object key, CancellationToken cancellationToken)
    {
      if (!cancellationToken.IsCancellationRequested)
      {
        this.Unlock(key);
      }
      return Task.FromResult<object>(cancellationToken);
    }
  }
}