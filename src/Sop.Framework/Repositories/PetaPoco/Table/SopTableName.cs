using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  /// <seealso cref="TableNameAttribute" />
  public class SopTableName : TableNameAttribute
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName"></param>
    public SopTableName(string tableName) :
        base(tableName)
    {

    }

  }









}
