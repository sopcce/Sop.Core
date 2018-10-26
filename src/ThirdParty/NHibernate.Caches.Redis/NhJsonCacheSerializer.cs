using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Caches.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NHibernate.Caches.Redis
{
  public class NhJsonCacheSerializer : ICacheSerializer
  {
    // By default, JSON.NET will always use Int64/Double when deserializing numbers
    // since there isn't an easy way to detect the proper number size. However,
    // because NHibernate does casting to the correct number type, it will fail.
    // Adding the type to the serialize object is what the "TypeNameHandling.All"
    // option does except that it doesn't include numbers.
    private class KeepNumberTypesConverter : JsonConverter
    {
      // We shouldn't have to account for Nullable<T> because the serializer
      // should see them as null.
      private static readonly ISet<System.Type> numberTypes = new HashSet<System.Type>(new[]
  {
            typeof(Byte), typeof(SByte),
            typeof(UInt16), typeof(UInt32), typeof(UInt64),
            typeof(Int16), typeof(Int32), typeof(Int64),
            typeof(Single), typeof(Double), typeof(Decimal)
        });

      public override bool CanConvert(System.Type objectType)
      {
        return numberTypes.Contains(objectType);
      }

      // JSON.NET will deserialize a value with the explicit type when 
      // the JSON object exists with $type/$value properties. So, we 
      // don't need to implement reading.
      public override bool CanRead { get { return false; } }

      public override bool CanWrite { get { return true; } }

      public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
      {
        // CanRead is false.
        throw new NotImplementedException();
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        writer.WriteStartObject();
        writer.WritePropertyName("$type");
        var typeName = value.GetType().FullName;
        writer.WriteValue(typeName);
        writer.WritePropertyName("$value");
        writer.WriteValue(value);
        writer.WriteEndObject();
      }
    }

    private class CustomContractResolver : DefaultContractResolver
    {
      private static readonly ISet<System.Type> nhibernateCacheObjectTypes = new HashSet<System.Type>(new[]
  {
            typeof(CachedItem),
            typeof(CacheLock),
            typeof(CacheEntry),
            typeof(CollectionCacheEntry)
        });

      protected override JsonObjectContract CreateObjectContract(System.Type objectType)
      {
        var result = base.CreateObjectContract(objectType);

        // JSON.NET uses the default constructor (or uninitialized objects)
        // by default. Since Point is immutable, we must use the parameterized
        // constructor. Since we don't need to take a dependency on JSON.NET
        // in the model, we can't use [JsonConstructor]. It is explicitly 
        // emulated here.
        if (objectType == typeof(Point))
        {
          result.CreatorParameters.Add(new JsonProperty()
          {
            PropertyName = "x",
            PropertyType = typeof(Int32)
          });
          result.CreatorParameters.Add(new JsonProperty()
          {
            PropertyName = "y",
            PropertyType = typeof(Int32)
          });
          result.OverrideCreator = (args) =>
          {
            return new Point((Int32)args[0], (Int32)args[1]);
          };
        }
        // By default JSON.NET will only use the public constructors that 
        // require parameters such as ISessionImplementor. Because the 
        // NHibernate cache objects use internal constructors that don't 
        // do anything except initialize the fields, it's much easier 
        // (no constructor lookup) to just get an uninitialized object and 
        // fill in the fields.
        else if (nhibernateCacheObjectTypes.Contains(objectType))
        {
          result.DefaultCreator = () => FormatterServices.GetUninitializedObject(objectType);
        }

        return result;
      }

      protected override IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
      {
        if (nhibernateCacheObjectTypes.Contains(type))
        {
          // By default JSON.NET will serialize the NHibernate objects with
          // their public properties. However, the backing fields/property
          // names don't always match up. Therefore, we *only* use the fields
          // so that we can get/set the correct value.
          var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              .Select(f => base.CreateProperty(f, memberSerialization));

          var result = fields.Select(p =>
          {
            p.Writable = true;
            p.Readable = true;
            return p;
          }).ToList();
          return result;
        }
        else
        {
          return base.CreateProperties(type, memberSerialization);
        }
      }
    }

    private readonly JsonSerializerSettings settings;

    public NhJsonCacheSerializer()
    {
      this.settings = new JsonSerializerSettings();
      //这一行就是设置Json.NET能够序列化接口或继承类的关键，
      //将TypeNameHandling设置为All后，Json.NET会在序列化后的json文本中附加一个属性说明json到底是从什么类序列化过来的，也可以设置TypeNameHandling为Auto，表示让Json.NET自动判断是否需要在序列化后的json中添加类型属性，如果序列化的对象类型和声明类型不一样的话Json.NET就会在json中添加类型属性，反之就不添加，
      //但是我发现TypeNameHandling.Auto有时候不太好用。。。
      settings.TypeNameHandling = TypeNameHandling.All;
      settings.Converters.Add(new KeepNumberTypesConverter());
      settings.ContractResolver = new CustomContractResolver();
    }

    public RedisValue Serialize(object value)
    {
      if (value == null) return RedisValue.Null;

      var result = JsonConvert.SerializeObject(value, Formatting.None, settings);
      return result;
    }

    public object Deserialize(RedisValue value)
    {
      if (value.IsNull) return null;

      var result = JsonConvert.DeserializeObject(value, settings);
      return result;
    }
  }
}
