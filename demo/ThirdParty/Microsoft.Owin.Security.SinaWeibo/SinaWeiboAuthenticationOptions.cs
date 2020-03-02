
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.Owin.Security.SinaWeibo
{

    /// <summary>
    /// 
    /// </summary>
    public class SinaWeiboAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public SinaWeiboAuthenticationOptions() : base(Constants.DefaultAuthenticationType)
        {
            Caption = Constants.DefaultAuthenticationType;
            this.CallbackPath = new PathString("/signin-sinaWeibo");
            AuthenticationMode = AuthenticationMode.Passive;

            Scope = new List<string>();
            BackchannelTimeout = TimeSpan.FromSeconds(60);
        }
        public ICertificateValidator BackchannelCertificateValidator { get; set; }

        public string Caption
        {
            get
            {
                return base.Description.Caption;
            }
            set
            {
                base.Description.Caption = value;
            }
        }

        public TimeSpan BackchannelTimeout { get; set; }

        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        /// 请求用户授权时向用户显示的可进行授权的列表。
        /// </summary>
        public IList<string> Scope { get; private set; }

        public PathString CallbackPath { get; set; }

        public string SignInAsAuthenticationType { get; set; }
 
        public ISinaWeiboAuthenticationProvider Provider { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        /// <summary>
        /// 申请QQ登录成功后，分配给应用的appid。
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 申请QQ登录成功后，分配给网站的appkey。
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Make sure you got the advanced api authroity to enable this Property, more information please visit:
        /// http://open.weibo.com/wiki/%E9%AB%98%E7%BA%A7%E6%8E%A5%E5%8F%A3%E7%94%B3%E8%AF%B7
        /// </summary>
        public bool RequireEmail { get; set; }

        
 

        public string ReturnEndpointPath { get; set; }
         
     
    }
}
