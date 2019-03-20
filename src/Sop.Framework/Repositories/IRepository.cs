using Sop.Framework.Repositories.NHibernate;
using NHibernate;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sop.Framework.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">仓储对应的实体</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// ISession实例
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// 根据Id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(object id);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entity">实体</param>
        object Create(T entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Update(T entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Delete(T entity);

        /// <summary>
        /// 根据查询条件批量删除
        /// </summary>
        /// <param name="predicate">查询条件</param>
        void Delete(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 实体集合
        /// </summary>
        IQueryable<T> Table { get; }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="order">排序</param>
        /// <returns>IQueryable类型的实体集合</returns>
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">第几页</param>
        PageList<T> Gets(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int pageSize, int pageIndex);


    }
}