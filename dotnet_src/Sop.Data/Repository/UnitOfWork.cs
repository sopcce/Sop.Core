using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sop.Data.Dapper;

namespace Sop.Data.Repository
{
    /// <summary>
    ///     Represents the default implementation of the <see cref="IUnitOfWork" />
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly Regex _rexOrderBy = new Regex(@"\s+ORDER\s+BY\s+([^\s]+(?:\s+ASC|\s+DESC)?)\s*$",
                                                              RegexOptions.IgnoreCase | RegexOptions.Singleline |
                                                              RegexOptions.Compiled);

        private readonly DbContext _context;
        private bool _disposed;

        /// <summary>
        ///     Initializes a new instance of the
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


        public Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object param = null,
                                                              IDbContextTransaction trans = null) where TEntity : class
        {
            var conn = GetConnection();
            return conn.QueryAsync<TEntity>(sql, param, trans?.GetDbTransaction());
        }


        public async Task<int> ExecuteAsync(string sql, object param, IDbContextTransaction trans = null)
        {
            var conn = GetConnection();
            return await conn.ExecuteAsync(sql, param, trans?.GetDbTransaction());
        }


        public async Task<IPageList<TEntity>> QueryPageListAsync<TEntity>(
            int pageIndex, int pageSize, string sql, object param = null) where TEntity : class
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var connection = GetConnection();
            var query = string.Empty;

            var tp = typeof(TEntity).GetProperties()
                                    .Where(p => p.GetCustomAttributes(true)
                                                 .Any(attr => attr.GetType().Name == typeof(KeyAttribute).Name))
                                    .ToList();
            var orderBySql = tp.Any()
                ? tp.FirstOrDefault()?.Name
                : typeof(TEntity).GetProperties().Where(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                                ?.FirstOrDefault()?.Name;

            var match = _rexOrderBy.Match(sql);
            if (match.Success)
            {
                sql = sql.Substring(0, match.Index);
                orderBySql = match.Groups[1].Value;
            }


            var tempPageIndex = (pageIndex - 1) * pageSize;
            if (_context.Database.IsMySql())
                query = $"{sql} LIMIT {tempPageIndex},{pageSize}";
            if (_context.Database.IsSqlServer())
                query =
                    $" select top {pageSize} * from ( select row_number() over(order by {orderBySql}) as row_number,* from ( {sql}) as u) temp_row where row_number>{tempPageIndex}  ";

            #region SQ

            // SELECT* FROM(select row_number() over(order by PersonnelId) as row_number,* from PE_Model_Personnel) AS u   WHERE row_number BETWEEN 10 AND 1000 
            // SELECT ROW_NUMBER() OVER(ORDER BY[PersonnelId] DESC) AS num,* FROM PE_Model_Personnel  ORDER BY num DESC 
            //OFFSET 10 ROWS FETCH NEXT 1000 ROWS ONLY
            // select top 10 * from ( select row_number() over(order by PersonnelId) as row_number,* from ( ) as u) temp_row where row_number>(22-1)*10;

            #endregion

            var items = await connection.QueryAsync<TEntity>(query, param);
            var pagedList = new PageList<TEntity>(items.AsQueryable(), pageIndex - 1, pageSize);
            return pagedList;
        }


        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }


        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDbConnection GetConnection()
        {
            return _context.Database.GetDbConnection();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }
    }
}