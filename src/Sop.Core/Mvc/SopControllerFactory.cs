using System.Web.Mvc;
using System.Web.Routing;

namespace Sop.Core.Mvc
{
    /// <summary>
    /// 替换创建Controller工厂类
    /// </summary>
    public class SopControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// 创建Controller
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController iController = base.CreateController(requestContext, controllerName);
            Controller controller = iController as Controller;
            if (iController != null)
                controller.TempDataProvider = new TempDataCookieProvider(requestContext.HttpContext);
            return controller;
        }
    }
}
