
using System;
using Owin;

namespace Microsoft.Owin.Security.SinaWeibo
{
    public static class SinaWeiboAccountAuthenticationExtensions
    {
        /// <summary>
        /// the redirect url is "yourdomain/signin-sinaWeibo"
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseSinaWeiboAuthentication(this IAppBuilder app, SinaWeiboAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(SinaWeiboAuthenticationMiddleware), app, options);
        }
        /// <summary>
        /// the redirect url is "yourdomain/signin-sinaWeibo"
        /// </summary>
        /// <param name="app"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public static void UseSinaWeiboAuthentication(this IAppBuilder app, string appId, string appSecret)
        {
            UseSinaWeiboAuthentication(app, new SinaWeiboAuthenticationOptions()
            {
                AppId = appId,
                AppSecret = appSecret,
                SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType()
            });
        }
    }
}
