﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ItemDoc.ConsoleBot.Events;
using ItemDoc.ConsoleBot.Models;
using ItemDoc.Core.WebCrawler;
using ItemDoc.Core.WebCrawler.Events;
using ItemDoc.Framework.Utility;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace ItemDoc.ConsoleBot.WebCrawler
{
    /// <summary>
    /// 基于Selenium+PhantomJS实现的爬虫
    /// </summary>
    public class Crawler
    {
        #region Instance

        private static volatile Crawler _instance = null;
        private static readonly object Lock = new object();

        /// <summary>
        /// SiteUrls单例实体
        /// </summary>
        /// <returns></returns>
        public static Crawler Instance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Crawler();
                    }
                }
            }
            return _instance;
        }

        #endregion Instance
        /// <summary>
        /// 爬虫启动事件
        /// </summary>
        public event EventHandler<OnStartEventArgs> OnStart;
        /// <summary>
        /// 爬虫完成事件
        /// </summary>
        public event EventHandler<OnCompletedEventArgs> OnCompleted;
        /// <summary>
        /// 爬虫出错事件
        /// </summary>
        public event EventHandler<OnErrorEventArgs> OnError;
        /// <summary>
        /// 定义PhantomJS内核参数
        /// </summary>
        private PhantomJSOptions _options;
        /// <summary>
        /// 定义Selenium驱动配置
        /// </summary>
        private PhantomJSDriverService _service;
         
        /// <summary>
        /// 高级爬虫
        /// </summary>
        /// <param name="url">抓取地址URL</param>
        /// <param name="script">要执行的Javascript脚本代码</param>
        /// <param name="operation">要执行的页面操作</param>
        /// <returns></returns>
        public async Task Start(string url, CrawlSettings settings)
        {
            await Task.Run(() =>
            {
                OnStart?.Invoke(this, new OnStartEventArgs(url));
                this._options = new PhantomJSOptions();//定义PhantomJS的参数配置对象
                if (string.IsNullOrEmpty(settings.Path))
                {
                    settings.Path = FileUtility.GetDiskFilePath("~/App_Data");
                }
                this._service = PhantomJSDriverService.CreateDefaultService(settings.Path);//初始化Selenium配置，传入存放phantomjs.exe文件的目录
                _service.IgnoreSslErrors = true;//忽略证书错误
                _service.WebSecurity = false;//禁用网页安全
                _service.HideCommandPromptWindow = true;//隐藏弹出窗口
                _service.LoadImages = false;//禁止加载图片
                _service.LocalToRemoteUrlAccess = true;//允许使用本地资源响应远程 URL
                _options.AddAdditionalCapability(@"phantomjs.page.settings.userAgent",
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                if (settings.ProxyOption != null)
                {
                    _service.ProxyType = "HTTP";//使用HTTP代理
                    _service.Proxy = settings.ProxyOption.Address;//代理IP及端口
                }
                else
                {
                    _service.ProxyType = "none";//不使用代理
                }


                var driver = new PhantomJSDriver(_service, _options);//实例化PhantomJS的WebDriver
                try
                {
                    var watch = DateTime.Now;
                    driver.Navigate().GoToUrl(url);//请求URL地址
                    if (settings.ScriptCode != null) driver.ExecuteScript(settings.ScriptCode, settings.ScriptCode);//执行Javascript代码

                    settings.Action?.Invoke(driver);
                    //var driverWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(operation.Timeout));//设置超时时间为x毫秒
                    //if (operation.Condition != null) driverWait.Until(operation.Condition);
                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = DateTime.Now.Subtract(watch).Milliseconds;//获取请求执行时间;
                    var pageSource = driver.PageSource;//获取网页Dom结构
                    this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(url, threadId, milliseconds, pageSource, driver));
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, new OnErrorEventArgs(url, ex));
                }
                finally
                {
                    driver.Close();
                    driver.Quit();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                }
            });
        }

        /// <summary>
        /// 异步创建爬虫
        /// </summary> 
        /// <param name="url">爬虫URL地址</param>
        /// <param name="settings">代理服务器</param>
        /// <returns>网页源代码</returns>
        public async Task<string> StartHttpTask(string url, CrawlSettings settings)
        {
            return await Task.Run(() =>
            {
                var pageSource = string.Empty;
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        throw new NullReferenceException("url is no null");
                    }
                    OnStart?.Invoke(this, new OnStartEventArgs(url));
                    var watch = new Stopwatch();
                    // 1~5 秒随机间隔的自动限速
                    if (settings.AutoSpeedLimit)
                        Thread.Sleep(new Random().Next(1000, 5000));
                    watch.Start();
                    //处理HttpWebRequest访问https有安全证书的问题（ 请求被中止: 未能创建 SSL/TLS 安全通道。）
                    ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    // 创建并配置Web请求  
                    request = WebRequest.Create(url) as HttpWebRequest;

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
                        this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(url, Thread.CurrentThread.ManagedThreadId, watch.ElapsedMilliseconds, response.StatusCode));
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
                    this.OnCompleted?.Invoke(this, new OnCompletedEventArgs(url, threadId, milliseconds, pageSource));
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, new OnErrorEventArgs(url, ex));
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


    }

}


