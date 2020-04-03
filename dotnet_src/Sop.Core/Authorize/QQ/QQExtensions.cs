using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Authentication.QQ
{
    public static class QQExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder)
            => builder.AddQQ(QQDefaults.AuthenticationScheme, _ => { });
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder, Action<QQOptions> configureOptions)
            => builder.AddQQ(QQDefaults.AuthenticationScheme, configureOptions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder, string authenticationScheme, Action<QQOptions> configureOptions)
            => builder.AddQQ(authenticationScheme, QQDefaults.DisplayName, configureOptions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="displayName"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<QQOptions> configureOptions)
            => builder.AddOAuth<QQOptions, QQHandler>(authenticationScheme, displayName, configureOptions);
    }
}
