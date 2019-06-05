using Common.Logging;
using Sop.FileServer.Events;
using Sop.FileServer.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sop.FileServer.WebCrawler
{
    public class HttpWebCrawler
    {
        private readonly ILog _logger = LogManager.GetLogger<HttpWebCrawler>();
        public event EventHandler<OnCompletedEventArgs> OnCompleted;
        /// <summary>
        /// 异步爬虫
        /// </summary>  
        /// <param name="settings">代理服务器</param>
        /// <returns>网页源代码</returns>
        public async Task<string> AsyncRun(CrawlSettings settings)
        {
            return await Task.Run(() =>
            {
                var pageSource = string.Empty;
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    if (string.IsNullOrWhiteSpace(settings.Url))
                    {
                        throw new NullReferenceException("url is no null");
                    }

                    var watch = new Stopwatch();
                  
                    watch.Start();
                    //处理HttpWebRequest访问https有安全证书的问题（ 请求被中止: 未能创建 SSL/TLS 安全通道。）
                    ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    // 创建并配置Web请求  
                    request = WebRequest.Create(settings.Url) as HttpWebRequest;

                    //设置配合着
                    if (request != null)
                    {
                        request.Accept = settings.RequestOption.Accept;
                        request.KeepAlive = settings.RequestOption.KeepAlive;//启用长连接
                        request.Method = settings.RequestOption.Method;
                        request.UserAgent = settings.RequestOption.UserAgent;
                        request.CookieContainer = settings.RequestOption.CookiesContainer;
                        request.ProtocolVersion = HttpVersion.Version10;
                        request.Accept = settings.RequestOption.Accept;
                        request.MediaType = settings.RequestOption.MediaType;
                        request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
                        request.ServicePoint.Expect100Continue = false;//加快载入速度
                        request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                        request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
                        request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                        request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");//定义gzip压缩页面支持
                        request.ContentType = "application/x-www-form-urlencoded";//定义文档类型及编码
                        request.AllowAutoRedirect = false;//禁止自动跳转
                        if (settings.RequestOption.Timeout > 0)
                        {
                            request.Timeout = settings.RequestOption.Timeout;
                        }
                        if (settings.ProxyOption != null)
                        {
                            WebProxy currentWebProxy = new WebProxy(settings.ProxyOption.Address, true);
                            if (!string.IsNullOrWhiteSpace(settings.ProxyOption.UserName)
                                && !string.IsNullOrWhiteSpace(settings.ProxyOption.PassWord))
                            {
                                currentWebProxy.Credentials = new System.Net.NetworkCredential(settings.ProxyOption.UserName, settings.ProxyOption.PassWord, settings.ProxyOption.Domain);
                            }

                            else
                                currentWebProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                            //设置代理
                            request.Proxy = currentWebProxy;
                        }
                        response = request.GetResponse() as HttpWebResponse;
                    }
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(settings.Url, Thread.CurrentThread.ManagedThreadId, watch.ElapsedMilliseconds, response.StatusCode));
                        using (response)
                        {
                            foreach (Cookie cookie in response.Cookies)
                                settings.RequestOption.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态

                            #region 如果页面压缩，则解压数据流
                            Stream stream = null;

                            if (response.ContentEncoding.ToLower().Contains("gzip"))
                            {
                                Stream responseStream = response.GetResponseStream();
                                if (responseStream != null)
                                {
                                    stream = new GZipStream(responseStream, CompressionMode.Decompress);
                                }
                            }
                            else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                            {
                                Stream responseStream = response.GetResponseStream();
                                if (responseStream != null)
                                {
                                    stream = new DeflateStream(responseStream, CompressionMode.Decompress);
                                }
                            }
                            else
                            {
                                stream = response.GetResponseStream();
                            }
                            #endregion
                            using (stream)
                            {
                                var memoryStream = new MemoryStream();
                                if (stream != null)
                                {
                                    stream.CopyTo(memoryStream);
                                    byte[] buffer = memoryStream.ToArray();
                                    Encoding encode = Encoding.ASCII;
                                    string html = encode.GetString(buffer);

                                    string localCharacterSet = response.CharacterSet;

                                    Match match = Regex.Match(html, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase);
                                    if (match.Success)
                                    {
                                        localCharacterSet = match.Groups[2].Value;

                                        var stringBuilder = new StringBuilder();
                                        foreach (char item in localCharacterSet)
                                        {
                                            if (item == ' ')
                                            {
                                                break;
                                            }

                                            if (item != '\"')
                                            {
                                                stringBuilder.Append(item);
                                            }
                                        }

                                        localCharacterSet = stringBuilder.ToString();
                                    }

                                    if (string.IsNullOrEmpty(localCharacterSet))
                                    {
                                        localCharacterSet = response.CharacterSet;
                                    }
                                    encode = !string.IsNullOrEmpty(localCharacterSet)
                                        ? Encoding.GetEncoding(localCharacterSet)
                                        : Encoding.GetEncoding(936);

                                    memoryStream.Close();
                                    pageSource = encode.GetString(buffer);

                                    stream?.Close();
                                }
                            }
                        }

                    }


                    watch.Stop();
                    var threadId = Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                    this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(settings.Url, threadId, milliseconds, pageSource));
                }
                catch (Exception ex)
                {
                    _logger.Error("HttpWebCrawler: " + ex.Message);
                }
                finally
                {
                    request?.Abort();
                    response?.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }


                return pageSource;
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public string Run(CrawlSettings settings)
        {
            var pageSource = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(settings.Url))
                {
                    throw new NullReferenceException("url is no null");
                }
                var watch = new Stopwatch();
              
                watch.Start();
                //处理HttpWebRequest访问https有安全证书的问题（ 请求被中止: 未能创建 SSL/TLS 安全通道。）
                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                // 创建并配置Web请求  
                request = WebRequest.Create(settings.Url) as HttpWebRequest;

                //设置配合着
                if (request != null)
                {
                    request.Accept = settings.RequestOption.Accept;
                    request.KeepAlive = settings.RequestOption.KeepAlive;//启用长连接
                    request.Method = settings.RequestOption.Method;
                    request.UserAgent = settings.RequestOption.UserAgent;
                    request.CookieContainer = settings.RequestOption.CookiesContainer;
                    request.ProtocolVersion = HttpVersion.Version10;
                    request.Accept = settings.RequestOption.Accept;
                    request.MediaType = settings.RequestOption.MediaType;
                    request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
                    request.ServicePoint.Expect100Continue = false;//加快载入速度
                    request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                    request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
                    request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                    request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");//定义gzip压缩页面支持
                    request.ContentType = "application/x-www-form-urlencoded";//定义文档类型及编码
                    request.AllowAutoRedirect = false;//禁止自动跳转
                    if (settings.RequestOption.Timeout > 0)
                    {
                        request.Timeout = settings.RequestOption.Timeout;
                    }
                    if (settings.ProxyOption != null)
                    {
                        WebProxy currentWebProxy = new WebProxy(settings.ProxyOption.Address, true);
                        if (!string.IsNullOrWhiteSpace(settings.ProxyOption.UserName)
                            && !string.IsNullOrWhiteSpace(settings.ProxyOption.PassWord))
                        {
                            currentWebProxy.Credentials = new System.Net.NetworkCredential(settings.ProxyOption.UserName, settings.ProxyOption.PassWord, settings.ProxyOption.Domain);
                        }

                        else
                            currentWebProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        //设置代理
                        request.Proxy = currentWebProxy;
                    }
                    response = request.GetResponse() as HttpWebResponse;
                }
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(settings.Url, Thread.CurrentThread.ManagedThreadId, watch.ElapsedMilliseconds, response.StatusCode));
                    using (response)
                    {
                        foreach (Cookie cookie in response.Cookies)
                            settings.RequestOption.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态

                        #region 如果页面压缩，则解压数据流
                        Stream stream = null;

                        if (response.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            Stream responseStream = response.GetResponseStream();
                            if (responseStream != null)
                            {
                                stream = new GZipStream(responseStream, CompressionMode.Decompress);
                            }
                        }
                        else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                        {
                            Stream responseStream = response.GetResponseStream();
                            if (responseStream != null)
                            {
                                stream = new DeflateStream(responseStream, CompressionMode.Decompress);
                            }
                        }
                        else
                        {
                            stream = response.GetResponseStream();
                        }
                        #endregion
                        using (stream)
                        {
                            var memoryStream = new MemoryStream();
                            if (stream != null)
                            {
                                stream.CopyTo(memoryStream);
                                byte[] buffer = memoryStream.ToArray();
                                Encoding encode = Encoding.ASCII;
                                string html = encode.GetString(buffer);

                                string localCharacterSet = response.CharacterSet;

                                Match match = Regex.Match(html, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase);
                                if (match.Success)
                                {
                                    localCharacterSet = match.Groups[2].Value;

                                    var stringBuilder = new StringBuilder();
                                    foreach (char item in localCharacterSet)
                                    {
                                        if (item == ' ')
                                        {
                                            break;
                                        }

                                        if (item != '\"')
                                        {
                                            stringBuilder.Append(item);
                                        }
                                    }

                                    localCharacterSet = stringBuilder.ToString();
                                }

                                if (string.IsNullOrEmpty(localCharacterSet))
                                {
                                    localCharacterSet = response.CharacterSet;
                                }
                                encode = !string.IsNullOrEmpty(localCharacterSet)
                                    ? Encoding.GetEncoding(localCharacterSet)
                                    : Encoding.GetEncoding(936);

                                memoryStream.Close();
                                pageSource = encode.GetString(buffer);

                                stream?.Close();
                            }
                        }
                    }

                }


                watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(settings.Url, threadId, milliseconds, pageSource));
            }
            catch (Exception ex)
            {
                _logger.Error("HttpWebCrawler: " + ex.Message);
            }
            finally
            {
                request?.Abort();
                response?.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }


            return pageSource;
        }
    }

}