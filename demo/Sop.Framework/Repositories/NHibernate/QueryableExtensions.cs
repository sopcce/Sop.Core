using System;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Linq;

namespace Sop.Framework.Repositories.NHibernate
{
  public static class QueryableExtensions
  {
    #region Private static readonly fields
    private static readonly PropertyInfo sessionProperty = typeof(DefaultQueryProvider).GetProperty("Session", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
    private static readonly FieldInfo batcherInterceptorField = typeof(AbstractBatcher).GetField("_interceptor", BindingFlags.NonPublic | BindingFlags.Instance);
    private static readonly FieldInfo sessionImplInterceptorField = typeof(SessionImpl).GetField("interceptor", BindingFlags.NonPublic | BindingFlags.Instance);
    #endregion

    #region Public extension methods
    /// <summary>
    /// /
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    public static void Delete<T>(this IQueryable<T> queryable)
    {
      if (queryable.GetType().GetGenericTypeDefinition() == typeof(NhQueryable<>))
      {
        //todo:输出sql不正确
        ISessionImplementor impl = sessionProperty.GetValue(queryable.Provider, null) as ISessionImplementor;
        IInterceptor oldInterceptor = sessionImplInterceptorField.GetValue(impl) as IInterceptor;
        IInterceptor deleteInterceptor = new DeleteInterceptor();

          if (impl != null)
          {
              batcherInterceptorField.SetValue(impl.Batcher, deleteInterceptor);

              queryable.Any();

              batcherInterceptorField.SetValue(impl.Batcher, oldInterceptor);
          }
      }
      else
      {
        throw (new ArgumentException("Invalid type", "queryable"));
      }
    }
    #endregion
  }
}
