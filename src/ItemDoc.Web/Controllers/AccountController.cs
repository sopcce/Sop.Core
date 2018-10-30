using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ItemDoc.Core.Mvc;
using ItemDoc.Core.Mvc.SystemMessage;
using ItemDoc.Framework.Environment;
using ItemDoc.Framework.Utility;
using ItemDoc.Framework.Validation;
using ItemDoc.Services.Auth.Identity;
using ItemDoc.Services.Auth.Model;
using ItemDoc.Services.Model;
using ItemDoc.Services.ViewModel;
using ItemDoc.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace ItemDoc.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private SignInService _signInManager;
        private UserManager _userManager;
        public AccountController()
        {
        }
        public AccountController(UserManager userManager, SignInService signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }



        public SignInService SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<SignInService>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        #region Login

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UsersLoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //判断验证码
            bool isok = Captcha.ValidateCheckCode(model.CaptchaCode);
            if (!isok)
            {
                ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "验证码输入错误");
                return View(model);
            }
            //若要启用密码失败触发帐户锁定，则更改为“应锁闭锁：真”
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.PassWord, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        #endregion

        #region Register

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UsersRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                //判断验证码
                bool isok = Captcha.ValidateCheckCode(model.CaptchaCode);
                if (!isok)
                {
                    ViewData["msg"] = new SystemMessageData(SystemMessageType.Error, "验证码输入错误");
                    return View();
                }


                var user = new User
                {
                    Id = IdSnowflake.Instance().GetId(),
                    UserId = Guid.NewGuid().ToString(),
                    UserName = model.UserName,

                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,

                    NickName = model.NickName,
                    TrueName = model.NickName,
                    PassWord = model.Password,
                    PassWordEncryption = (PassWordEncryptionType)new Random().Next(1, 5),
                    SecurityStamp = Guid.NewGuid().ToString(),

                    Status = Status.NoActivated,

                    CreatedIP = WebUtility.GetIp(),
                    ActivityTime = DateTime.Now,
                    ActivityIP = WebUtility.GetIp(),
                    LastActivityIP = WebUtility.GetIp(),
                    DateCreated = DateTime.Now,
                    LastActivityTime = DateTime.Now,
                    LockoutEndDateUtc = DateTime.UtcNow.AddMinutes(-1),
                    LockoutEnabled = true,
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await UserManager.AddToRoleAsync(user.Id, "user");
                    await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Country, "England"));
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            return View(model);
        }
        #endregion 

        #region ExternalLogin
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return View(nameof(ExternalLoginFailure));
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View(nameof(ExternalLoginConfirmation), new ExternalLoginConfirmationViewModel
                    {
                        UserName = loginInfo.Email,
                        IsLogin = true,
                        NickName = loginInfo.DefaultUserName,

                    });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {


                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View(nameof(ExternalLoginFailure));
                }

                //判断验证码
                bool isok = Captcha.ValidateCheckCode(model.CaptchaCode);
                if (!isok)
                {
                    ModelState.AddModelError("", "验证码输入错误");
                    return View(model);
                }
                #region change

                if (model.IsLogin)
                {
                    #region Login 
                    var result = await SignInManager.PasswordSignInAsync(model.UserName, model.PassWord, true, shouldLockout: true);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            var user = await UserManager.FindByNameAsync(model.UserName);
                            var result1 = await UserManager.AddLoginAsync(user.Id, info.Login);
                            if (result1.Succeeded)
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                AddErrors(result1);
                                return View(model);
                            }
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = true });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "关联失败");
                            return View(model);
                    }

                    #endregion
                }
                else
                {
                    var user = new User
                    {
                        Id = IdSnowflake.Instance().GetId(),
                        UserId = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false,
                        NickName = model.NickName,
                        TrueName = model.NickName,
                        PassWord = model.PassWord,
                        PassWordEncryption = (PassWordEncryptionType)new Random().Next(1, 5),
                        SecurityStamp = Guid.NewGuid().ToString(),
                        Status = Status.NoActivated,
                        CreatedIP = WebUtility.GetIp(),
                        ActivityTime = DateTime.Now,
                        ActivityIP = WebUtility.GetIp(),
                        LastActivityIP = WebUtility.GetIp(),
                        DateCreated = DateTime.Now,
                        LastActivityTime = DateTime.Now,
                        LockoutEndDateUtc = DateTime.UtcNow.AddMinutes(-1),
                        LockoutEnabled = true,
                    };
                    var result = await UserManager.CreateAsync(user, user.PassWord);

                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: true);
                            return RedirectToLocal(returnUrl);
                        }
                    }

                    AddErrors(result);
                }


                #endregion
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #endregion




        public ActionResult Settings()
        {
            return View();
        }
        public ActionResult _SettingsHeader()
        {
           
            return View();
        }

        #region ForgotPassword
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion









        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }












        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(long userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }







        #region  ResetPassword
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return  View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //获取当前登录用户，如果获取不到
            string code1 = await UserManager.GeneratePasswordResetTokenAsync(23123);

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion


        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId.ToString() == "")
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut();

            //var authenticationManager = HttpContext.GetOwinContext().Authentication;
            //authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");


        }

    



        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}