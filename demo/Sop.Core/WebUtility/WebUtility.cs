using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Sop.Framework.Utility;

namespace Sop.Core.WebUtility
{
    /// <summary>
    /// 网页全局通用工具类。
    /// 1、提供与Web请求时可使用的工具类，
    /// 2、包括Url解析、Url/Html编码、获取IP地址
    /// </summary>
    public static class WebUtility
    {
        /// <summary>
        /// HTTP网络抓取或提交时默认的UserAgent标识串
        /// </summary>
        private static readonly string HttpUserAgent = "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0; Alexa Toolbar) like Gecko";


        #region IP相关
        /// <summary>
        /// 获取用户真实的IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static string GetIp()
        {
            return GetIp(HttpContext.Current);
        }
        /// <summary>
        /// 透过代理获取真实IP
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>返回获取的ip地址</returns>
        public static string GetIp(HttpContext httpContext)
        {
            string text = string.Empty;
            if (httpContext == null)
            {
                return text;
            }
            text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(text))
            {
                text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(text))
            {
                text = HttpContext.Current.Request.UserHostAddress;
            }
            if (null == text || text == String.Empty || !IsIp(text))
            {
                return "0.0.0.0";
            }
            return text;
        }
        /// <summary>
        /// 获取远程客户机的IP地址
        /// </summary>
        /// <param name="clientSocket">客户端的socket对象</param>        
        /// <returns></returns>
        public static string GetIPClient(Socket clientSocket)
        {
            IPEndPoint client = (IPEndPoint)clientSocket.RemoteEndPoint;
            return client.Address.ToString();
        }
        /// <summary>
        /// 获取本地机器IP地址
        /// </summary>
        /// <returns></returns>
        public static IPHostEntry GetIpLocal()
        {
            string strHostIP = string.Empty;
            IPHostEntry oIPHost = Dns.GetHostEntry(System.Environment.MachineName);

            return oIPHost;
        }
        /// <summary>
        /// 判断是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(string ip)
        {
            bool isok = System.Text.RegularExpressions.Regex.IsMatch(ip,
                @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
            return isok;
        }
        /// <summary> 
        /// 把IP地址转换为数字格式 
        /// </summary> 
        /// <param name="strIp">IP地址</param> 
        /// <returns>数字</returns> 
        public static int IptoNum(string strIp)
        {
            string[] temp = strIp.Split('.');
            return (int.Parse(temp[0])) * 256 * 256 * 256 + (int.Parse(temp[1])) * 256 * 256 * 256 + (int.Parse(temp[2])) * 256 * 256 * 256;
        }
        /// <summary>
        /// 将字符串形式的IP地址转换成IPAddress对象
        /// </summary>
        /// <param name="ip">字符串形式的IP地址</param>        
        public static IPAddress StringToIpAddress(string ip)
        {
            return IPAddress.Parse(ip);
        }
        #endregion

        

        #region Http Get Post
        /// <summary>
        /// 同步方式发起Http Get请求(数据内容需自行UrlEncode编码)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="QueryString">参数字符串(不用带“?”)</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <param name="userAgent">浏览器类型标识字符串，可为空(默认IE标识)</param>
        /// <param name="referer">来路网址字符串，可为空</param>
        /// <param name="encode">采用的编码方式</param>
        /// <returns>请求返回值</returns>
        public static string HttpGet(string url, string QueryString, int timeout, string userAgent, string referer, Encoding encode)
        {
            if (!string.IsNullOrEmpty(QueryString))
            {
                url = url + "?" + QueryString.Trim(new char[]
                {
                    ' ',
                    '?',
                    '&'
                });
            }
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            if (string.IsNullOrEmpty(userAgent))
            {
                userAgent = "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0; Alexa Toolbar) like Gecko";
            }
            if (httpWebRequest != null)
            {
                httpWebRequest.UserAgent = userAgent;
                if (!string.IsNullOrEmpty(referer))
                {
                    httpWebRequest.Referer = referer;
                }
                httpWebRequest.Method = "GET";
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Timeout = timeout * 1000;
                return ResponseRead(httpWebRequest, encode);
            }
            return null;
        }
        /// <summary>
        /// 同步方式发起Http Get请求(数据内容需自行UrlEncode编码)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="QueryString">参数字符串(不用带“?”)</param>
        /// <param name="Timeout">超时时间(秒)</param>
        /// <param name="userAgent">浏览器类型标识字符串，可为空(默认IE标识)</param>
        /// <param name="referer">来路网址字符串，可为空</param>
        /// <returns>请求返回值</returns>
        public static string HttpGet(string url, string QueryString, int Timeout, string userAgent, string referer)
        {
            return HttpGet(url, QueryString, Timeout, userAgent, referer, Encoding.UTF8);
        }
        /// <summary>
        /// 同步方式发起Http Get请求(数据内容需自行UrlEncode编码)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">参数字符串(不用带“?”)</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <returns>请求返回值</returns>
        public static string HttpGet(string url, string queryString, int timeout)
        {
            return HttpGet(url, queryString, timeout, string.Empty, string.Empty, Encoding.UTF8);
        }
        /// <summary>
        /// 同步方式发起Http Post请求(数据内容需自行UrlEncode编码)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">参数字符串(不用带“?”)</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <param name="userAgent">浏览器类型标识字符串，可为空(默认IE标识)</param>
        /// <param name="referer">来路网址字符串，可为空</param>
        /// <param name="encode">采用的编码方式</param>
        /// <returns>请求返回值</returns>
        public static string HttpPost(string url, string queryString, int timeout, string userAgent, string referer, Encoding encode)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                queryString = queryString.Trim(new char[]
                {
                    ' ',
                    '?',
                    '&'
                });
            }
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            if (string.IsNullOrEmpty(userAgent))
            {
                userAgent = "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0; Alexa Toolbar) like Gecko";
            }
            httpWebRequest.UserAgent = userAgent;
            if (!string.IsNullOrEmpty(referer))
            {
                httpWebRequest.Referer = referer;
            }
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.Timeout = timeout * 1000;
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(queryString);
                streamWriter.Close();
            }
            return ResponseRead(httpWebRequest, encode);
        }
        /// <summary>
        /// 同步方式发起Http Post请求(数据内容需自行UrlEncode编码)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">参数字符串(不用带“?”)</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <param name="userAgent">浏览器类型标识字符串，可为空(默认IE标识)</param>
        /// <param name="referer">来路网址字符串，可为空</param>
        /// <returns>请求返回值</returns>
        public static string HttpPost(string url, string queryString, int timeout, string userAgent, string referer)
        {
            return HttpPost(url, queryString, timeout, userAgent, referer, Encoding.UTF8);
        }
        /// <summary>
        /// 同步方式发起Http Post请求(数据内容需自行UrlEncode编码)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">参数字符串(不用带“?”)</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <returns>请求返回值</returns>
        public static string HttpPost(string url, string queryString, int timeout)
        {
            return HttpPost(url, queryString, timeout, string.Empty, string.Empty, Encoding.UTF8);
        }
        #endregion


        

        #region Http
        /// <summary>
        /// 读取HttpWebRequest的返回结果
        /// </summary>
        /// <param name="webRequest">HttpWebRequest对象</param>
        /// <param name="encode">采用的编码方式</param>
        /// <returns>请求返回值</returns>
        public static string ResponseRead(HttpWebRequest webRequest, Encoding encode)
        {
            string result = null;
            try
            {
                using (var responseStream = webRequest.GetResponse().GetResponseStream())
                {
                    var stream = responseStream;
                    if (stream != null)
                    {
                        using (var streamReader = new StreamReader(stream, encode))
                        {
                            result = streamReader.ReadToEnd();
                            streamReader.Close();
                            responseStream?.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result = (ex.Message ?? string.Empty);
            }
            return result;
        }
        #endregion

        #region MyRegion




        /// <summary>
        /// 判断当前请求是否为Spider程序。
        /// </summary>
        /// <returns>是:True，否:False</returns>
        public static bool IsSpider(bool userAgentIsNullAsSpider)
        {
            string text = HttpContext.Current.Request.UserAgent;
            if (string.IsNullOrEmpty(text))
                text = string.Empty;
            else
                text = text.ToLower().Trim();
            return (userAgentIsNullAsSpider && string.IsNullOrEmpty(text)) || (text.Contains("spider") || text.Contains("googlebot") || text.Contains("yahoo! slurp") || text.Contains("msnbot") || text.Contains("yodaobot"));
        }
        /// <summary>
        /// 判断当前用户的访问来路是否来自搜索引擎。
        /// </summary>
        public static bool IsComeFromSearchEngine()
        {
            string text = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            text = (string.IsNullOrEmpty(text) ? string.Empty : text.ToLower().Trim());
            return (
                text.Contains(".baidu.com") ||
                text.Contains(".soso.com") ||
                text.Contains(".youdao.com") ||
                text.Contains(".yahoo.com") ||
                text.Contains(".sogou.com") ||
                text.Contains(".bing.com") ||
                text.Contains(".so.com") ||
                text.Contains(".google.com")) &&
                text.Contains("?") &&
                text.Contains("=");
        }



        #endregion




        /// <summary>
        /// 得到当前网站的根地址
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            // 是否为SSL认证站点
            string secure = HttpContext.Current.Request.ServerVariables["HTTPS"];
            string httpProtocol = (secure == "on" ? "https://" : "http://");
            // 服务器名称
            string serverName = HttpContext.Current.Request.ServerVariables["Server_Name"];
            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            // 应用服务名称
            string applicationName = HttpContext.Current.Request.ApplicationPath;
            return httpProtocol + serverName + (port.Length > 0 ? ":" + port : string.Empty) + applicationName;
        }











         


        /// <summary>
        /// 
        /// </summary>
        public static Random RandomNumber = new Random();
        #region 前端HTML相关处理类


        /// <summary>
        /// 进行HTML编码及空格、回车等转换支持
        /// </summary>
        /// <param name="SrcText">源文本</param>
        /// <param name="KeepHtmlTag">是否保留源文本中的HTML标签</param>
        /// <returns>编码后的HTML代码</returns>
        public static string HtmlEncode(string SrcText, bool KeepHtmlTag)
        {
            string text = KeepHtmlTag ? SrcText : System.Web.HttpUtility.HtmlEncode(SrcText);
            return text.Replace("  ", "&nbsp; ").Replace(System.Environment.NewLine, "<br />");
        }
        /// <summary>
        /// 清除指定文本的所有HTML格式，使之变为纯文本(不Trim空格)
        /// </summary>
        /// <param name="HtmlContent">HTML内容</param>
        /// <param name="BlockTagAsNewSection">DIV或P或H2等块标签视为新段落</param>
        /// <returns>纯文本数据</returns>
        /// <remarks>
        /// 后续可再调用StringParse.AutoTypesetting 进行自动排版
        /// </remarks>
        public static string ClearHtmlFormat(string HtmlContent, bool BlockTagAsNewSection)
        {
            if (string.IsNullOrEmpty(HtmlContent))
            {
                return string.Empty;
            }
            if (BlockTagAsNewSection)
            {
                string[] array = new string[]
                {
                    "<h1>",
                    "<h2>",
                    "<h3>",
                    "<h4>",
                    "<div>",
                    "<div ",
                    "<p>",
                    "<p ",
                    "<table>",
                    "<table "
                };
                string[] array2 = new string[]
                {
                    "</h1>",
                    "</h2>",
                    "</h3>",
                    "</h4>",
                    "</div>",
                    "</p>",
                    "</table>"
                };
                string text = System.Environment.NewLine + System.Environment.NewLine;
                string[] array3 = array;
                for (int i = 0; i < array3.Length; i++)
                {
                    string text2 = array3[i];
                    HtmlContent = HtmlContent.Replace(text2, text + text2);
                    string text3 = text2.ToUpper();
                    HtmlContent = HtmlContent.Replace(text3, text + text3);
                }
                string[] array4 = array2;
                for (int j = 0; j < array4.Length; j++)
                {
                    string text4 = array4[j];
                    HtmlContent = HtmlContent.Replace(text4, text4 + text);
                    string text5 = text4.ToUpper();
                    HtmlContent = HtmlContent.Replace(text5, text5 + text);
                }
            }
            HtmlContent = Regex.Replace(HtmlContent, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", System.Environment.NewLine);
            HtmlContent = Regex.Replace(HtmlContent, "<(.|\\n)*?>", string.Empty);
            HtmlContent = HtmlContent.Replace("&nbsp;", " ").Replace("&NBSP;", " ");
            return HtmlContent;
        }
        /// <summary>
        /// 清除指定文本的所有HTML格式，使之变为纯文本(不Trim空格)
        /// </summary>
        /// <param name="HtmlContent">HTML内容</param>
        /// <returns>纯文本数据</returns>
        public static string ClearHtmlFormat(string HtmlContent)
        {
            return ClearHtmlFormat(HtmlContent, false);
        }
        /// <summary>
        /// 判断指定字符串是否含有HTML转译字符。
        /// </summary>
        /// <param name="SrcValue">源字符串</param>
        /// <returns>true：含有，false：不含有</returns>
        public static bool IsContainsHtmlTranslationCharacters(string SrcValue)
        {
            if (string.IsNullOrEmpty(SrcValue))
            {
                return false;
            }
            string text = System.Web.HttpUtility.HtmlEncode(SrcValue);
            return !text.Equals(SrcValue);
        }
        /// <summary>
        /// 用户输入内容过滤，仅允许文本格式的HTML代码。如果输入含有超链接、Script、按钮等代码将会清除格式。
        /// </summary>
        /// <param name="SourceHtmlCode">HTML源数据(可以为null)</param>
        /// <returns>经过过滤的安全代码</returns>
        public static string OnlyTextCodeFilter(string SourceHtmlCode)
        {
            if (string.IsNullOrEmpty(SourceHtmlCode))
            {
                return string.Empty;
            }
            string text = SourceHtmlCode;
            if (Regex.IsMatch(text, "<a", RegexOptions.IgnoreCase | RegexOptions.Multiline) || Regex.IsMatch(text, "<script", RegexOptions.IgnoreCase | RegexOptions.Multiline) || Regex.IsMatch(text, "<input", RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                text = text.Replace("&nbsp;", "[nbsp;]").Replace("&NBSP;", "[nbsp;]");
                text = text.Replace("<br />", "[br/]").Replace("<BR />", "[br/]");
                text = text.Replace("<br/>", "[br/]").Replace("<BR/>", "[br/]");
                text = text.Replace("<p>", "[(p)]").Replace("<P>", "[(p)]");
                text = text.Replace("</p>", "[(/p)]").Replace("</P>", "[(/p)]");
                text = text.Replace("<u>", "[(u)]").Replace("<U>", "[(u)]");
                text = text.Replace("</u>", "[(/u)]").Replace("</U>", "[(/u)]");
                text = text.Replace("<b>", "[(b)]").Replace("<B>", "[(b)]");
                text = text.Replace("<strong>", "[(b)]").Replace("<STRONG>", "[(b)]");
                text = text.Replace("</b>", "[(/b)]").Replace("</B>", "[(/b)]");
                text = text.Replace("</strong>", "[(/b)]").Replace("</STRONG>", "[(/b)]");
                text = text.Replace("<i>", "[(i)]").Replace("<I>", "[(i)]");
                text = text.Replace("<em>", "[(i)]").Replace("<EM>", "[(i)]");
                text = text.Replace("</i>", "[(/i)]").Replace("</I>", "[(/i)]");
                text = text.Replace("</em>", "[(/i)]").Replace("</EM>", "[(/i)]");
                text = ClearHtmlFormat(text);
                text = text.Replace("[nbsp;]", "&nbsp;");
                text = text.Replace("[br/]", "<br />");
                text = text.Replace("[(p)]", "<p>").Replace("[(/p)]", "</p>");
                text = text.Replace("[(u)]", "<u>").Replace("[(/u)]", "</u>");
                text = text.Replace("[(b)]", "<b>").Replace("[(/b)]", "</b>");
                text = text.Replace("[(i)]", "<i>").Replace("[(/i)]", "</i>");
            }
            return text;
        }
        /// <summary>
        /// 把UBB代码转译成HTML代码。
        /// [color=(red);]颜色[/color]
        /// [b]粗体[/b]
        /// [u]下划线[/u]
        /// [i]斜体[/i]
        /// </summary>
        /// <param name="SourceText">源文本数据</param>
        /// <returns>转译后的HTML代码</returns>
        public static string UBBCodeToHTMLCode(string SourceText)
        {
            if (string.IsNullOrEmpty(SourceText))
            {
                return string.Empty;
            }
            if (!SourceText.Contains("[") || !SourceText.Contains("]"))
            {
                return SourceText;
            }
            string text = SourceText.Replace("[COLOR=(", "[color=(").Replace("[/COLOR]", "[/color]");
            text = text.Replace("[B]", "[b]").Replace("[/B]", "[/b]");
            text = text.Replace("[U]", "[u]").Replace("[/U]", "[/u]");
            text = text.Replace("[I]", "[i]").Replace("[/I]", "[/i]");
            text = text.Replace("[color=(", "<span style=\"color:").Replace(");]", ";\">").Replace("[/color]", "</span>");
            text = text.Replace("[b]", "<b>").Replace("[/b]", "</b>");
            text = text.Replace("[u]", "<u>").Replace("[/u]", "</u>");
            return text.Replace("[i]", "<i>").Replace("[/i]", "</i>");
        }


        #endregion


        #region Http请求模拟器
        /// <summary>
        /// 获取文档内容
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>文档内容</returns>
        public static string GetHTMLContent(string url)
        {
            return GetHTMLContent(url, null, null);
        }
        /// <summary>
        /// 获取文档内容
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="endRegexString">结束标识的正则表达式</param>
        /// <returns>文档内容</returns>
        public static string GetHTMLContent(string url, string endRegexString)
        {
            return GetHTMLContent(url, null, endRegexString);
        }
        /// <summary>
        ///  获取html文档
        ///  如果endRegexString不为空，则获取从开头到第一次匹配endTagRegex为止的部分文档内容；
        ///  否则获取整个html文档
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="encoding">HTML内容编码方式</param>
        /// <param name="endRegexString">结束标识的正则表达式</param>
        /// <returns>文档内容</returns>
        public static string GetHTMLContent(string url, System.Text.Encoding encoding, string endRegexString)
        {
            HttpWebResponse httpWebResponse = null;
            System.IO.Stream stream = null;
            System.IO.StreamReader streamReader = null;
            string result;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = 30000;
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    result = null;
                }
                else
                {
                    stream = httpWebResponse.GetResponseStream();
                    if (encoding == null)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(httpWebResponse.CharacterSet) || httpWebResponse.CharacterSet == "ISO-8859-1")
                            {
                                encoding = GetEncoding(url);
                            }
                            else
                            {
                                encoding = System.Text.Encoding.GetEncoding(httpWebResponse.CharacterSet);
                            }
                        }
                        catch
                        {
                            encoding = System.Text.Encoding.UTF8;
                        }
                        if (encoding == null)
                        {
                            encoding = System.Text.Encoding.UTF8;
                        }
                    }
                    httpWebRequest.Timeout = 8000;
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    stream = httpWebResponse.GetResponseStream();
                    streamReader = new System.IO.StreamReader(stream, encoding);
                    string text;
                    if (string.IsNullOrEmpty(endRegexString))
                    {
                        text = streamReader.ReadToEnd();
                    }
                    else
                    {
                        Regex regex = new Regex(endRegexString, RegexOptions.IgnoreCase);
                        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                        string text2 = string.Empty;
                        while ((text2 = streamReader.ReadLine()) != null)
                        {
                            stringBuilder.Append(text2);
                            text2 = stringBuilder.ToString();
                            if (regex.IsMatch(text2))
                            {
                                break;
                            }
                        }
                        text = stringBuilder.ToString();
                    }
                    streamReader.Close();
                    stream.Close();
                    httpWebResponse.Close();
                    result = text;
                }
            }
            catch (WebException)
            {
                result = null;
            }
            catch (System.IO.IOException)
            {
                result = null;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// 从html文档中得到Encoding
        /// </summary>
        public static Encoding GetEncoding(string url)
        {
            string hTmlContent = GetHTMLContent(url, System.Text.Encoding.UTF8, "charset\\b\\s*=\\s*(?<charset>[a-zA-Z\\d|-]*)");
            Regex regex = new Regex("charset\\b\\s*=\\s*(?<charset>[a-zA-Z\\d|-]*)");
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            if (regex.IsMatch(hTmlContent))
            {
                foreach (Match match in regex.Matches(hTmlContent))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(match.Groups["charset"].Value))
                        {
                            encoding = Encoding.GetEncoding(match.Groups["charset"].Value);
                            if (encoding != null)
                            {
                                break;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return encoding;
        }
        /// <summary>
        /// 获取html内容中的meta部分内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="regStart"></param>
        /// <param name="regEnd"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string GetMetaString(string html, string regStart, string regEnd, bool ignoreCase)
        {
            string pattern = string.Format("{0}(?<getcontent>[\\s|\\S]*?){1}", regStart, regEnd);
            Regex regex;
            if (ignoreCase)
            {
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
            }
            else
            {
                regex = new Regex(pattern);
            }
            return regex.Match(html).Groups["getcontent"].Value;
        }
        /// <summary>
        /// 获取html内容中的Title
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="ignoreCas">是否忽略大小写</param>
        /// <returns>标签title</returns>
        public static string GetTitle(string html, bool ignoreCas)
        {
            string metaString = GetMetaString(html, "<meta name=\"title\"([\\s]*)content=\"", "\"([\\s]*)/?>", ignoreCas);
            if (string.IsNullOrEmpty(metaString))
            {
                string pattern = "(?<=<title.*>)([\\s\\S]*)(?=</title>)";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                return regex.Match(html).Value.Trim();
            }
            return metaString;
        }
        /// <summary>
        /// 获取html代码中的description
        /// </summary>
        /// <param name="html">html内容</param>
        /// <param name="ignoreCas">是否忽略大小写</param>
        /// <returns>description</returns>
        public static string GetDescription(string html, bool ignoreCas)
        {
            string metaString = GetMetaString(html, "<meta([\\s]*)name=\"description\"([\\s]*)content=\"", "\"([\\s]*)/?>", ignoreCas);
            if (string.IsNullOrEmpty(metaString))
            {
                metaString = GetMetaString(html, "<meta([\\s]*)content=\"", "\"([\\s]*)name=\"description\"([\\s]*)/?>", ignoreCas);
            }
            return metaString;
        }
        /// <summary>
        /// 通过url获取html文档
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sMethod"></param>
        /// <param name="Param"></param>
        /// <param name="bAutoRedirect"></param>
        /// <param name="ecode"></param>
        /// <returns></returns>
        public static string GetHtmlByUrl(string url, string sMethod, string Param, bool bAutoRedirect, System.Text.Encoding ecode)
        {
            sMethod = sMethod.ToUpper();
            sMethod = ((sMethod != "POST") ? "GET" : sMethod);
            string result = "";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = sMethod;
            httpWebRequest.AllowAutoRedirect = bAutoRedirect;
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; MyIE2; .NET CLR 1.1.4322)";
            httpWebRequest.Timeout = 10000;
            if (sMethod == "POST")
            {
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Param);
                httpWebRequest.ContentLength = (long)bytes.Length;
                try
                {
                    System.IO.Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                catch (System.Exception)
                {
                    httpWebRequest = null;
                    return "-1";
                }
            }
            HttpWebResponse httpWebResponse = null;
            System.IO.Stream stream = null;
            System.IO.StreamReader streamReader = null;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebResponse.GetResponseStream();
                streamReader = new System.IO.StreamReader(stream, ecode);
                result = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                result = ex.ToString();
            }
            if (httpWebResponse != null)
            {
                httpWebResponse.Close();
                httpWebResponse = null;
            }
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
            if (streamReader != null)
            {
                streamReader.Close();
                streamReader = null;
            }
            httpWebRequest = null;
            return result;
        }
        /// <summary>
        ///  通过UrlCookie获取Html文档
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="sMethod"></param>
        /// <param name="Param"></param>
        /// <param name="bAutoRedirect"></param>
        /// <param name="ecode"></param>
        /// <returns></returns>
        public static string GetHTMLByUrlCookie(string url, ref CookieContainer cookie, string sMethod, string Param, bool bAutoRedirect, System.Text.Encoding ecode)
        {
            sMethod = sMethod.ToUpper();
            sMethod = ((sMethod != "POST") ? "GET" : sMethod);
            string result = "";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.CookieContainer = cookie;
            httpWebRequest.Method = sMethod;
            httpWebRequest.AllowAutoRedirect = bAutoRedirect;
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; MyIE2; .NET CLR 1.1.4322)";
            httpWebRequest.Timeout = 2000;
            if (sMethod == "POST")
            {
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Param);
                httpWebRequest.ContentLength = (long)bytes.Length;
                try
                {
                    System.IO.Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                catch (System.Exception)
                {
                    httpWebRequest = null;
                    return "-1";
                }
            }
            HttpWebResponse httpWebResponse = null;
            System.IO.Stream stream = null;
            System.IO.StreamReader streamReader = null;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebResponse.GetResponseStream();
                streamReader = new System.IO.StreamReader(stream, ecode);
                result = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                result = ex.ToString();
            }
            if (httpWebResponse != null)
            {
                httpWebResponse.Close();
                httpWebResponse = null;
            }
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
            if (streamReader != null)
            {
                streamReader.Close();
                streamReader = null;
            }
            httpWebRequest = null;
            return result;
        }
        /// <summary>
        /// 请求URL得到HTML内容,私有函数
        /// </summary>
        /// <param name="sUrl">url地址</param>
        /// <param name="sEncode">HTML内容编码方式</param>
        /// <param name="iMaxRetry">如果请求失败，最大重试次数</param>
        /// <param name="iCurrentRetry">当前是第几次请求</param>
        /// <returns>HTML内容</returns>
        private static string GetHtml(string sUrl, System.Text.Encoding sEncode, int iMaxRetry, int iCurrentRetry)
        {
            string result = string.Empty;
            try
            {
                Uri requestUri = new Uri(sUrl);
                WebRequest webRequest = WebRequest.Create(requestUri);
                WebResponse response = webRequest.GetResponse();
                System.IO.Stream responseStream = response.GetResponseStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, sEncode);
                result = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();
            }
            catch
            {
                iCurrentRetry++;
                if (iCurrentRetry <= iMaxRetry)
                {
                    GetHtml(sUrl, sEncode, iMaxRetry, iCurrentRetry);
                }
            }
            return result;
        }
        /// <summary>
        /// 带重试功能的获取HTML内容
        /// </summary>
        /// <param name="sUrl">url地址</param>
        /// <param name="sEncode">HTML内容编码方式</param>
        /// <param name="iMaxRetry">如果请求失败，最大重试次数</param>
        /// <returns>HTML内容</returns>
        public static string GetHtmlWithTried(string sUrl, System.Text.Encoding sEncode, int iMaxRetry)
        {
            string empty = string.Empty;
            return GetHtml(sUrl, sEncode, iMaxRetry, 0);
        }

        #endregion


        /// <summary>
        /// 移除html内的Elemtnts/Attributes及&amp;nbsp;，超过charLimit个字符进行截断
        /// </summary>
        /// <param name="rawHtml">待截字的html字符串</param>
        /// <param name="charLimit">最多允许返回的字符数</param>
        public static string TrimHtml(string rawHtml, int charLimit)
        {
            if (string.IsNullOrEmpty(rawHtml))
                return string.Empty;

            string nohtml = StripHtml(rawHtml, true, false);
            nohtml = StripBBTags(nohtml);

            if (charLimit <= 0 || charLimit >= nohtml.Length)
                return nohtml;
            else
                return StringUtility.Trim(nohtml, charLimit);
        }

        /// <summary>
        /// 移除Html标签
        /// </summary>
        /// <param name="rawString">待处理字符串</param>
        /// <param name="removeHtmlEntities">是否移除Html实体</param>
        /// <param name="enableMultiLine">是否保留换行符（<p/><br/>会转换成换行符）</param>
        /// <returns>返回处理后的字符串</returns>
        public static string StripHtml(string rawString, bool removeHtmlEntities, bool enableMultiLine)
        {
            string result = rawString;
            if (enableMultiLine)
            {
                result = Regex.Replace(result, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                result = Regex.Replace(result, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            result = result.Replace("\"", "''");
            if (removeHtmlEntities)
            {
                //StripEntities removes the HTML Entities 
                result = Regex.Replace(result, "&[^;]*;", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            return Regex.Replace(result, "<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }


        /// <summary>
        /// 移除Html用于内容预览
        /// </summary>
        /// <remarks>
        /// 将br、p替换为\n，“'”替换为对应Html实体，并过滤所有Html、Xml、UBB标签
        /// </remarks>
        /// <param name="rawString">用于预览的文本</param>
        /// <returns>返回移除换行及html、ubb标签的字符串</returns>
        public static string StripForPreview(string rawString)
        {
            string tempString;

            tempString = rawString.Replace("<br>", "\n");
            tempString = tempString.Replace("<br/>", "\n");
            tempString = tempString.Replace("<br />", "\n");
            tempString = tempString.Replace("<p>", "\n");
            tempString = tempString.Replace("'", "&#39;");

            tempString = StripHtml(tempString, false, false);
            tempString = StripBBTags(tempString);

            return tempString;
        }

        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="content">待处理的字符串</param>
        /// <remarks>处理后的字符串</remarks>
        public static string StripBBTags(string content)
        {
            return Regex.Replace(content, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 移除script标签
        /// Helper function used to ensure we don't inject script into the db.
        /// </summary>
        /// <remarks>
        /// 移除&lt;script&gt;及javascript:
        /// </remarks>
        /// <param name="rawString">待处理的字符串</param>
        /// <remarks>处理后的字符串</remarks>
        public static string StripScriptTags(string rawString)
        {
            // Perform RegEx
            rawString = Regex.Replace(rawString, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            rawString = rawString.Replace("\"javascript:", "\"");

            return rawString;
        }










        #region 条形码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string get39(string s, int width, int height)
        {
            Hashtable ht = new Hashtable();

            #region 39码 12位
            ht.Add('A', "110101001011");
            ht.Add('B', "101101001011");
            ht.Add('C', "110110100101");
            ht.Add('D', "101011001011");
            ht.Add('E', "110101100101");
            ht.Add('F', "101101100101");
            ht.Add('G', "101010011011");
            ht.Add('H', "110101001101");
            ht.Add('I', "101101001101");
            ht.Add('J', "101011001101");
            ht.Add('K', "110101010011");
            ht.Add('L', "101101010011");
            ht.Add('M', "110110101001");
            ht.Add('N', "101011010011");
            ht.Add('O', "110101101001");
            ht.Add('P', "101101101001");
            ht.Add('Q', "101010110011");
            ht.Add('R', "110101011001");
            ht.Add('S', "101101011001");
            ht.Add('T', "101011011001");
            ht.Add('U', "110010101011");
            ht.Add('V', "100110101011");
            ht.Add('W', "110011010101");
            ht.Add('X', "100101101011");
            ht.Add('Y', "110010110101");
            ht.Add('Z', "100110110101");
            ht.Add('0', "101001101101");
            ht.Add('1', "110100101011");
            ht.Add('2', "101100101011");
            ht.Add('3', "110110010101");
            ht.Add('4', "101001101011");
            ht.Add('5', "110100110101");
            ht.Add('6', "101100110101");
            ht.Add('7', "101001011011");
            ht.Add('8', "110100101101");
            ht.Add('9', "101100101101");
            ht.Add('+', "100101001001");
            ht.Add('-', "100101011011");
            ht.Add('*', "100101101101");
            ht.Add('/', "100100101001");
            ht.Add('%', "101001001001");
            ht.Add('$', "100100100101");
            ht.Add('.', "110010101101");
            ht.Add(' ', "100110101101");
            #endregion

            #region 39码 9位
            //ht.Add('0', "000110100");
            //ht.Add('1', "100100001");
            //ht.Add('2', "001100001");
            //ht.Add('3', "101100000");
            //ht.Add('4', "000110001");
            //ht.Add('5', "100110000");
            //ht.Add('6', "001110000");
            //ht.Add('7', "000100101");
            //ht.Add('8', "100100100");
            //ht.Add('9', "001100100");
            //ht.Add('A', "100001001");
            //ht.Add('B', "001001001");
            //ht.Add('C', "101001000");
            //ht.Add('D', "000011001");
            //ht.Add('E', "100011000");
            //ht.Add('F', "001011000");
            //ht.Add('G', "000001101");
            //ht.Add('H', "100001100");
            //ht.Add('I', "001001100");
            //ht.Add('J', "000011100");
            //ht.Add('K', "100000011");
            //ht.Add('L', "001000011");
            //ht.Add('M', "101000010");
            //ht.Add('N', "000010011");
            //ht.Add('O', "100010010");
            //ht.Add('P', "001010010");
            //ht.Add('Q', "000000111");
            //ht.Add('R', "100000110");
            //ht.Add('S', "001000110");
            //ht.Add('T', "000010110");
            //ht.Add('U', "110000001");
            //ht.Add('V', "011000001");
            //ht.Add('W', "111000000");
            //ht.Add('X', "010010001");
            //ht.Add('Y', "110010000");
            //ht.Add('Z', "011010000");
            //ht.Add('-', "010000101");
            //ht.Add('.', "110000100");
            //ht.Add(' ', "011000100");
            //ht.Add('*', "010010100");
            //ht.Add('$', "010101000");
            //ht.Add('/', "010100010");
            //ht.Add('+', "010001010");
            //ht.Add('%', "000101010");
            #endregion

            s = "*" + s.ToUpper() + "*";

            string result_bin = "";//二进制串

            try
            {
                foreach (char ch in s)
                {
                    result_bin += ht[ch].ToString();
                    result_bin += "0";//间隔，与一个单位的线条宽度相等
                }
            }
            catch { return "存在不允许的字符！"; }

            string result_html = ""; //HTML代码
            string color = "";       //颜色
            foreach (char c in result_bin)
            {
                color = c == '0' ? "#FFFFFF" : "#000000";
                result_html += "<div style=\"width:" + width + "px;height:" + height + "px;float:left;background:" + color + ";\"></div>";
            }
            result_html += "<div style=\"clear:both\"></div>";

            int len = ht['*'].ToString().Length;
            foreach (char c in s)
            {
                result_html += "<div style=\"width:" + (width * (len + 1)) + "px;float:left;color:#000000;text-align:center;\">" + c + "</div>";
            }
            result_html += "<div style=\"clear:both\"></div>";

            return "<div style=\"background:#FFFFFF;padding:5px;font-size:" + (width * 10) + "px;font-family:'楷体';\">" + result_html + "</div>";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string getEAN13(string s, int width, int height)
        {
            int checkcode_input = -1;//输入的校验码
            if (!Regex.IsMatch(s, @"^\d{12}$"))
            {
                if (!Regex.IsMatch(s, @"^\d{13}$"))
                {
                    return "存在不允许的字符！";
                }
                else
                {
                    checkcode_input = int.Parse(s[12].ToString());
                    s = s.Substring(0, 12);
                }
            }

            int sum_even = 0;//偶数位之和
            int sum_odd = 0; //奇数位之和

            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)
                {
                    sum_odd += int.Parse(s[i].ToString());
                }
                else
                {
                    sum_even += int.Parse(s[i].ToString());
                }
            }

            int checkcode = (10 - (sum_even * 3 + sum_odd) % 10) % 10;//校验码

            if (checkcode_input > 0 && checkcode_input != checkcode)
            {
                return "输入的校验码错误！";
            }

            s += checkcode;//变成13位

            // 000000000101左侧42个01010右侧35个校验7个101000000000
            // 6        101左侧6位 01010右侧5位校验1位101000000000

            string result_bin = "";//二进制串
            result_bin += "000000000101";

            string type = ean13type(s[0]);
            for (int i = 1; i < 7; i++)
            {
                result_bin += ean13(s[i], type[i - 1]);
            }
            result_bin += "01010";
            for (int i = 7; i < 13; i++)
            {
                result_bin += ean13(s[i], 'C');
            }
            result_bin += "101000000000";

            string result_html = ""; //HTML代码
            string color = "";       //颜色
            int height_bottom = width * 5;
            foreach (char c in result_bin)
            {
                color = c == '0' ? "#FFFFFF" : "#000000";
                result_html += "<div style=\"width:" + width + "px;height:" + height + "px;float:left;background:" + color + ";\"></div>";
            }
            result_html += "<div style=\"clear:both\"></div>";

            result_html += "<div style=\"float:left;color:#000000;width:" + (width * 9) + "px;text-align:center;\">" + s[0] + "</div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#000000;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#FFFFFF;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#000000;\"></div>";
            for (int i = 1; i < 7; i++)
            {
                result_html += "<div style=\"float:left;width:" + (width * 7) + "px;color:#000000;text-align:center;\">" + s[i] + "</div>";
            }
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#FFFFFF;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#000000;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#FFFFFF;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#000000;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#FFFFFF;\"></div>";
            for (int i = 7; i < 13; i++)
            {
                result_html += "<div style=\"float:left;width:" + (width * 7) + "px;color:#000000;text-align:center;\">" + s[i] + "</div>";
            }
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#000000;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#FFFFFF;\"></div>";
            result_html += "<div style=\"float:left;width:" + width + "px;height:" + height_bottom + "px;background:#000000;\"></div>";
            result_html += "<div style=\"float:left;color:#000000;width:" + (width * 9) + "px;\"></div>";
            result_html += "<div style=\"clear:both\"></div>";

            return "<div style=\"background:#FFFFFF;padding:0px;font-size:" + (width * 10) + "px;font-family:'楷体';\">" + result_html + "</div>";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string ean13(char c, char type)
        {
            switch (type)
            {
                case 'A':
                    {
                        switch (c)
                        {
                            case '0': return "0001101";
                            case '1': return "0011001";
                            case '2': return "0010011";
                            case '3': return "0111101";//011101
                            case '4': return "0100011";
                            case '5': return "0110001";
                            case '6': return "0101111";
                            case '7': return "0111011";
                            case '8': return "0110111";
                            case '9': return "0001011";
                            default: return "Error!";
                        }
                    }
                case 'B':
                    {
                        switch (c)
                        {
                            case '0': return "0100111";
                            case '1': return "0110011";
                            case '2': return "0011011";
                            case '3': return "0100001";
                            case '4': return "0011101";
                            case '5': return "0111001";
                            case '6': return "0000101";//000101
                            case '7': return "0010001";
                            case '8': return "0001001";
                            case '9': return "0010111";
                            default: return "Error!";
                        }
                    }
                case 'C':
                    {
                        switch (c)
                        {
                            case '0': return "1110010";
                            case '1': return "1100110";
                            case '2': return "1101100";
                            case '3': return "1000010";
                            case '4': return "1011100";
                            case '5': return "1001110";
                            case '6': return "1010000";
                            case '7': return "1000100";
                            case '8': return "1001000";
                            case '9': return "1110100";
                            default: return "Error!";
                        }
                    }
                default: return "Error!";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string ean13type(char c)
        {
            switch (c)
            {
                case '0': return "AAAAAA";
                case '1': return "AABABB";
                case '2': return "AABBAB";
                case '3': return "AABBBA";
                case '4': return "ABAABB";
                case '5': return "ABBAAB";
                case '6': return "ABBBAA";//中国
                case '7': return "ABABAB";
                case '8': return "ABABBA";
                case '9': return "ABBABA";
                default: return "Error!";
            }
        }
        #endregion

    }
}
