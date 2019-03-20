
using System;
using System.Web;
using System.Web.Routing;

namespace Sop.Core.Mvc
{
    /// <summary>
    /// 用于IHttpHandler的RouteHandler
    /// </summary>
    public class HttpHandlerRouteHandler<THttpHandler> : IRouteHandler where THttpHandler : IHttpHandler
    {
        private Func<RequestContext, THttpHandler> _handlerFactory = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handlerFactory"></param>
        public HttpHandlerRouteHandler(Func<RequestContext, THttpHandler> handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        /// <summary>
        /// 获取IHttpHandler
        /// </summary>
        /// <param name="requestContext"><see cref="RequestContext"/></param>
        /// <returns></returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return _handlerFactory(requestContext);
        }
    }
}