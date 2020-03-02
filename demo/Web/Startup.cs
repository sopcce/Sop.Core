﻿
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Common.Logging;
using Sop.Core.Mvc.ModelBinder;
using Sop.Framework.Caching;
using Sop.Framework.Environment;
using Sop.Framework.Repositories;
using Sop.Framework.Repositories.NHibernate;
using Sop.Services.Auth;
using Microsoft.Owin;
using Owin;
using StackExchange.Redis;
using System.Reflection;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(Sop.Web.Startup))]
namespace Sop.Web
{
    public partial class Startup
    {
        private static readonly ILog Logger = LogManager.GetLogger<MvcApplication>();
        private readonly bool _miniProfiler = Config.AppSettings<bool>("MiniProfiler.Enabled", true);
        private static readonly bool RedisCache = Config.AppSettings<bool>("RedisCaching.Enabled", true);
        private static readonly string RedisServer = Config.AppSettings<string>("RedisServerConnectionString", "");
        private static readonly string ServerProxy = Config.AppSettings<string>("ServerProxy", null);

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
        /// <summary>
        /// 初始化DI
        /// </summary>
        /// <param name="assemblies"></param>
        public static void InitializeDiContainer(Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service") && !t.Name.Contains("CacheService"))
              .AsSelf().AsImplementedInterfaces().SingleInstance().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //注册Repository
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance().PropertiesAutowired();
            //注册NHibernate的SessionManager
            builder.Register(@delegate: c => new SessionManager(assemblies)).AsSelf().SingleInstance().PropertiesAutowired();


            //注册缓存服务，每次请求都是一个新的实例
            builder.Register(c => new MemoryCacheManager()).As<ICacheManager>().SingleInstance().PropertiesAutowired();
            //注册Redis服务
            //TODO: 记录redis 服务异常不能启动的时候，应该测试是否可以使用。
            ConfigurationOptions option = new ConfigurationOptions
            {
                AllowAdmin = true,
                AbortOnConnectFail = false,
                SyncTimeout = 6000,
                Password = "sopcce.com.cc2018"
            };
            option.EndPoints.Add("127.0.0.1", 6379);

            //option.EndPoints.Add("127.0.0.1", 6380);
            //option.EndPoints.Add("127.0.0.1", 6381);
            //option.EndPoints.Add("127.0.0.1", 6382);

            try
            {
                #region 尝试启动本机服务
                var serviceControllers = ServiceController.GetServices();
                var listDictionary = new Dictionary<string, ServiceController>();
                foreach (var service in serviceControllers)
                {
                    if (service.ServiceName.ToLower().Contains("redis"))
                    {
                        listDictionary.Add(service.ServiceName.ToLower(), service);
                    }
                }
                if (listDictionary.ContainsKey("redis"))
                {
                    throw new System.Exception("不存在redis的windows服务位于本机，请修改或配置");
                }
                foreach (var info in listDictionary)
                {
                    if (info.Value.Status != ServiceControllerStatus.Running)
                    {
                        info.Value.Start();
                    }
                } 
                #endregion
                var test = ConnectionMultiplexer.Connect(option, null);
                var nn = test.GetStatus();
                if (test.IsConnected == true)
                {

                    builder.Register(c => ConnectionMultiplexer.Connect(option)).SingleInstance().PropertiesAutowired();

                    builder.Register(c => new RedisCacheManager(option)).As<ICacheManager>().SingleInstance().PropertiesAutowired();
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }



            //IAuthenticationService
            builder.Register(c => new OwinAuthenticationService()).As<IAuthenticationService>().PropertiesAutowired().InstancePerRequest();


            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(assemblies).PropertiesAutowired();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModelBinders(assemblies);
            builder.RegisterModelBinderProvider();
            builder.RegisterFilterProvider();
            builder.RegisterModule(new AutofacWebTypesModule());

            builder.RegisterApiControllers(assemblies).PropertiesAutowired();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);


            IContainer container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
            DiContainer.RegisterContainer(container);
        }

        /// <summary>
        /// 初始化MVC环境
        /// </summary>
        public static void InitializeMvc()
        {

            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();


            ValueProviderFactories.Factories.Add(new CookieValueProviderFactory());
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //禁止Response的header信息包含X-AspNetMvc-Version
            MvcHandler.DisableMvcResponseHeader = true;

        }



    }
}



