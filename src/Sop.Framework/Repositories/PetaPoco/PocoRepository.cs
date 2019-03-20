using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ItemDoc.Framework.Caching;
using ItemDoc.Framework.Environment;

namespace ItemDoc.Framework.Repositories
{
  /// <summary>
  /// 对PetaPoco封装，用于数据访问
  /// 1、使用linq查询
  /// </summary>
  /// <typeparam name="T">实体类型</typeparam>
  public partial class PocoRepository<T> : IRepository<T> where T : class
  {
    /// <summary>
    /// 数据库database对象
    /// </summary>
    private Database _database;

    private const string cachekey = "ItemDoc:table:{0}";
    /// <summary>
    /// 
    /// </summary>
    private readonly object _lock = new object();
    /// <summary>
    /// 
    /// </summary>
    public PocoRepository()
    {
      Database();
    }
    /// <summary>
    /// Gets the database.
    /// </summary>
    /// <value>
    /// The database.
    /// </value>
    Database IRepository<T>.Database => _database;


    /// <summary>
    /// 默认PetaPocoDatabase实例,为空是默认使用第一个connectionStringName
    /// </summary>
    /// <returns>Database</returns>
    public Database Database(string connectionStringName = null)
    {

      int connectionStringsCount = ConfigurationManager.ConnectionStrings.Count;
      if (connectionStringName == null && connectionStringsCount > 0)
      {
        int value = (connectionStringsCount - 1) > 0 ? (connectionStringsCount - 1) : 0;
        string connectionStringsName = ConfigurationManager.ConnectionStrings[value].Name;
        _database = new Database(connectionStringsName);

      }
      else
      {
        _database = new Database(connectionStringName);
      }

      return _database;
    }

    /// <summary>
    /// 获取单个实体
    /// </summary>
    /// <param name="id">主键ID</param>
    /// <returns>Entity</returns>
    public virtual T GetById(object id)
    {
      T entity = Database().SingleOrDefault<T>(id);
      return entity;
    }
    /// <summary>
    /// 添加实体
    /// </summary>
    /// <param name="entity">实体</param>
    public virtual object Insert(T entity)
    {
      try
      {
        if (entity != null)
          return Database().Insert(entity);
      }
      catch (Exception e)
      {
        if (new Database().OnException(e))
          throw;
      }
      return null;
    }
    /// <summary>
    /// 添加实体集合
    /// </summary>
    /// <param name="entities">实体集合</param>
    public virtual object Insert(IEnumerable<T> entities)
    {

      if (entities != null)
      {
        try
        {
          List<object> _objects = new List<object>();
          using (var scope = Database().GetTransaction())
          {
            foreach (var entity in entities)
            {
              try
              {
                _objects.Add(Database().Insert(entity));
              }
              catch (Exception)
              {
                Database().AbortTransaction();
              }
            }
            scope.Complete();
          }

          return _objects;

        }
        catch (Exception e)
        {
          if (new Database().OnException(e))
            throw;
        }
      }
      return null;
    }

    /// <summary>
    /// 把实体entiy更新到数据库
    /// </summary>
    /// <param name="entity">实体</param>
    public virtual object Update(T entity)
    {

      try
      {
        if (entity != null)
          return Database().Update(entity);
      }
      catch (Exception e)
      {
        if (new Database().OnException(e))
          throw;
      }
      return null;

    }

    /// <summary>
    /// 修改实体集合
    /// </summary>
    /// <param name="entities">实体集合</param>
    public virtual object Update(IEnumerable<T> entities)
    {
      if (entities != null)
      {
        try
        {
          List<object> _objects = new List<object>();
          using (var scope = Database().GetTransaction())
          {
            foreach (var entity in entities)
            {
              try
              {
                _objects.Add(Database().Update(entity));
              }
              catch (Exception)
              {
                Database().AbortTransaction();
              }
            }
            scope.Complete();
          }

          return _objects;

        }
        catch (Exception e)
        {
          if (new Database().OnException(e))
            throw;
        }
      }
      return null;
    }
    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="entity">实体</param>
    public virtual object Delete(T entity)
    {
      try
      {
        if (entity != null)
          return Database().Delete(entity);
      }
      catch (Exception e)
      {
        if (new Database().OnException(e))
          throw;
      }
      return null;

    }

    /// <summary>
    /// 删除实体集合d
    /// </summary>
    /// <param name="entities">实体集合</param>
    public virtual object Delete(IEnumerable<T> entities)
    {
      if (entities != null)
      {
        try
        {
          List<object> _objects = new List<object>();
          using (var scope = Database().GetTransaction())
          {
            foreach (var entity in entities)
            {
              try
              {
                _objects.Add(Database().Delete(entity));
              }
              catch (Exception)
              {
                Database().AbortTransaction();
              }
            }
            scope.Complete();
          }

          return _objects;

        }
        catch (Exception e)
        {
          if (new Database().OnException(e))
            throw;
        }
      }
      return null;
    }


    /// <summary>
    /// 获取所有实体（仅用于数据量少的情况）
    /// </summary>
    /// <returns>返回所有实体集合</returns>
    public virtual IQueryable<T> Table
    {
      get
      {
        try
        {

          var tableName = SopTable.Instance().GetTableName<T>();
          var sql = Sql.Builder
            .Select(SopTable.GetColumnStr<T>())
            .From(tableName);
          //TODO 建议使用分页异步读取,读取之后存放到集合中。
          var sqlcount = Sql.Builder
             .Select("COUNT(*)")
             .From(tableName);

          var pageSize = 500;
          //bool LoadCache = true;
          List<T> lists = new List<T>();
          var TotalCount = Database().ExecuteScalar<int>(sqlcount);
          var TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

          #region 缓存，暂时注释掉，后期完善
          //var cacheManager = DiContainer.Resolve<ICacheManager>();
          //if (LoadCache && TotalPages > 10)
          //{
          //  if (cacheManager != null)
          //  {
          //    var list = cacheManager.Get<IEnumerable<T>>(string.Format(cachekey, tableName));
          //    if (list != null && list.Count() > 0 && TotalCount == list.Count())
          //    {
          //      return list.AsQueryable();
          //    }
          //    else
          //    {
          //      cacheManager.Remove(string.Format(cachekey, tableName));
          //    }
          //  }
          //} 
          #endregion

          #region 读取数据源 
          for (int i = 0; i < TotalPages; i++)
          {
            var table = Database().SkipTake<T>(i * pageSize, pageSize, sql);
            lists.AddRange(table);
          }
          #endregion

          #region 设置缓存 //TODO 暂时注释掉，后期完善
          //if (cacheManager != null)
          //{
          //  cacheManager.Set(string.Format(cachekey, tableName), lists, TimeSpan.FromDays(365));
          //}
          #endregion
          return lists.AsQueryable();
        }
        catch (Exception e)
        {
          if (SopException.OnException(e))
            throw;
          return null;
        }

      }
    }


    public virtual IPageList<T> Gets(int pageIndex, int pageSize, string sql, params object[] args)
    {
      var dao = Database();
      dao.CommandTimeout = 120;
      var tableName = SopTable.Instance().GetTableName<T>();
      if (string.IsNullOrWhiteSpace(sql))
      {
        var defaultSql = Sql.Builder
          .Select(SopTable.GetColumnStr<T>())
          .From(tableName);
        sql = defaultSql.SQL;
        args = defaultSql.Arguments;
      }
      var list = dao.Page<T>(pageIndex, pageSize, sql, args);

      return list.ToPagedList(pageIndex, pageSize);
    }

    public virtual IPageList<TR> Gets<TR>(int pageIndex, int pageSize, string sql, params object[] args) where TR : class
    {
      var dao = Database();
      dao.CommandTimeout = 120;
      if (string.IsNullOrWhiteSpace(sql))
      {
        return null;
      }
      var list = dao.Page<TR>(pageIndex, pageSize, sql, args);
      return list.ToPagedList(pageIndex, pageSize);
    }




    /// <summary>
    ///     Executes a non-query command
    /// </summary>
    /// <param name="sql">The SQL statement to execute</param>
    /// <param name="args">Arguments to any embedded parameters in the SQL</param>
    /// <returns>The number of rows affected</returns>
    public virtual int Execute(string sql, params object[] args)
    {
      try
      {
        return Database().Execute(sql, args);
      }
      catch (Exception e)
      {
        if (new Database().OnException(e))
          throw;
        return -1;
      }

    }

    /// <summary>
    /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">SQL 查询字符串</param>
    /// <param name="parameters">要应用于 SQL 查询字符串的参数</param>
    /// <returns></returns>
    public virtual IQueryable<T> SqlQuery(string sql, params object[] parameters)
    {
      try
      {
        var entitylists = Database().Query<T>(sql, parameters);
        return entitylists.AsQueryable();


      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }
    }




  }
}