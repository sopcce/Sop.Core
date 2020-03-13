using Autofac;
using Autofac.Core;

namespace Sop.Data.Container
{
  /// <summary>
  ///     依赖注入容器对Autofac进行封装
  /// </summary>
  public class DiContainer
    {
        private static IContainer _container;

        /// <summary>
        ///     注册DIContainer
        /// </summary>
        /// <param name="container">Autofac.IContainer</param>
        public static void RegisterContainer(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///     获取IContainer
        /// </summary>
        /// <returns>Autofac.IContainer</returns>
        public static IContainer GetContainer()
        {
            return _container;
        }

        /// <summary>
        ///     按类型获取组件
        /// </summary>
        /// <typeparam name="TService">组件类型</typeparam>
        /// <returns>返回获取的组件</returns>
        public static TService Resolve<TService>()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                return scope.Resolve<TService>();
            }
        }

        /// <summary>
        ///     按名称获取组件
        /// </summary>
        /// <typeparam name="TService">组件类型</typeparam>
        /// <param name="serviceName">组件名称</param>
        /// <returns>返回获取的组件</returns>
        public static TService ResolveNamed<TService>(string serviceName)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                return _container.ResolveNamed<TService>(serviceName);
            }
        }

        /// <summary>
        ///     按参数获取组件
        /// </summary>
        /// <typeparam name="TService">组件类型</typeparam>
        /// <param name="parameters">
        ///     <see cref="Autofac.Core.Parameter" />
        /// </param>
        /// <returns>返回获取的组件</returns>
        public static TService Resolve<TService>(params Parameter[] parameters)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                return _container.Resolve<TService>(parameters);
            }
        }

        /// <summary>
        ///     按key获取组件
        /// </summary>
        /// <typeparam name="TService">组件类型</typeparam>
        /// <param name="serviceKey">枚举类型的Key</param>
        /// <returns>返回获取的组件</returns>
        public static TService ResolveKeyed<TService>(object serviceKey)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                return _container.ResolveKeyed<TService>(serviceKey);
            }
        }
    }
}