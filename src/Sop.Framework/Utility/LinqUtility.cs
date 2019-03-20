 
using System.Data.Objects;

namespace Sop.Framework.Utility
{
    public class LinqUtility
    {
        /// <summary>
        /// 获取 T-SQL
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static  string ToSql(object o)
        {
            var q = o as ObjectQuery;
            return q != null ? q.ToTraceString() : string.Empty;
        }
    }
}
