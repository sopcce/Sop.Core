using Common.Logging;
using OpenQA.Selenium.PhantomJS;
using Sop.FileServer.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Sop.FileServer.Spider.Events;

namespace Sop.FileServer.WebCrawler
{
    /// <summary>
    /// 基于Selenium+PhantomJS实现的爬虫
    /// </summary>
    public class Crawler
    {
        #region Instance
        private readonly ILog _logger = LogManager.GetLogger<Crawler>();
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
        /// <summary>
        /// 爬虫完成事件
        /// </summary>
        public event EventHandler<OnCompletedEventArgs> OnCompleted;

        /// <summary>
        /// 定义PhantomJS内核参数
        /// </summary>
        private PhantomJSOptions _options;
        /// <summary>
        /// 定义Selenium驱动配置
        /// </summary>
        private PhantomJSDriverService _service;

        #endregion Instance


        ///// <summary>
        ///// 高级爬虫
        ///// </summary>
        ///// <param name="url">抓取地址URL</param>
        ///// <param name="settings"></param>
        ///// <returns></returns>
        ////public void Start(string url, CrawlSettings settings)
        ////{
        ////   // Task.Run(() =>
        ////   //{
        ////   //    //初始化临时目录
        ////   //    var homePath = Assembly.GetExecutingAssembly().Location;
        ////   //    homePath = homePath.Substring(0, homePath.LastIndexOf('\\'));
        ////   //    var tempPath = Path.Combine(homePath, "pdf2temp");
        ////   //    if (!Directory.Exists(tempPath))
        ////   //    {
        ////   //        Directory.CreateDirectory(tempPath);
        ////   //    }
        ////   //    this._options = new PhantomJSOptions(); //定义PhantomJS的参数配置对象
        ////   //    this._options.AddAdditionalCapability(@"phantomjs.page.settings.userAgent",
        ////   //        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");



        ////   //    //var info = PhantomJSDriverService.CreateDefaultService();
        ////   //    //info.CookiesFile = "cookiesFile";
        ////   //    //info.DiskCache = true;
        ////   //    //info.IgnoreSslErrors = true;
        ////   //    //info.LoadImages = true;
        ////   //    //info.LocalToRemoteUrlAccess = true;
        ////   //    //info.MaxDiskCacheSize = 1000;
        ////   //    //info.OutputEncoding = "abc";
        ////   //    //info.Proxy = "address:999";
        ////   //    //info.ProxyType = "socks5";
        ////   //    //info.ScriptEncoding = "def";
        ////   //    //info.SslProtocol = "sslv2";
        ////   //    //info.WebSecurity = true;
        ////   //    //string json = info.ToJson();

        ////   //    //File.WriteAllText(@"C:\myconfig.json", json);




        ////   //    var file = new DirectoryInfo(homePath)?.GetFiles("phantomjs.exe")[0];
        ////   //    this._service = file.Exists ? PhantomJSDriverService.CreateDefaultService(file.DirectoryName) : PhantomJSDriverService.CreateDefaultService();



        ////   //    _service.IgnoreSslErrors = true; //忽略证书错误
        ////   //    _service.WebSecurity = false; //禁用网页安全
        ////   //    _service.HideCommandPromptWindow = true; //隐藏弹出窗口
        ////   //    _service.LoadImages = true; //禁止加载图片
        ////   //    _service.LocalToRemoteUrlAccess = true; //允许使用本地资源响应远程 URL


        ////   //    if (settings.ProxyOption != null)
        ////   //    {
        ////   //        _service.ProxyType = "HTTP"; //使用HTTP代理
        ////   //        _service.Proxy = settings.ProxyOption.Address; //代理IP及端口
        ////   //    }
        ////   //    else
        ////   //    {
        ////   //        _service.ProxyType = "none"; //不使用代理
        ////   //    }


        ////   //    var driver = new PhantomJSDriver(_service, _options); //实例化PhantomJS的WebDriver
        ////   //    try
        ////   //    {
        ////   //        Stopwatch stopwatch = new Stopwatch();
        ////   //        driver.Navigate().GoToUrl(url); //请求URL地址
        ////   //        if (settings.ScriptCode != null)
        ////   //            driver.ExecuteScript(settings.ScriptCode, settings.ScriptCode); //执行Javascript代码 
        ////   //        settings.Action?.Invoke(driver);
        ////   //        var watch = DateTime.Now;

        ////   //        var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId; //获取当前任务线程ID
        ////   //        var milliseconds = DateTime.Now.Subtract(watch).Milliseconds; //获取请求执行时间;

        ////   //        var pageSource = driver.PageSource; //获取网页Dom结构 
        ////   //        this.OnCompleted?.Invoke(this,
        ////   //            new OnCompletedEventArgs(url, threadId, milliseconds, pageSource, driver));

        ////   //    }
        ////   //    catch (Exception ex)
        ////   //    {
        ////   //        _logger.Error("---erop :" + ex.Message);
        ////   //    }
        ////   //    finally
        ////   //    {
        ////   //        driver.Close();
        ////   //        driver.Dispose();
        ////   //        driver.Quit();
        ////   //        GC.Collect();
        ////   //        GC.WaitForPendingFinalizers();
        ////   //    }
        ////   //});
        ////}
         
    }

}



