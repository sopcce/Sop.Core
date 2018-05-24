using ItemDoc.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
[assembly: OwinStartup(typeof(OwinStartup))]

namespace ItemDoc.Web
{
  public class OwinStartup
  {
    public void Configuration(IAppBuilder app)
    {
      // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
      // New code:
      // app.Run(context =>
      //  {
      //    context.Response.ContentType = "text/plain";
      //     return context.Response.WriteAsync("Hello, world.");
      // });
      System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier =
        System.Security.Claims.ClaimTypes.NameIdentifier;


      var userInfo = new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        LoginPath = new PathString("/User/Login"),
        CookieSecure = CookieSecureOption.SameAsRequest,
        ExpireTimeSpan = TimeSpan.FromDays(1),
        SlidingExpiration = true,//当用户保持访问网站的时候再过特定时间（不访问）则失效
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


    }
  }
}