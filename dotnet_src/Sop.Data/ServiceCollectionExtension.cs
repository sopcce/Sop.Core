using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Sop.Data;
using Sop.Data.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public static class ServiceCollectionExtension
    {
        #region private

        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        private static void AddDefault(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var allAssemblies = AppDomain.CurrentDomain.GetCurrentPathAssembly();
            foreach (var assembly in allAssemblies)
            {
                var types = assembly.GetTypes()
                                    .Where(type => type.IsClass
                                                   && type.BaseType != null
                                                   && type.HasImplementedRawGeneric(typeof(IRepository<>)));
                foreach (var type in types)
                {
                    var interfaces = type.GetInterfaces();

                    var interfaceType = interfaces.FirstOrDefault(x => x.Name == $"I{type.Name}");
                    if (interfaceType == null) interfaceType = type;
                    var serviceDescriptor = new ServiceDescriptor(interfaceType, type, ServiceLifetime.Scoped);
                    if (!services.Contains(serviceDescriptor)) services.Add(serviceDescriptor);
                }
            }

            services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
        }

        #endregion

        #region ServiceCollectionExtension

        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSopData(this IServiceCollection services,
                                                    Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<BaseDbContext>(options);
            services.AddScoped<DbContext, BaseDbContext>();
            AddDefault(services);
            return services;
        }

        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSopData(this ContainerBuilder builder,
                                                  Action<DbContextOptionsBuilder> options)
        {
            var services = new ServiceCollection();
            services.AddDbContext<BaseDbContext>(options);
            services.AddScoped<DbContext, BaseDbContext>();
            AddDefault(services);
            builder.Populate(services);
            return builder;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSopData<T>(this IServiceCollection services,
                                                       Action<DbContextOptionsBuilder> options) where T : BaseDbContext
        {
            services.AddDbContext<T>(options);
            services.AddScoped<DbContext, T>();
            AddDefault(services);
            return services;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSopData<T>(this ContainerBuilder builder,
                                                     Action<DbContextOptionsBuilder> options) where T : BaseDbContext
        {
            var services = new ServiceCollection();
            services.AddDbContext<T>(options);
            services.AddScoped<DbContext, T>();
            AddDefault(services);
            builder.Populate(services);
            builder.Build();
            return builder;
        }

        #endregion
    }
}