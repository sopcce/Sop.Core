using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Common.Logging;
using ItemDoc.Core.Mvc.ModelBinder;
using ItemDoc.Framework.Caching;
using ItemDoc.Framework.Environment;
using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.Repositories.NHibernate;
using ItemDoc.Services.Auth;
using StackExchange.Profiling;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ItemDoc.Upload
{
 
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetLogger<MvcApplication>();
        private readonly bool _miniProfiler = Config.AppSettings<bool>("MiniProfiler.Enabled", true);
        private static readonly bool RedisCache = Config.AppSettings<bool>("RedisCaching.Enabled", false);
        private static readonly string RedisServer = Config.AppSettings<string>("RedisServerConnectionString", "");



        protected void Application_Start()
        {
            //获取当前相关的程序集
            IEnumerable<string> files = Directory.EnumerateFiles(HttpRuntime.BinDirectory, "ItemDoc.*.dll");
            files = files.Union(Directory.EnumerateFiles(HttpRuntime.BinDirectory, "Sop.Common.*.dll"));
            Assembly[] assemblies = files.Select(n => Assembly.Load(AssemblyName.GetAssemblyName(n))).ToArray();

            //初始化DI容器
            InitializeDiContainer(assemblies);
            //ItemDoc.Services.Mapping.
            //初始化MVC环境
            InitializeMVC();

         
 
        }

        protected void Application_BeginRequest()
        { 
        }


        protected void Application_End(Object source, EventArgs e)
        {
            //记录日志
            Logger.Info("站点已停止");
        }


        protected void Application_EndRequest()
        {
            Response.Headers.Remove("Server");
            //if (Response.StatusCode == 404)
            //{
            //    Response.Redirect(SiteUrls.Instance().Error404());
            //}
            //else if (Response.StatusCode == 500)
            //{
            //    Response.Redirect(SiteUrls.Instance().Error404());
            //}
            
        }


        protected void Application_OnError()
        {
            //将异常记录到日志
            var exception = Server.GetLastError();
            Logger.Info(Request.Url.ToString(), exception);
        }


        protected void Application_PostReleaseRequestState()
        {
            Response.Headers.Remove("Server");
        }


        private void InitializeDiContainer(Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service") && !t.Name.Contains("CacheService"))
              .AsSelf().AsImplementedInterfaces().SingleInstance().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //注册Repository
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance().PropertiesAutowired();

            //注册NHibernate的SessionManager
            builder.Register(@delegate: c => new SessionManager(assemblies)).SingleInstance().PropertiesAutowired();

            //注册Redis服务
            ConfigurationOptions option = new ConfigurationOptions
            {
                AllowAdmin = true,
                AbortOnConnectFail = false,
                SyncTimeout = 6000
            };
            option.EndPoints.Add("47.93.18.104", 6379);
            //option.EndPoints.Add("127.0.0.1", 6380);
            //option.EndPoints.Add("127.0.0.1", 6381);
            //option.EndPoints.Add("127.0.0.1", 6382);
            builder.Register(c => ConnectionMultiplexer.Connect(option)).SingleInstance();

            builder.Register(c => new RedisCacheManager(option)).As<ICacheManager>().SingleInstance().PropertiesAutowired();
            //注册缓存服务，每次请求都是一个新的实例
            builder.Register(c => new MemoryCacheManager()).As<ICacheManager>().SingleInstance().PropertiesAutowired();

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


        private void InitializeMVC()
        {

            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();


            ValueProviderFactories.Factories.Add(new CookieValueProviderFactory());
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //禁止Response的header信息包含X-AspNetMvc-Version
            MvcHandler.DisableMvcResponseHeader = true;
        }


    }
}
