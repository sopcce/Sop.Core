using ItemDoc.Core.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using ItemDoc.Core.Mvc.SystemMessage;


namespace ItemDoc.Core
{
  /// <summary>
  /// 站点Url配置
  /// </summary>
  public class SiteUrls
  {
    #region Instance

    private static volatile SiteUrls _instance = null;
    private static readonly object Lock = new object();

    /// <summary>
    /// SiteUrls单例实体
    /// </summary>
    /// <returns></returns>
    public static SiteUrls Instance()
    {
      if (_instance == null)
      {
        lock (Lock)
        {
          if (_instance == null)
          {
            _instance = new SiteUrls();
          }
        }
      }
      return _instance;
    }

    #endregion Instance

    #region 信息提示

    /// <summary>
    /// 系统提示信息页面
    /// </summary>
    /// <param name="tempData">临时数据对象</param>
    /// <param name="systemMessage">系统提示信息ViewModel</param>
    /// <param name="inModel">是否显示在模式框中(默认为false)</param>
    /// <returns></returns>
    public string SystemMessage(TempDataDictionary tempData, SystemMessage systemMessage, bool inModel = false)
    {
      if (tempData != null && systemMessage != null)
      {
        tempData.AddOrReplace(Mvc.SystemMessage.SystemMessage.TempDataKey, systemMessage);
      }

      if (inModel)
      {
        return CachedUrlHelper.Action("_SystemMessage", "Components");
      }
      else
      {
        return CachedUrlHelper.Action("SystemMessage", "Components");
      }
    }

    /// <summary>
    /// Http404自定义信息显示页面
    /// </summary>
    /// <returns></returns>
    public string Error404()
    {
      return CachedUrlHelper.Action("Error", "Home");
    }
 


    #endregion

    #region User

    /// <summary>
    /// 账号注册页面
    /// </summary>
    /// <returns></returns>
    public string UserRegister()
    {
      return CachedUrlHelper.Action("Register", "User");
    }

    public string UserHome()
    {
      return CachedUrlHelper.Action("Home", "User");
    }
    public string UserLogin()
    {
      return CachedUrlHelper.Action("Login", "User");
    }
    public string UserSignOut()
    {
      return CachedUrlHelper.Action("SignOut", "User");
    }
    /// <summary>
    /// 验证码功能
    /// </summary>
    /// <returns></returns>

    public string UserCaptchaCode()
    {
      return CachedUrlHelper.Action("CaptchaCode", "User");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string UserValidateCaptchaCode()
    {
      return CachedUrlHelper.Action("User", "ValidateCaptchaCode");
    }





    #endregion

    #region Blogs
    /// <summary>
    /// 博客删除
    /// </summary>
    /// <returns></returns>
    public string BlogsDelete(int id)
    {
      RouteValueDictionary route = new RouteValueDictionary();
      route.Add("id", id);
      return CachedUrlHelper.Action("Delete", "Blogs", route);
    }
    #endregion



    #region Home
    public string HomeAbout()
    {
      return CachedUrlHelper.Action("About", "Home");
    }public string HomeQuestion()
    {
      return CachedUrlHelper.Action("Question", "Home");
    }
    public string HomeContact()
    {
      return CachedUrlHelper.Action("Contact", "Home");
    }

    /// <summary>
    /// 用户首页
    /// </summary>
    /// <returns></returns>
    public string HomeIndex()
    {
      return CachedUrlHelper.Action("Index", "Home");
    }
    #endregion

    #region Tool 

    public string ToolIndex()
    {
      return CachedUrlHelper.Action("Index", "Tool");
    }

    #endregion


    public string ItemIndex()
    {
      return CachedUrlHelper.Action("Index", "Item");
    }
     
    #region ItemCatalog
    public string ItemCatalog(string id)
    {

      RouteValueDictionary route = new RouteValueDictionary();
      if (id != null)
        route.Add("id", id);
      return CachedUrlHelper.Action("Catalog", "Item", route);
    }

    public string ItemCatalogEdit(int? id)
    {
      RouteValueDictionary route = new RouteValueDictionary();
      if (id != null)
        route.Add("id", id);
      return CachedUrlHelper.Action("CatalogEdit", "Item", route);
    }
    public string ItemCatalogEdit()
    {
      return CachedUrlHelper.Action("CatalogEdit", "Item");
    }
    public string ItemCatalogDelete(string ids)
    {
      RouteValueDictionary route = new RouteValueDictionary();
      if (ids != null)
        route.Add("ids", ids);
      return CachedUrlHelper.Action("CatalogDelete", "Item", route);
    }
    public string ItemPost(int? id)
    {
      RouteValueDictionary route = new RouteValueDictionary();
      if (id != null)
        route.Add("id", id);
      return CachedUrlHelper.Action("Post", "Item", route);
    }
    public string ItemCatalogDelete()
    {

      return CachedUrlHelper.Action("CatalogDelete", "Item");
    }

    #endregion
    #region ItemItems  
    public string ItemItemsIndex(int id = 0)
    {

      RouteValueDictionary route = new RouteValueDictionary();
      route.Add("id", id);
      return CachedUrlHelper.Action("ItemsIndex", "Item", route);
    }

    public string ItemItemsEdit(int? id)
    {
      RouteValueDictionary route = new RouteValueDictionary();
      if (id != null)
        route.Add("id", id);
      return CachedUrlHelper.Action("ItemsEdit", "Item", route);
    }
    public string ItemItemsEdit()
    {
      return CachedUrlHelper.Action("ItemsEdit", "Item");
    }
    public string ItemItemsDelete(int? id)
    {
      RouteValueDictionary route = new RouteValueDictionary();
      if (id != null)
        route.Add("id", id);
      return CachedUrlHelper.Action("ItemsDelete", "Item", route);
    }
    #endregion

  }
}
