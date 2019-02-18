using ItemDoc.ConsoleBotServer.AppData;
using ItemDoc.ConsoleBotServer.Helper;
using ItemDoc.Services.Model;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using Polly;
using System;

namespace ItemDoc.ConsoleBotServer
{
    class Program
    {
        private static readonly string title = "ConsoleBotServer";


        //[STAThread]
        static void Main(string[] args)
        {
            //启动程序，测试功能 //初始化Id sheng
            //搜索引擎爬虫，商业爬虫，内容取者爬虫和监控爬虫
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(10, 1);
            Console.Write("欢迎使用ConsoleBotServer监控内容爬虫");
            //Console.WriteLine(); 

            //var path = Config.AppSettings<string>("RemoteConfig", "BotConfig.config");
            //Console.Write(path);
            //var local = FileUtility.GetDiskFilePath(path);

            //Pdf2Html.Instance().PDF2HTML();
            var info = new AttachmentInfo()
            {
                Id = 1,
                AttachmentId = "gg",
                OwnerId = Guid.NewGuid().ToString("N"),
                ServerId = "",
                ServerUrlPath = "",
                Path = "E:/GitHub/ItemDoc/src/Plugin/ItemDoc.ConsoleBotServer/bin/Debug/pdf2temp/demo.xlsx",
                Extension = "xlsx",
                Size = 25677,
                FileNames = "",
                UploadFileName = "",
                MimeType = "",
                Status = AttachmentStatus.None,
                DisplayOrder = 1,
                Ip = "",
                DateCreated = DateTime.Now,
                HasThumbnail = true,
                HtmlPreview = true,
                PageCount = 10,
            };

            var isok1 = Pdf2Html.Instance().ExcelToPDF(info);
            Console.Read();


            //for (int i = 0; i < 10000; i++)
            //{
            //    var url = "http://www.baidu.com";
            //    IWebDriver driver = new ChromeDriver(GetChromeDriverService());
            //    driver.Navigate().GoToUrl(url);
            //    if (!string.IsNullOrWhiteSpace(driver.PageSource))
            //    {
            //        Console.WriteLine(url); 
            //    } 

            //}
            //var url = "https://www.baidu.com";
            //int prot = 8123;
            //string ip = "39.107.84.185";
            //string proxyAddr = $"http://{ip}:{prot}";

            //var proxyUser = "";
            //var proxyPassWord = "";
            //var proxyDomain = "";


            //var isok = FreeProxy.CheckProxy(url, proxyAddr, proxyUser, proxyPassWord, proxyDomain);

            //var vae = Crawler.Instance();
            //vae.OnStart += (s, e) =>
            //{
            //    Console.WriteLine("爬虫开始抓取地址：" + e.Url);
            //};
            //vae.OnError += (s, e) =>
            //{
            //    Console.WriteLine("爬虫抓取出现错误：" + e.Url + "，异常消息：" + e.Exception.ToString());
            //};
            //vae.OnCompleted += async (s, e) =>
            //{
            //    var str = await vae.StartHttpTask(e.URL, new CrawlSettings()
            //    {
            //        CrawlerType = CrawlerType.HttpWebRequest,
            //        AutoSpeedLimit = false,
            //        ProxyOption = new ProxyOptions()
            //        {
            //            Address = proxyAddr
            //        },
            //        RequestOption = new RequestOptions()
            //        {
            //            Method = "post"
            //        }

            //    });
            //    Console.WriteLine(str);
            //};



            Console.Read();



            //System.Console.SetCursorPosition(0,4);//定位光标位置，第四行第一位
            //System.Console.CursorTop;//获取已输出文本的行数
            //System.Console.BufferWidth;//获取控制台的宽度

            // 所有log 日志全部存储到数据库，方便记录。


            //获取当前用模板设置的代理账号。
            //Console.Title = title;
            //Closebtn();
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(CloseConsole);
            //Console.WriteLine("管理器启动中...");

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ProxyForm());




            //Console.WriteLine("按任意键退出...");
            //Console.Read();

            //Console.ReadKey();

            //重试、超时处理、缓存、返回、

            //Polly();


        }

        public void ookk()
        {


        }

        private static PhantomJSDriverService GetPhantomJSDriverService()
        {
            PhantomJSDriverService pds = PhantomJSDriverService.CreateDefaultService();
            //设置代理服务器地址
            //pds.Proxy = $"{ip}:{port}"; 
            //设置代理服务器认证信息
            //pds.ProxyAuthentication = GetProxyAuthorization();

            return pds;
        }
        private static ChromeDriverService GetChromeDriverService()
        {
            var pds = ChromeDriverService.CreateDefaultService();


            //设置代理服务器地址
            //pds.Proxy = $"{ip}:{port}"; 
            //设置代理服务器认证信息
            //pds.ProxyAuthentication = GetProxyAuthorization();
            pds.HideCommandPromptWindow = true;

            //var options = new ChromeOptions();

            //options.AddArguments("--proxy-server=" + "<< IP Address >>" + ":" + "<< Port Number >>");


            //options.Proxy = null;

            //string userAgent = "<< User Agent Text >>";

            //options.AddArgument($"--user-agent={userAgent}$PC${"<< User Name >>" + ":" + "<< Password >>"}");



            return pds;
        }
        /// <summary>
        /// 禁用关闭按钮
        /// </summary>
        static void Closebtn()
        {
            IntPtr windowHandle = Win32Helper.FindWindow(null, title);
            IntPtr closeMenu = Win32Helper.GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            Win32Helper.RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }



        /// <summary>  
        /// 关闭时的事件  
        /// </summary>  
        /// <param name="sender">对象</param>  
        /// <param name="e">参数</param>  
        protected static void CloseConsole(object sender, ConsoleCancelEventArgs e)
        {
            Environment.Exit(0);
            //return;
        }

        /// <summary>
        ///   //重试、超时处理、缓存、返回、
        /// </summary>
        static void Polly()
        {
            var fallBackPolicy =
                Policy<string>
                    .Handle<Exception>()
                    .Fallback("执行失败，返回Fallback");

            var fallBack = fallBackPolicy.Execute(() =>
            {
                return "zhe shi yi ge ce shi";
            });
            Console.WriteLine(fallBack);

            var politicaWaitAndRetry =
                Policy<string>
                    .Handle<Exception>()
                    .Retry(3, (ex, count) =>
                    {
                        Console.WriteLine("执行失败! 重试次数 {0}", count);
                        Console.WriteLine("异常来自 {0}", ex.GetType().Name);
                    });

            var mixedPolicy = Policy.Wrap(fallBackPolicy, politicaWaitAndRetry);
            var mixedResult = mixedPolicy.Execute(ThrowException);
            Console.WriteLine($"执行结果: {mixedResult}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static string ThrowException()
        {
            throw new Exception();
        }


        static int Compute()
        {
            var a = 0;
            return 1 / a;
        }
    }
}
