using AutoMapper;
using Sop.Core.Mvc;
using Sop.Core.Mvc.SystemMessage;
using Sop.Services.Model;
using Sop.Services.Servers;
using Sop.Services.ViewModel;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using Sop.Core.Validation;

namespace Sop.Web.Controllers
{
    public class UserController : BaseController
    {
        public AuthenticationService Authentication { get; set; }
        public UsersService UsersService { get; set; }
        public PostService PostService { get; set; }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult demo()
        {
            //SQLiteConnection.CreateFile("E:\\itemdoc4.db");

            var info = UsersService.GetByUserId("2");
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
            var list = PostService.GetAll().MapToList<PostViewModel>().ToList();
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
                    bool isok = Captcha.ValidateCheckCode(infoModel.CaptchaCode);
                    if (!isok)
                    {
                        ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "验证码输入错误");

                        return View();
                    }


                    UsersInfo info = infoModel.MapTo<UsersInfo>();
                    UsersService.Insert(info);
                    var loginInfo = UsersService.GetByUserId(info.UserId);
                    if (loginInfo != null)
                    {
                        // 注册成功自动登陆
                        //Authentication.Login(loginInfo.Id, loginInfo.UserName, true);
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
                    bool isok = Captcha.ValidateCheckCode(loginModel.CaptchaCode);
                    if (!isok)
                    {
                        ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "验证码输入错误");
                        return View();
                    }
                    bool loginStatus;
                    var info = UsersService.Login(loginModel.UserName, loginModel.PassWord, out loginStatus);
                    if (loginStatus)
                    {
                        //Authentication.UserSignIn(info.Id, info.UserName, loginModel.RememberPassword);
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
            var isExsit = UsersService.IsAccountExsit(UserLoginName);
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
            var isExsit = UsersService.IsAccountExsit(UserLoginName);
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
