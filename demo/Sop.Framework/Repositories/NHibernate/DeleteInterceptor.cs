using System;
using System.Text.RegularExpressions;
using NHibernate;
using NHibernate.SqlCommand;

namespace Sop.Framework.Repositories.NHibernate
{
    public class DeleteInterceptor : EmptyInterceptor
    {
        private static readonly Regex Regex = new Regex("\\s+from\\s+([^\\s]+)\\s+([^\\s]+)\\s+");

        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Match match = Regex.Match(sql.ToString());
            String tableName = match.Groups[1].Value;
            String tableAlias = match.Groups[2].Value;

            sql = sql.Substring(match.Groups[2].Index);
            sql = sql.Replace(tableAlias, tableName);
            sql = sql.Insert(0, "delete from ");

            Int32 orderByIndex = sql.IndexOfCaseInsensitive(" order by ");

            if (orderByIndex > 0)
            {
                sql = sql.Substring(0, orderByIndex);
            }

            int limitIndex = sql.IndexOfCaseInsensitive(" limit ");
            if (limitIndex > 0)
            {
                sql = sql.Substring(0, limitIndex);
            }

            return sql;
        }
    }
}
