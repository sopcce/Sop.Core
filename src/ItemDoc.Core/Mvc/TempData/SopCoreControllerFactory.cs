using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using ItemDoc.Core.Mvc.TempData;

namespace ItemDoc.Core.Mvc
{
    /// <summary>
    /// 替换创建Controller工厂类
    /// </summary>
    public class SopCoreControllerFactory : DefaultControllerFactory
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
                controller.TempDataProvider = new CookieTempDataProvider(requestContext.HttpContext);
            return controller;
        }
    }
}
