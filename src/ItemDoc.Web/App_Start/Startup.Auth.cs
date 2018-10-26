using System;
using ItemDoc.Services.Auth.Identity;
using ItemDoc.Services.Auth.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Github;
using Microsoft.Owin.Security.QQ;
using Microsoft.Owin.Security.Weixin;
using Owin;


namespace ItemDoc.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var folderStorage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Storage");

            // 配置数据库上下文、用户管理器和登录管理器，以便为每个请求使用单个实例
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<UserManager>(() => new UserManager(new UserStore(folderStorage)));
            app.CreatePerOwinContext<RoleManager>(() => new RoleManager(new RoleStore(folderStorage)));
            app.CreatePerOwinContext<SignInService>((options, context) => new SignInService(context.GetUserManager<UserManager>(), context.Authentication));



            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier =
              System.Security.Claims.ClaimTypes.NameIdentifier;


            // 使应用程序可以使用 Cookie 来存储已登录用户的信息
            // 并使用 Cookie 来临时存储有关使用第三方登录提供程序登录的用户的信息
            // 配置登录 Cookie
            var userInfo = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieSecure = CookieSecureOption.SameAsRequest,
                ExpireTimeSpan = TimeSpan.FromDays(1),
                SlidingExpiration = true,//当用户保持访问网站的时候再过特定时间（不访问）则失效
                Provider = new CookieAuthenticationProvider
                {
                    // 当用户登录时使应用程序可以验证安全戳。
                    // 这是一项安全功能，当你更改密码或者向帐户添加外部登录名时，将使用此功能。
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager, User, long>(
                  validateInterval: TimeSpan.FromMinutes(30),
                  regenerateIdentityCallback: (manager, user) =>
                  {
                      var userIdentity = manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                      return (userIdentity);
                  },
                  getUserIdCallback: (id) =>
                  {
                      long.TryParse(id.GetUserId(), out var uId);
                      return uId;
                  })
                }
            };
            app.UseCookieAuthentication(userInfo);

            //var OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //  AllowInsecureHttp = true,
            //  AuthenticationMode = AuthenticationMode.Active,
            //  TokenEndpointPath = new PathString("/token"), //获取 access_token 授权服务请求地址
            //  AuthorizeEndpointPath = new PathString("/authorize"), //获取 authorization_code 授权服务请求地址
            //  AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(10), //access_token 过期时间

            //  Provider = new OpenAuthorizationServerProvider(), //access_token 相关授权服务
            //  AuthorizationCodeProvider = new OpenAuthorizationCodeProvider(), //authorization_code 授权服务
            //  RefreshTokenProvider = new OpenRefreshTokenProvider() //refresh_token 授权服务
            //};
            //app.UseOAuthBearerTokens(OAuthOptions); //表示 token_type 使用 bearer 方式

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 使应用程序可以在双重身份验证过程中验证第二因素时暂时存储用户信息。
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // 使应用程序可以记住第二登录验证因素，例如电话或电子邮件。
            // 选中此选项后，登录过程中执行的第二个验证步骤将保存到你登录时所在的设备上。
            // 此选项类似于在登录时提供的“记住我”选项。
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // 取消注释以下行可允许使用第三方登录提供程序登录
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //  ClientId = "",
            //  ClientSecret = ""
            //});

            app.UseGitHubAuthentication("44165d1ebe608934fcdc", "40d96fcf249cd627b56859d93433402d9947cc79");

            app.UseWeixinAuthentication("asd", "dsaasd");

            app.UseQQAuthentication("101264620", "95850abee8a3ffbfddb6027fce3e1933");














        }
    }
}