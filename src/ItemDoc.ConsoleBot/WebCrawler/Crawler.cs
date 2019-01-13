using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ItemDoc.ConsoleBot.WebCrawler.Events;
using ItemDoc.Core.WebCrawler;
using ItemDoc.Core.WebCrawler.Events;
using ItemDoc.Core.WebCrawler.Models;
using ItemDoc.Framework.Utility;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace ItemDoc.ConsoleBot.WebCrawler
{
    /// <summary>
    /// 基于Selenium+PhantomJS实现的爬虫
    /// </summary>
    public class Crawler : ICrawler
    {
        public event EventHandler<OnStartEventArgs> OnStart;//爬虫启动事件

        public event EventHandler<OnCompletedEventArgs> OnCompleted;//爬虫完成事件

        public event EventHandler<OnErrorEventArgs> OnError;//爬虫出错事件
        /// <summary>
        /// 定义PhantomJS内核参数
        /// </summary>
        private PhantomJSOptions _options;
        /// <summary>
        /// 定义Selenium驱动配置
        /// </summary>
        private PhantomJSDriverService _service;

        public CrawlSettings Settings { get; private set; }

        public Crawler()
        {
            Settings = new CrawlSettings();
            switch (Settings.CrawlerType)
            {
                case CrawlerType.HttpWebRequest:
                    break;
                case CrawlerType.PhantomJS:
                    this._options = new PhantomJSOptions();//定义PhantomJS的参数配置对象
                    if (string.IsNullOrEmpty(Settings.Path))
                    {
                        Settings.Path = FileUtility.GetDiskFilePath("~/App_Data");
                    }
                    this._service = PhantomJSDriverService.CreateDefaultService(Settings.Path);//初始化Selenium配置，传入存放phantomjs.exe文件的目录
                    _service.IgnoreSslErrors = true;//忽略证书错误
                    _service.WebSecurity = false;//禁用网页安全
                    _service.HideCommandPromptWindow = true;//隐藏弹出窗口
                    _service.LoadImages = false;//禁止加载图片
                    _service.LocalToRemoteUrlAccess = true;//允许使用本地资源响应远程 URL
                    _options.AddAdditionalCapability(@"phantomjs.page.settings.userAgent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                    if (Settings.Proxy != null)
                    {
                        _service.ProxyType = "HTTP";//使用HTTP代理
                        _service.Proxy = Settings.Proxy;//代理IP及端口
                    }
                    else
                    {
                        _service.ProxyType = "none";//不使用代理
                    }

                    break;
            }
        }




        /// <summary>
        /// 高级爬虫
        /// </summary>
        /// <param name="uri">抓取地址URL</param>
        /// <param name="script">要执行的Javascript脚本代码</param>
        /// <param name="operation">要执行的页面操作</param>
        /// <returns></returns>
        public async Task Start(Uri uri, Script script, Operation operation)
        {
            await Task.Run(() =>
            {
                if (OnStart != null) this.OnStart(this, new OnStartEventArgs(uri));

                var driver = new PhantomJSDriver(_service, _options);//实例化PhantomJS的WebDriver
                try
                {
                    var watch = DateTime.Now;
                    driver.Navigate().GoToUrl(uri.ToString());//请求URL地址
                    if (script != null) driver.ExecuteScript(script.Code, script.Args);//执行Javascript代码
                    if (operation.Action != null) operation.Action.Invoke(driver);
                    var driverWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(operation.Timeout));//设置超时时间为x毫秒
                    if (operation.Condition != null) driverWait.Until(operation.Condition);
                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = DateTime.Now.Subtract(watch).Milliseconds;//获取请求执行时间;
                    var pageSource = driver.PageSource;//获取网页Dom结构
                    this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(uri, threadId, milliseconds, pageSource, driver));
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, new OnErrorEventArgs(uri, ex));
                }
                finally
                {
                    driver.Close();
                    driver.Quit();
                }
            });
        }

        /// <summary>
        /// 异步创建爬虫
        /// </summary>
        /// <param name="uri">爬虫URL地址</param>
        /// <param name="proxy">代理服务器</param>
        /// <returns>网页源代码</returns>
        public async Task<string> Start(Uri uri, string proxy = null)
        {
            return await Task.Run(() =>
            {
                var pageSource = string.Empty;
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    OnStart?.Invoke(this, new OnStartEventArgs(uri));
                    var watch = new Stopwatch();
                    // 1~5 秒随机间隔的自动限速
                    if (Settings.AutoSpeedLimit)
                        Thread.Sleep(new Random().Next(1000, 5000));
                    watch.Start();
                    //处理HttpWebRequest访问https有安全证书的问题（ 请求被中止: 未能创建 SSL/TLS 安全通道。）
                    ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    // 创建并配置Web请求  
                    request = WebRequest.Create(uri) as HttpWebRequest;
                    //
                    ConfigRequest(request);
                    if (request != null)
                    {
                        response = request.GetResponse() as HttpWebResponse;
                    }

                    if (response != null)
                    {
                        using (response)
                        {
                            foreach (Cookie cookie in response.Cookies)
                                Settings.Options.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态

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
                                pageSource = this.ParseContent(stream, response.CharacterSet);

                                stream?.Close();
                            }
                        }

                    }


                    watch.Stop();
                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                    this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(uri, threadId, milliseconds, pageSource));
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, new OnErrorEventArgs(uri, ex));
                }
                finally
                {
                    request?.Abort();
                    response?.Close();
                    GC.Collect();
                }


                return pageSource;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private void ConfigRequest(HttpWebRequest request)
        {
            request.Accept = Settings.Options.Accept;
            request.KeepAlive = Settings.Options.KeepAlive;//启用长连接
            request.Method = Settings.Options.Method;
            request.UserAgent = Settings.Options.UserAgent;
            request.CookieContainer = Settings.Options.CookiesContainer;
            request.ProtocolVersion = HttpVersion.Version10;
            request.AllowAutoRedirect = true;
            request.Accept = this.Settings.Options.Accept;
            request.MediaType = this.Settings.Options.MediaType;
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
            request.ServicePoint.Expect100Continue = false;//加快载入速度
            request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
            request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
            request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");//定义gzip压缩页面支持
            request.ContentType = "application/x-www-form-urlencoded";//定义文档类型及编码
            request.AllowAutoRedirect = false;//禁止自动跳转
            if (Settings.Options.Timeout > 0)
            {
                request.Timeout = Settings.Options.Timeout;
            }
        }

        /// <summary>
        /// 解析内容
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="characterSet"></param>
        /// <returns></returns>
        private string ParseContent(Stream stream, string characterSet)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            byte[] buffer = memoryStream.ToArray();

            Encoding encode = Encoding.ASCII;
            string html = encode.GetString(buffer);

            string localCharacterSet = characterSet;

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
                localCharacterSet = characterSet;
            }

            if (!string.IsNullOrEmpty(localCharacterSet))
            {
                encode = Encoding.GetEncoding(localCharacterSet);
            }
            else
            {
                encode = Encoding.GetEncoding(936);
            }

            memoryStream.Close();

            return encode.GetString(buffer);
        }

    }

}



