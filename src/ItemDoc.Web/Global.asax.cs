
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
            files = files.Union(Directory.EnumerateFiles(HttpRuntime.BinDirectory, "Sop.Common.*.dll"));
            Assembly[] assemblies = files.Select(n => Assembly.Load(AssemblyName.GetAssemblyName(n))).ToArray();

            //初始化DI容器
            Startup.InitializeDiContainer(assemblies);
            //ItemDoc.Services.Mapping.
            //初始化MVC环境
            Startup.InitializeMvc();

            MiniProfiler.Configure(new MiniProfilerOptions
            {

                RouteBasePath = "~/profiler",
                SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter(),

                PopupRenderPosition = RenderPosition.Right,  // defaults to left
                PopupMaxTracesToShow = 15,                   // defaults to 15


                ResultsAuthorize = request => request.IsLocal,


                ResultsListAuthorize = request =>
                {

                    return true; // all requests are legit in this example
                },

                // Stack trace settings
                StackMaxLength = 256, // default is 120 characters

                // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                // (defaults to true, and connection opening/closing is tracked)
                TrackConnectionOpenClose = true
            }
                    // Optional settings to control the stack trace output in the details pane, examples:
                    .ExcludeType("SessionFactory")  // Ignore any class with the name of SessionFactory)
                    .ExcludeAssembly("NHibernate")  // Ignore any assembly named NHibernate
                    .ExcludeMethod("Flush")         // Ignore any method with the name of Flush

            );

            // If we're using EntityFramework 6, here's where it'd go.
            // This is in the MiniProfiler.EF6 NuGet package.
            // MiniProfilerEF6.Initialize();

        }

        protected void Application_BeginRequest()
        {

            if (Request.IsLocal && _miniProfiler)
                MiniProfiler.StartNew();
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
                MiniProfiler.Current?.Stop(); // Be sure to stop the profiler!

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


 


    }
}