//using System;
//using System.Web.Mvc;

//namespace System.Web.Mvc
//{
//    /// <summary>
//    /// Controller扩展方法
//    /// </summary>
//    public static class ControllerExtensions
//    {
//        public static ActionResult NoPermissionResult(this Controller controller)
//        {
//            //post请求时，如果是异步的，返回值是json格式
//            if (controller.Request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase))
//            {
//                if (controller.Request.IsAjaxRequest())
//                {
//                    return new JsonResult() { Data = new SystemMessageData(SystemMessageType.Error, "无权操作") };
//                }
//            }
//            else
//            {
//                //如果是页面子请求，例如页面侧边栏功能区块，则直接返回空页面
//                if (controller.ControllerContext.IsChildAction)
//                {
//                    return new EmptyResult();
//                }

//                if (controller.Request.IsAjaxRequest())
//                {
//                    if (controller.Request.ContentType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
//                    {
//                        return new JsonResult() { Data = new SystemMessageData(SystemMessageType.Error, "无权操作") };
//                    }
//                    else
//                    {
//                        return new RedirectResult(SiteUrls.Instance().SystemMessage(controller.TempData, new SystemMessage
//                        {
//                            Title = "无权操作",
//                            Body = "你没有被授权查看该内容，请与管理员联系。",
//                            SystemMessageType = SystemMessageType.Error
//                        }, true));
//                    }
//                }
//            }

//            return new RedirectResult(SiteUrls.Instance().SystemMessage(controller.TempData, new SystemMessage
//            {
//                Title = "无权操作",
//                Body = "你没有被授权查看该内容，请与管理员联系。",
//                SystemMessageType = SystemMessageType.Error
//            }));
//        }
//    }
//}