using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItemDoc.Core.Auth;
using ItemDoc.Core.Mvc;
using ItemDoc.Core.Mvc.SystemMessage;
using ItemDoc.Framework.Validation;
using ItemDoc.Services.Mapping;
using ItemDoc.Services.Model;
using ItemDoc.Services.Servers;
using ItemDoc.Services.ViewModel;

namespace ItemDoc.Web.Controllers
{
  public class UserController : Controller
  {
    private readonly IAuthenticationService _authentication;
    private readonly IUsersService _usersService;
    private IPostService _postService;
    public UserController(
      IUsersService usersService,
      IAuthenticationService authentication,
      IPostService postService)
    {
      _authentication = authentication;
      _postService = postService;
      _usersService = usersService;
    }

    /// <summary>
    /// Indexes this instance.
    /// </summary>
    /// <returns></returns>
    public ActionResult demo()
    {
      //SQLiteConnection.CreateFile("E:\\itemdoc4.db");

      var info = _usersService.GetByUserId("2");
      // ReSharper disable once Mvc.ViewNotResolved
      return View();
    }

    public ActionResult Home(string userName)
    {
      ViewBag.userName = userName;
      return View();
    }

    public ActionResult _ProfileHeader(string userName)
    {
      ViewBag.userName = userName;
      return View();
    }


    public ActionResult MyHomepage(string userName)
    {

      return View();
    }

    public ActionResult Post(string userName)
    {
      var list = _postService.GetAll().MapToList<PostViewModel>().ToList();
      ViewData["postView"] = list;
      return View();
    }


    #region 注册登陆退出
    /// <summary>
    /// Registers this instance.
    /// </summary>
    /// <returns></returns>
    public ActionResult Register()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Register(UsersRegisterViewModel infoModel)
    {
      try
      {
        bool a1 = TryValidateModel(infoModel);
        if (ModelState.IsValid)
        {
          //判断验证码
          bool isok = Captcha.ValidateCheckCode(infoModel.UserLoginCaptchaCode);
          if (!isok)
          {
            ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "验证码输入错误");
            return View();
          }


          UsersLoginInfo info = infoModel.AsUserLoginInfo();
          _usersService.Insert(info);
          var loginInfo = _usersService.GetByUserId(info.UserId);
          if (loginInfo != null)
          {
            // 注册成功自动登陆
            _authentication.UserSignIn(loginInfo.UserId, loginInfo.UserName, true);
            //
            return RedirectToAction("Index", "Home", new { msg = "注册成功", isok = true.ToString() });
          }
          else
          {
            ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "注册失败");
            return View();
          }

        }
        else
        {
          ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "注册失败");
          return View();
        }
      }
      catch (Exception ex)
      {
        ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, ex.Message);
        return View();
      }

    }


    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Login(UsersLoginViewModel loginModel)
    {
      try
      {
        bool a1 = TryValidateModel(loginModel);
        if (ModelState.IsValid)
        {
          //判断验证码
          bool isok = Captcha.ValidateCheckCode(loginModel.UserLoginCaptchaCode);
          if (!isok)
          {
            ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "验证码输入错误");
            return View();
          }
          bool loginStatus;
          var info = _usersService.Login(loginModel.UserLoginName, loginModel.UserLoginPassWord, out loginStatus);
          if (loginStatus)
          {
            _authentication.UserSignIn(info.UserId, info.UserName, loginModel.RememberPassword);
          }
          else
          {
            ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "密码输入错误");
            return View();
          }
        }
        else
        {
          ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "登陆失败");
          return View();
        }
      }
      catch (Exception ex)
      {
        ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, ex.Message);
        return View(loginModel);
      }
      return RedirectToAction("Index", "Home", new { msg = "登录成功", isok = true.ToString() });

    }





    /// <summary>
    /// 验证登录名是否存在
    /// </summary>
    /// <param name="UserLoginName"></param>
    /// <returns></returns>
    public JsonResult ValidateLoginNameIsExsit(string UserLoginName)
    {
      if (string.IsNullOrEmpty(UserLoginName))
        return Json(true, JsonRequestBehavior.AllowGet);
      //存在返回
      var isExsit = _usersService.IsAccountExsit(UserLoginName);
      return Json(isExsit, JsonRequestBehavior.AllowGet);
    }
    /// <summary>
    /// 验证登录名
    /// </summary>
    /// <param name="UserLoginName"></param>
    /// <returns></returns>
    public JsonResult ValidateLoginName(string UserLoginName)
    {
      if (string.IsNullOrEmpty(UserLoginName))
        return Json(true, JsonRequestBehavior.AllowGet);
      var isExsit = _usersService.IsAccountExsit(UserLoginName);
      return Json(!isExsit, JsonRequestBehavior.AllowGet);
    }
    /// <summary>
    /// 验证码验证
    /// </summary>
    /// <param name="userLoginCaptchaCode"></param>
    /// <returns></returns>
    public JsonResult ValidateCaptchaCode(string userLoginCaptchaCode)
    {
      if (string.IsNullOrEmpty(userLoginCaptchaCode))
        return Json(userLoginCaptchaCode == null, JsonRequestBehavior.AllowGet);
      bool isok = Captcha.ValidateCheckCode(userLoginCaptchaCode);
      return Json(isok, JsonRequestBehavior.AllowGet);
    }


    /// <summary>
    /// Captchas the code.
    /// </summary>
    /// <returns></returns>
    public ActionResult CaptchaCode()
    {
      var stream = Captcha.SetStreamValidate();


      //string newFile = "F:\\444.gif";
      //Bitmap bt1 = new Bitmap(stream);
      //bt1.Save(newFile, ImageFormat.Gif);
      //  return new FileStreamResult(stream, "image/jpeg");
      return File(stream, "image/gif");


    }

    public ActionResult SignOut()
    {
      // AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = rememberme }, claimsIdentity);
      var authenticationManager = HttpContext.GetOwinContext().Authentication;
      authenticationManager.SignOut();
      return RedirectToAction("Index", "Home");
    }
    #endregion

    [Authorize]
    public ActionResult Setting()
    {
      // ReSharper disable once Mvc.ViewNotResolved
      return View();
    }



  }



}
