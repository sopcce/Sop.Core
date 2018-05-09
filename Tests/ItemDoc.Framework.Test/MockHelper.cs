
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Moq;
using System.Web.Routing;

namespace Sop.Framework.Test
{
    /// <summary>
    /// 所有模拟对象的工厂
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public static class MockHelper
    {
        /// <summary>
        /// 模拟HttpContextBase
        /// </summary>
        public static HttpContextBase HttpContext()
        {
            
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.Items).Returns(new Hashtable());

          

            return context.Object;
        }

        /// <summary>
        /// 模拟HttpContextBase
        /// </summary>
        public static HttpContextBase GetHttpContext()
        {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            string rawUrl = Uri.UriSchemeHttp.ToString() + "://localhost";
            Uri uri = new Uri(rawUrl);

            mockHttpContext.Setup(o => o.Request.Url).Returns(uri);
            mockHttpContext.Setup(o => o.Request.RawUrl).Returns(rawUrl);
            mockHttpContext.Setup(o => o.Request.PathInfo).Returns(String.Empty);
            mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase)null);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => r);
            mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());
            return mockHttpContext.Object;
        }
        /// <summary>
        /// 模拟RequestContext
        /// </summary>
        /// <returns></returns>
        public static RequestContext GetRequestContext()
        {
            HttpContextBase httpcontext = GetHttpContext();
            RouteData rd = new RouteData();
            return new RequestContext(httpcontext, rd);
        }

        /// <summary>
        /// 模拟ViewContext
        /// </summary>
        /// <param name="viewData">viewData</param>
        /// <returns>ViewContext</returns>
        public static ViewContext GetViewContext(ViewDataDictionary viewData)
        {
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>() { CallBase = true };
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            mockViewContext.Setup(x => x.HttpContext.User.Identity.Name).Returns("TheUser");
            mockViewContext.Setup(c => c.Writer).Returns(new StringWriter());

            return mockViewContext.Object;
        }

        /// <summary>
        /// 模拟IViewDataContainer
        /// </summary>
        /// <param name="viewData">ViewData</param>
        /// <returns>IViewDataContainer</returns>
        public static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData)
        {
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            return mockContainer.Object;
        }

        /// <summary>
        /// 构造虚拟的
        /// </summary>
        /// <param name="url">虚拟的Url只用于构造HttpContext，例如http://www.abc.com</param>
        /// <param name="fileName">HttpRequest中与请求关联的文件名</param>
        /// <param name="querySting">HttpRequest中的查询字符串</param>
        /// <param name="appVPath">应用程序域中的应用程序虚拟路径，需要带/例如，/app</param>
        public static void HttpContextCurrent(string url = "http://www.abc.com", string fileName = "", string querySting = "", string appVPath = "/eg")
        {
            if (!string.IsNullOrEmpty(appVPath))
            {
                AppDomain.CurrentDomain.SetData(".appDomain", "*");
                AppDomain.CurrentDomain.SetData(".appVPath", appVPath);
            }

            System.Web.HttpContext.Current = new HttpContext(
                                    new HttpRequest(fileName, url, querySting),
                                    new HttpResponse(new StringWriter())
                                );
        }

        #region 模拟HtmlHelper
        /// <summary>
        /// 模拟HtmlHelper&lt;object&gt;
        /// </summary>
        /// <returns>HtmlHelper&lt;object&gt;</returns>
        public static HtmlHelper<object> GetHtmlHelper()
        {
            MockHelper.HttpContextCurrent();
            HttpContextBase httpcontext = GetHttpContext();
            RouteCollection rt = GetRouteCollection();
            RouteData rd = GetRouteData();
            ViewContext viewContext = new ViewContext()
            {
                HttpContext = httpcontext,
                RouteData = rd,
                Writer = new StringWriter(),
                ViewData = new ViewDataDictionary()
            };
            IViewDataContainer container = GetViewDataContainer(new ViewDataDictionary());

            HtmlHelper<object> htmlHelper = new HtmlHelper<object>(viewContext, container, rt);
            return htmlHelper;
        }

       
        /// <summary>
        /// 模拟HtmlHelper
        /// </summary>
        /// <typeparam name="TModel">TModel</typeparam>
        /// <param name="viewData">viewData</param>
        /// <returns>HtmlHelper&lt;TModel&gt;</returns>
        public static HtmlHelper<TModel> GetHtmlHelper<TModel>(ViewDataDictionary<TModel> viewData)
        {
            MockHelper.HttpContextCurrent();
            ViewContext viewContext = GetViewContext(viewData);
            IViewDataContainer container = GetViewDataContainer(viewData);
            return new HtmlHelper<TModel>(viewContext, container);
        }

        #endregion

        #region 模拟UrlHelper
        /// <summary>
        /// 模拟UrlHelper
        /// </summary>
        /// <returns>UrlHelper</returns>
        public static UrlHelper GetUrlHelper()
        {
            return GetUrlHelper(GetRouteData(), GetRouteCollection());
        }

        /// <summary>
        /// 模拟UrlHelper
        /// </summary>
        /// <param name="routeData">routeData</param>
        /// <param name="routeCollection">routeCollection</param>
        /// <returns>UrlHelper</returns>
        public static UrlHelper GetUrlHelper(RouteData routeData = null, RouteCollection routeCollection = null)
        {
            MockHelper.HttpContextCurrent();
            HttpContextBase httpcontext = GetHttpContext();
            UrlHelper urlHelper = new UrlHelper(new RequestContext(httpcontext, routeData ?? new RouteData()), routeCollection ?? new RouteCollection());
            return urlHelper;
        }

        /// <summary>
        /// 模拟获取完整路径的UrlHelper<br/>
        /// 主机名为：http://www.mysite.com/
        /// </summary>
        /// <returns>UrlHelper</returns>
        public static UrlHelper GetUrlHelperForIsLocalUrl()
        {
            MockHelper.HttpContextCurrent();
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(context => context.Request.Url).Returns(new Uri("http://www.mysite.com/"));
            RequestContext requestContext = new RequestContext(contextMock.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext);
            return helper;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 模拟RequestContext
        /// </summary>
        /// <param name="routeData">routeData</param>
        /// <returns>RequestContext</returns>
        private static RequestContext GetRequestContext(RouteData routeData)
        {
            HttpContextBase httpcontext = GetHttpContext();
            return new RequestContext(httpcontext, routeData);
        }

        /// <summary>
        /// 模拟RouteCollection
        /// </summary>
        /// <returns>RouteCollection</returns>
        private static RouteCollection GetRouteCollection()
        {
            RouteCollection rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            return rt;
        }

        /// <summary>
        /// 模拟RouteData
        /// </summary>
        /// <returns>RouteData</returns>
        private static RouteData GetRouteData()
        {
            RouteData rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");
            return rd;
        }

        #endregion


    }
}
