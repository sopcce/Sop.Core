﻿using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.QQ
{
    /// <summary>
    /// 
    /// </summary>
    public class QQHandler : OAuthHandler<QQOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public QQHandler(IOptionsMonitor<QQOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="properties"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            AuthenticationProperties properties,
            OAuthTokenResponse tokens)
        {
            //获取OpenId
            var openIdEndpoint =
                QueryHelpers.AddQueryString(Options.OpenIdEndpoint, "access_token", tokens.AccessToken);
            var openIdResponse = await Backchannel.GetAsync(openIdEndpoint, Context.RequestAborted);
            if (!openIdResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"未能检索QQ Connect的OpenId(返回状态码:{openIdResponse.StatusCode})，请检查access_token是正确。");
            }

            var json = JObject.Parse(ConvertToJson(await openIdResponse.Content.ReadAsStringAsync()));
            var openId = GetOpenId(json);

            //获取用户信息
            var parameters = new Dictionary<string, string>
            {
                {"openid", openId},
                {"access_token", tokens.AccessToken},
                {"oauth_consumer_key", Options.ClientId}
            };
            var userInformationEndpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);
            var userInformationResponse = await Backchannel.GetAsync(userInformationEndpoint, Context.RequestAborted);
            if (!userInformationResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"未能检索QQ Connect的用户信息(返回状态码:{userInformationResponse.StatusCode})，请检查参数是否正确。");
            }

            var userInfo = await userInformationResponse.Content.ReadAsStringAsync();
            var payload = JObject.Parse(userInfo);
            payload.Add("openid", openId);
            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, openId, ClaimValueTypes.String, Options.ClaimsIssuer));


            // // var payload = JsonElement.ParseValue();
            // var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme,
            //     Options, Backchannel, tokens, payload);
            //
            //
            // context.RunClaimActions();
            // await Events.CreatingTicket(context);
            // return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            return new AuthenticationTicket(null, null, Scheme.Name);
        }

        /// <summary>
        /// 通过Authorization Code获取Access Token。
        /// 重写改方法，QQ这一步用的是Get请求，base中用的是Post
        /// </summary>
        protected async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var parameters = new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"client_id", Options.ClientId},
                {"client_secret", Options.ClientSecret},
                {"code", code},
                {"redirect_uri", redirectUri}
            };

            var endpoint = QueryHelpers.AddQueryString(Options.TokenEndpoint, parameters);

            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
            if (response.IsSuccessStatusCode)
            {
                var json = ConvertToJson(await response.Content.ReadAsStringAsync());
                // var payload = JObject.Parse(ConvertToJson(await response.Content.ReadAsStringAsync()));
                var payload = JsonDocument.Parse(json);
                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                return OAuthTokenResponse.Failed(new Exception("获取QQ Connect Access Token出错。"));
            }
        }

        /// <summary>
        /// 尝试装换JSON
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string ConvertToJson(string text)
        {
            /*
             * 为什么有个 callback() 包裹; 
             * 个人猜测这是一个用于跨域调用的JSONP的回调函数，名称并不是固定的，只是默认为callback,用jquery.ajax请求时就可以设置这个回调函数名
             * 当然，这也只是猜测，QQ互联文档上也没有明确说明
             * 但是，可以肯定的是，我们是在服务端发出的请求，没有主动设置，返回的一定是 callback(json) 格式。
            */

            //通过Access Token，成功取得对应用户身份OpenID时的数据返回格式
            //详情请访问QQ互联api文档 htt p://wiki.connect.qq.com/%E8%8E%B7%E5%8F%96%E7%94%A8%E6%88%B7openid_oauth2-0
            var openIdRegex = new Regex("callback\\((?<json>[ -~]+)\\);");
            if (openIdRegex.IsMatch(text))
            {
                return openIdRegex.Match(text).Groups["json"].Value;
            }

            //获取Access Token成功后的返回数据格式,详情请参见QQ互联api文档“Step2：通过Authorization Code获取Access Token ”章节
            //htt p://wiki.connect.qq.com/%E4%BD%BF%E7%94%A8authorization_code%E8%8E%B7%E5%8F%96access_token
            var tokenRegex = new Regex("^access_token=.{1,}&expires_in=.{1,}&refresh_token=.{1,}");
            if (tokenRegex.IsMatch(text))
            {
                return "{\"" + text.Replace("=", "\":\"").Replace("&", "\",\"") + "\"}";
            }

            return "{}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string GetOpenId(JObject json)
        {
            return json.Value<string>("openid");
        }
    }
}