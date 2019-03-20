using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sop.Core.Mvc.Handler
{
    public class CustomRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            bool a = Regex.IsMatch(requestContext.HttpContext.Request.Url.Query, string.Format(@"^\?{0}$", "routeInfo"));
           
            if (a)
            {
                OutputRouteDiagnostics(requestContext.RouteData, requestContext.HttpContext);
            }

            var handler = new CustomMvcHandler(requestContext);
            return handler;
        }

        private bool HasQueryStringKey(string keyToTest, HttpRequestBase request)
        {
            return true;
        }


        private void OutputRouteDiagnostics(RouteData routeData, HttpContextBase context)
        {
            var response = context.Response;
            response.Write(
                @"<style>body {font-family: Arial;}
                     table th {background-color: #359; color: #fff;}
              </style>
        <h1>Route Data:</h1>
        <table border='1' cellspacing='0' cellpadding='3'>
            <tr><th>Key</th><th>Value</th></tr>");
            foreach (var pair in routeData.Values)
            {
                response.Write(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", pair.Key, pair.Value));
            }
            response.Write(
                @"</table>
            <h1>Routes:</h1>
            <table border='1' cellspacing='0' cellpadding='3'>
                <tr><th></th><th>Route</th></tr>");

            bool foundRouteUsed = false;
            foreach (Route r in RouteTable.Routes)
            {
                response.Write("<tr><td>");
                bool matches = r.GetRouteData(context) != null;
                string backgroundColor = matches ? "#bfb" : "#fbb";
                if (matches && !foundRouteUsed)
                {
                    response.Write("&raquo;");
                    foundRouteUsed = true;
                }
                response.Write(string.Format(
                    "</td><td style='font-family: Courier New; background-color:{0}'>{1}</td></tr>",
                    backgroundColor, r.Url));
            }

            response.End();
        }
    }

    public class CustomMvcHandler : MvcHandler
    {
        public CustomMvcHandler(RequestContext requestContext)
            : base(requestContext)
        {
        }

        protected override void ProcessRequest(HttpContext httpContext)
        {
            try
            {
                base.ProcessRequest(httpContext);
            }
            catch (Exception exc)
            {
                throw new ApplicationException("RouteUsed:" +
                       RequestContext.RouteData.Route.GetVirtualPath(
                        RequestContext, RequestContext.RouteData.Values), exc);
            }
        }
    }

}