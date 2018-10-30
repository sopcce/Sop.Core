using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using ItemDoc.Framework.Environment;

namespace ItemDoc.Upload
{
    public static class WebApiConfig
    {
        private const string QueryStringParameterName = "datatype";

        public static void Register(HttpConfiguration config)
        {
            #region 跨域访问

            var allowedMethods =Config.AppSettings<string>("cors:allowedMethods","*"); 
            var allowedOrigin = Config.AppSettings<string>("cors:allowedOrigin", "*");
            var allowedHeaders = Config.AppSettings<string>("cors:allowedHeaders", "*"); 
            var gedCors = new EnableCorsAttribute(allowedOrigin, allowedHeaders, allowedMethods)
            {
                SupportsCredentials = true
            };

            config.EnableCors(gedCors);

            #endregion 跨域访问

            //// Web API 特性路由
            config.MapHttpAttributeRoutes();

            //新加的规则
            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //默认返回 json
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
            //    new QueryStringMapping(QueryStringParameterName, "json", "application/json"));
            //返回格式选择xml
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping(QueryStringParameterName, "xml", "application/xml"));
        }
    }
}
