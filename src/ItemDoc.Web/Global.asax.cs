
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using StackExchange.Profiling;
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
using Common.Logging;
using ItemDoc.Core;
using ItemDoc.Core.Auth;
using ItemDoc.Core.Mvc.ModelBinder;
using ItemDoc.Framework.Caching;
using ItemDoc.Framework.Environment;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Web
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
      Assembly[] assemblies = files.Select(n => Assembly.Load(AssemblyName.GetAssemblyName(n))).ToArray();

      //初始化DI容器
      InitializeDiContainer(assemblies);
      //ItemDoc.Services.Mapping.
      //初始化MVC环境
      InitializeMVC();

      

    }

    protected void Application_BeginRequest()
    {
      if (Request.IsLocal || _miniProfiler)
        MiniProfiler.Start();
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
      //关闭stop
      if (Request.IsLocal || _miniProfiler)
        MiniProfiler.Stop();
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


      builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Repositories"))
        .AsSelf().AsImplementedInterfaces().SingleInstance().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);



      builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service") && !t.Name.Contains("CacheService"))
        .AsSelf().AsImplementedInterfaces().SingleInstance().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);




      builder.RegisterGeneric(typeof(PocoRepository<>)).As(typeof(IRepository<>)).SingleInstance().PropertiesAutowired();


      //builder.Register(c => new FormsAuthenticationService()).As<IAuthenticationService>()
      //  .PropertiesAutowired().InstancePerRequest();

      builder.Register(c => new OwinAuthenticationService()).As<IAuthenticationService>()
        .PropertiesAutowired().InstancePerRequest();


      builder.Register(c => new MemoryCacheManager()).As<ICacheManager>().SingleInstance().PropertiesAutowired();

      if (RedisCache)
      {
        try
        {
          builder.Register(c => new RedisCacheManager(RedisServer)).As<ICacheManager>().SingleInstance().PropertiesAutowired();
        }
        catch (Exception ex)
        {
          //todo: 写入日志文件 
          var msg = ex.Message;
          Response.Redirect(SiteUrls.Instance().Error404());
        }

      }
    


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
      MvcHandler.DisableMvcResponseHeader = true;
    }
    
  
  }
}