using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Sop.Framework.WebUtility
{
    /// <summary>
    /// 链接工具类
    /// 
    /// </summary>
    public class UrlUtility
    {
        #region Instance

        private static volatile UrlUtility _instance = null;
        private static readonly object Lock = new object();
        /// <summary>
        /// CookieUtility
        /// </summary>
        /// <returns></returns>
        public static UrlUtility Instance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UrlUtility();
                    }
                }
            }
            return _instance;
        }

        #endregion Instance

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(int description, int reservedValue);

        /// <summary>
        /// 用于检查网络是否可以连接互联网
        /// </summary>
        /// <returns>true表示连接成功,false表示连接失败 </returns>
        public bool GetConnectInternetStatus()
        {
            try
            {
                int Description = 0;
                bool connectInternet = InternetGetConnectedState(Description, 0);
                if (connectInternet)
                {
                    connectInternet = GetPingIpOrDomainName("www.baidu.com");
                }
                return connectInternet;
            }
            catch (Exception e)
            {

                return false;
            }
            return false;

        }
        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns>true表示Ping成功,false表示Ping失败</returns>
        public bool GetPingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                strIpOrDName = strIpOrDName.Replace("http://", "").Replace("http://", "");
                if (strIpOrDName.Contains(":"))
                {

                    strIpOrDName = strIpOrDName.Substring(0, strIpOrDName.LastIndexOf(":", StringComparison.Ordinal));
                }

                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                if (objPinReply != null)
                {
                    string strInfo = objPinReply.Status.ToString();
                    if (strInfo.ToLower() == "success")
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取根域名
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="domainRules">域名规则</param>
        public string GetServerDomain(Uri uri, string[] domainRules)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string text = uri.Host.ToString().ToLower();
            if (text.IndexOf('.') <= 0)
            {
                return text;
            }
            string[] array = text.Split(new char[]
            {
                '.'
            });
            string s = array.GetValue(array.Length - 1).ToString();
            int num = -1;
            if (int.TryParse(s, out num))
            {
                return text;
            }
            string text3 = string.Empty;
            string result = string.Empty;
            int i = 0;
            while (i < domainRules.Length)
            {
                if (text.EndsWith(domainRules[i].ToLower()))
                {
                    var text2 = domainRules[i].ToLower();
                    text3 = text.Replace(text2, "");
                    if (text3.IndexOf('.') > 0)
                    {
                        string[] array2 = text3.Split(new char[]
                        {
                            '.'
                        });
                        return array2.GetValue(array2.Length - 1).ToString() + text2;
                    }
                    return text3 + text2;
                }
                else
                {
                    result = text;
                    i++;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取指定Url路径的上一级完整路径(当前Url的父路径)
        /// </summary>
        /// <param name="UrlPath">Url路径(不能包括文件名部分，如果含文件名则返回去除文件名后的当前路径)</param>
        /// <param name="lockInBaseUrl">指定基Url安全限制，必须在此路径内 (如果为空表示不限制)</param>
        /// <returns>上一级完整Url路径</returns>
        public string GetParentUrlPath(string UrlPath, string lockInBaseUrl)
        {
            string text = UrlPath.TrimEnd(new char[]
            {
                '/',
                ' '
            });
            text = text.Substring(0, text.LastIndexOf("/") + 1);
            if (!string.IsNullOrEmpty(lockInBaseUrl) && !text.StartsWith(lockInBaseUrl, StringComparison.CurrentCultureIgnoreCase))
            {
                text = UrlPath;
            }
            return text;
        }
        /// <summary>
        /// 获取当前请求的域名，如：sopcce.com
        /// </summary>
        /// <param name="mustRaw">是否必须要完整的原始请求域名(如含www或端口号)</param>
        public string GetCurrentDomain(bool mustRaw)
        {
            HttpRequest request = HttpContext.Current.Request;
            string text = request.ServerVariables["HTTP_HOST"] ?? string.Empty;
            if (mustRaw)
            {
                return text;
            }
            if (text.StartsWith("www.", true, null))
            {
                text = text.Substring(4);
            }
            int num = text.IndexOf(':');
            if (num > 0)
            {
                text = text.Substring(0, num);
            }
            return text;
        }
        /// <summary>
        /// 获取指定完整域名的基域名，如：520.sopcce.com对应sopcce.com
        /// </summary>
        /// <param name="fullDomain">完整域名(仅域名部分)</param>
        public string GetBaseDomain(string fullDomain)
        {
            if (string.IsNullOrEmpty(fullDomain))
            {
                return string.Empty;
            }
            if (!fullDomain.Contains("."))
            {
                return fullDomain;
            }
            string[] array = fullDomain.Split(new char[]
            {
                '.'
            });
            int num = array.Length;
            if (num < 3)
            {
                return fullDomain;
            }
            return string.Format("{0}.{1}", array[num - 2], array[num - 1]);
        }
        /// <summary>
        /// 在指定完整URL中截取域名部分，如：http://www.sopcce.com/xiaoqiu/对应www.sopcce.com (返回小写)
        /// </summary>
        /// <param name="fullUrl">完整URL，可含路径及参数</param>
        public string GetDomainFromUrl(string fullUrl)
        {
            if (string.IsNullOrEmpty(fullUrl))
            {
                return string.Empty;
            }
            fullUrl = fullUrl.ToLower().Replace("http://", string.Empty).Replace("https://", string.Empty);
            if (!fullUrl.Contains("/"))
            {
                return fullUrl;
            }
            return fullUrl.Split(new char[]
            {
                '/'
            })[0];
        }
        /// <summary>
        /// 在指定完整URL中截取起始URL部分，如：http://www.sopcce.com/test/对应http://www.sopcce.com/
        /// </summary>
        /// <param name="fullUrl">完整URL，可含路径及参数</param>
        /// <returns></returns>
        public string GetBaseUrl(string fullUrl)
        {
            if (string.IsNullOrEmpty(fullUrl))
                return string.Empty;
            if (fullUrl.Length <= 10)
                return fullUrl;
            int num = fullUrl.IndexOf("/", 8, StringComparison.Ordinal);
            if (num < 0)
            {
                return $"{fullUrl}/";
            }
            return fullUrl.Substring(0, num + 1);
        }
        /// <summary>
        /// 获取带传输协议的完整的主机地址
        /// </summary> 
        /// <param name="uri">Uri</param>
        /// <returns>
        /// <para>返回带传输协议的完整的主机地址</para>
        ///     <example>https://www.sopcce.com:80</example>
        /// </returns>
        public string GetHostPath(Uri uri)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string str = uri.IsDefaultPort ? string.Empty : (":" + System.Convert.ToString(uri.Port, System.Globalization.CultureInfo.InvariantCulture));
            return uri.Scheme + Uri.SchemeDelimiter + uri.Host + str;
        }
        /// <summary>
        /// 将URL转换为在请求客户端可用的 URL（转换 ~/ 为绝对路径）
        /// </summary>
        /// <param name="relativeUrl">相对url</param>
        /// <returns>返回绝对路径</returns>
        public string SetResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;
            try
            {
                if (relativeUrl.StartsWith("~/"))
                {
                    string[] array = relativeUrl.Split(new char[] { '?' });
                    string text = VirtualPathUtility.ToAbsolute(array[0]);
                    if (array.Length > 1)
                    {
                        text = text + "?" + array[1];
                    }
                    return text;
                }
                return relativeUrl;
            }
            catch
            {
            }
            return relativeUrl;
        }
        /// <summary>
        /// 把content中的虚拟路径转化成完整的url、
        /// <example>
        /// 例如： /abc/e.aspx 转化成 http://www.sopcce.com/abc/e.aspx
        /// </example>
        /// </summary>
        /// <param name="content">content</param>
        public string SetCompleteUrl(string content)
        {
            string pattern = "src=[\"']\\s*(/[^\"']*)\\s*[\"']";
            string pattern2 = "href=[\"']\\s*(/[^\"']*)\\s*[\"']";
            string str = GetHostPath(HttpContext.Current.Request.Url);
            content = Regex.Replace(content, pattern, "src=\"" + str + "$1\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            content = Regex.Replace(content, pattern2, "href=\"" + str + "$1\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return content;
        }





        /// <summary>
        /// html编码
        /// </summary>
        /// <remarks>
        /// <para>调用HttpUtility.HtmlEncode()，当前已知仅作如下转换：</para>
        /// <list type="bullet">
        ///     <item>&lt; = &amp;lt;</item>
        ///     <item>&gt; = &amp;gt;</item>
        ///     <item>&amp; = &amp;amp;</item>
        ///     <item>" = &amp;quot;</item>
        /// </list>
        /// </remarks>
        /// <param name="rawContent">待编码的字符串</param>
        public string HtmlEncode(string rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
            {
                return rawContent;
            }
            return System.Web.HttpUtility.HtmlEncode(rawContent);
        }
        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="rawContent">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public string HtmlDecode(string rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
            {
                return rawContent;
            }
            return System.Web.HttpUtility.HtmlDecode(rawContent);
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="urlToEncode">待编码的url字符串</param>
        /// <returns>编码后的url字符串</returns>
        public string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
            {
                return urlToEncode;
            }
            return System.Net.WebUtility.UrlEncode(urlToEncode);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="urlToDecode">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
            {
                return urlToDecode;
            }
            return System.Net.WebUtility.UrlDecode(urlToDecode);
        }
    }
}