using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ItemDoc.Web
{
  /// <summary>
  /// 路由匹配有特殊到一般  范围有小到大
  /// </summary>
  public static class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
      routes.MapMvcAttributeRoutes();

      #region Item
      //匹配特殊路由
      routes.MapRoute(
        name: "Item_Post",
        url: "P/{id}",
        defaults: new { controller = "Item", action = "Post", id = UrlParameter.Optional }

      );
      //编辑
      routes.MapRoute(
        name: "Item_PostEdit",
        url: "Item/PostEdit/{catalogId}/{id}",
        defaults: new { controller = "Item", action = "PostEdit", catalogId = 0, id = UrlParameter.Optional }
      );
      #endregion


      #region User 
      //
      routes.MapRoute(
        name: "User_Home",
        url: "u/{userName}",
        defaults: new { controller = "User", action = "Home", userName = UrlParameter.Optional }
      );


      #endregion









      //安装模块
      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new
          {
            controller = "Install",
            action = "Index",
            id = UrlParameter.Optional
          }
      );





    }
  }
}
