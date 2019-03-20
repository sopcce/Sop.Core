using ItemDoc.Framework.Environment;
using ItemDoc.Framework.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{
  public class SopTable
  {
    #region Instance

    private static volatile SopTable _instance = null;
    private static readonly object Lock = new object();

    /// <summary>
    /// SiteUrls单例实体
    /// </summary>
    /// <returns></returns>
    public static SopTable Instance()
    {
      if (_instance == null)
      {
        lock (Lock)
        {
          if (_instance == null)
          {
            _instance = new SopTable();
          }
        }
      }
      return _instance;
    }

    #endregion Instance

    /// <summary>
    /// The default mapper
    /// </summary>
    private readonly IMapper _defaultMapper;

    public SopTable()
    {
      _defaultMapper = _defaultMapper ?? new ConventionMapper();
    }
    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public string GetTableName<T>()
    {
      string tableName = string.Empty;
      try
      {
        var pocoData = PocoData.ForType(typeof(T), _defaultMapper);

        var infoTable = pocoData?.TableInfo;
        if (infoTable != null)
          tableName = infoTable.TableName;
      }
      catch (Exception ex)
      {
        var exinfo = ex;
        if (tableName == null)
          tableName = null;
      }
      finally
      {
        if (string.IsNullOrWhiteSpace(tableName))
        {
          var tableNameAttribute = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), false)[0] as TableNameAttribute;
          if (tableNameAttribute != null)
            tableName = tableNameAttribute.Value;
        }
      }
      return tableName;

    }
    public TableInfo GetTableInfo<T>()
    {
      TableInfo infoTable = null;
      try
      {
        var pocoData = PocoData.ForType(typeof(T), _defaultMapper);
        infoTable = pocoData?.TableInfo;
        if (infoTable == null && string.IsNullOrWhiteSpace(infoTable.PrimaryKey))
        {
          infoTable = new TableInfo() { PrimaryKey = "Id", AutoIncrement = true };
        }

      }
      catch (Exception ex)
      {
        //
      }
      return infoTable;

    }





    /// <summary>
    /// Gets the column list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IList<string> GetColumnList<T>()
    {
      IList<string> list = new List<string>();
      try
      {
        var pocoData = PocoData.ForType(typeof(T), _defaultMapper);

        var columns = pocoData?.Columns;
        if (columns != null)
        {
          foreach (var column in columns)
          {
            list.Add(column.Key);
          }
        }

      }
      catch (Exception)
      {
        list = null;
      }
      finally
      {
        if (list == null)
        {
          list = new List<string>
          {
            "*"
          };
        }
      }
      return list;


    }
    /// <summary>
    /// Gets the column string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetColumnStr<T>()
    {
      var colmuns = SopTable.Instance().GetColumnList<T>();
      return string.Join(",", colmuns);
    }

    

    public static  string GetColumnStr<T>(T t)
    {
      string tStr = string.Empty;
      if (t == null)
      {
        return tStr;
      }
      PropertyInfo[] properties = t.GetType().GetProperties(bindingAttr: BindingFlags.Instance | BindingFlags.Public);

      if (properties.Length <= 0)
      {
        return tStr;
      }
      foreach (System.Reflection.PropertyInfo item in properties)
      {
        string name = item.Name;
        object value = item.GetValue(t, null);
        if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
        {
          tStr += string.Format("{0}:{1},", name, value);
        }
        else
        {
          GetColumnStr(value);
        }
      }
      return tStr;
    }

  }

}
