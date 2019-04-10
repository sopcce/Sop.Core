using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using Polly;
using Sop.FileServer.Helper;
using Sop.Framework.Utility;
using Sop.Framework.WebUtility;
using Sop.Services.Parameter;

namespace Sop.FileServer
{
    public class Program
    {
        private static readonly string title = "Console.Server";



        //[STAThread]
        static void Main(string[] args)
        {
            //启动程序，测试功能 //初始化Id sheng
            //搜索引擎爬虫，商业爬虫，内容取者爬虫和监控爬虫
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("欢迎使用Console.Server监控内容爬虫");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();


            /*
             * 1、定义一个xml文件内，可以使用导入导出的方式配置爬虫
             * 
             */

            string token = "dd35a2425";
            string data = "1";
            data = EncryptionUtility.AES_Decrypt(data, token);
            long date = DateTime.Now.Ticks;

            string newToken = EncryptionUtility.Sha512Encode(data + date);
            var url = $"https://localhost:44326/v1/api/Upload?token={newToken}&data={data}&-={date}";

            var path = "E:\\Img\\404.jpg";
            var strHttpPost = HttpUtility.Instance().HttpPost(url, FileUtility.GetFileStream(path));

            Console.WriteLine($"strHttpPost:{strHttpPost}");
            Console.ReadKey();

            //var hotelUrl = "http://www.xicidaili.com/nt/1";

            //var hotelCrawler = new Crawler();
            //hotelCrawler.OnCompleted += (s, e) =>
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("===============================================" + Environment.NewLine);
            //    sb.Append("地址：" + e.URL.ToString() + Environment.NewLine);
            //    sb.Append("耗时：" + e.Milliseconds + "毫秒");
            //    sb.Append("===============================================" + Environment.NewLine);

            //    ////By.XPath("//input[contains(@id, 'ip_list')]")

            //    var comments = e.WebDriver.FindElement(By.Id("ip_list") );
            //    //找到约53条结果

            //    var totaltText = comments.FindElement(By.XPath("div[@id='pagebar_container']/div[@class='mun']")).Text;
            //    var wait = new WebDriverWait(e.WebDriver, TimeSpan.FromSeconds(3));


            //    sb.AppendLine();
            //    sb.Append("===============================================");
            //    sb.AppendLine();
            //    sb.Append("找到结果：" + totaltText);
            //    sb.AppendLine();
            //    string total = System.Text.RegularExpressions.Regex.Replace(totaltText, @"[^0-9]+", "");
            //    sb.Append("找到结果：" + total);


            //    var contents = comments.FindElements(By.XPath("ul[@class='news-list2']/li"));
            //    sb.AppendLine();
            //    foreach (var content in contents)
            //    {
            //        sb.Append("===============================================");
            //        sb.AppendLine();
            //        var name = content.FindElement(By.XPath("div[@class='gzh-box2']/div[@class='txt-box']/p[@class='tit']")).Text;
            //        sb.Append("名称：" + name);
            //        sb.AppendLine();
            //        sb.Append("微信号：" + content.FindElement(By.XPath("div[@class='gzh-box2']/div[@class='txt-box']/p[@class='info']/label[@name='em_weixinhao']")).Text);
            //        //sb.AppendLine();
            //        //sb.Append("发文：" + content.FindElement(By.XPath("div[@class='gzh-box2']/div[@class='txt-box']/p[@class='info']/text()[3]")).Text);
            //        //sb.AppendLine();
            //        //sb.Append("功能介绍：" + content.FindElement(By.XPath("dl[1]/dd")).Text);
            //        //sb.AppendLine();
            //        //sb.Append("微信认证：" + content.FindElement(By.XPath("dl[2]/dd")).Text);
            //        //sb.AppendLine();


            //        //if (content.FindElement(By.XPath("dl[3]/dd/a")).IsExist())
            //        //{
            //        //    sb.Append("最近文章："
            //        //              + content.FindElement(By.XPath("dl[3]/dd/a")).Text + Environment.NewLine
            //        //              + content.FindElement(By.XPath("dl[3]/dd/a")).GetAttribute("href"));
            //        //    sb.AppendLine();
            //        //}


            //        //div/div[2]/p[2]/
            //        //sb.Append("微信号：" + content.FindElement(By.XPath("div[@class='gzh-box2']/div[@class='txt-box']/p[@class='info']")).Text);
            //        //sb.Append("找到结果：" + content.FindElement(By.XPath("div[contains(@class,'user_info')]/p[@class='name']")).Text);
            //        sb.AppendLine();

            //    }

            //    //var hotelName = e.WebDriver.FindElement(By.XPath("//*[@id='J_htl_info']/div[@class='name']/h2[@class='cn_n']")).Text;
            //    //var address = e.WebDriver.FindElement(By.XPath("//*[@id='J_htl_info']/div[@class='adress']")).Text;
            //    //var price = e.WebDriver.FindElement(By.XPath("//*[@id='div_minprice']/p[1]")).Text;
            //    //var score = e.WebDriver.FindElement(By.XPath("//*[@id='divCtripComment']/div[1]/div[1]/span[3]/span")).Text;
            //    //var reviewCount = e.WebDriver.FindElement(By.XPath("//*[@id='commentTab']/a")).Text;

            //    //var comments = e.WebDriver.FindElement(By.XPath("//*[@id='hotel_info_comment']/div[@id='commentList']/div[1]/div[1]/div[1]"));
            //    //var currentPage = Convert.ToInt32(comments.FindElement(By.XPath("div[@class='c_page_box']/div[@class='c_page']/div[contains(@class,'c_page_list')]/a[@class='current']")).Text);
            //    //var totalPage = Convert.ToInt32(comments.FindElement(By.XPath("div[@class='c_page_box']/div[@class='c_page']/div[contains(@class,'c_page_list')]/a[last()]")).Text);
            //    //var messages = comments.FindElements(By.XPath("div[@class='comment_detail_list']/div"));
            //    //var nextPage = Convert.ToInt32(comments.FindElement(By.XPath("div[@class='c_page_box']/div[@class='c_page']/div[contains(@class,'c_page_list')]/a[@class='current']/following-sibling::a[1]")).Text);

            //    ////sb.Clear();
            //    //Console.WriteLine();
            //    //Console.WriteLine("名称：" + hotelName);
            //    //Console.WriteLine("地址：" + address);
            //    //Console.WriteLine("价格：" + price);
            //    //Console.WriteLine("评分：" + score);
            //    //Console.WriteLine("数量：" + reviewCount);
            //    //Console.WriteLine("页码：" + "当前页（" + currentPage + "）" + "下一页（" + nextPage + "）" + "总页数（" + totalPage + "）" + "每页（" + messages.Count + "）");
            //    //Console.WriteLine();
            //    //Console.WriteLine("===============================================");
            //    //Console.WriteLine();
            //    //Console.WriteLine("点评内容：");

            //    //foreach (var message in messages)
            //    //{
            //    //    Console.WriteLine("帐号：" + message.FindElement(By.XPath("div[contains(@class,'user_info')]/p[@class='name']")).Text);
            //    //    Console.WriteLine("房型：" + message.FindElement(By.XPath("div[@class='comment_main']/p/a")).Text);
            //    //    Console.WriteLine("内容：" + message.FindElement(By.XPath("div[@class='comment_main']/div[@class='comment_txt']/div[1]")).Text.Substring(0, 50) + "....");
            //    //    Console.WriteLine();
            //    //    Console.WriteLine();
            //    //}
            //    Console.WriteLine();

            //};
            //hotelCrawler.Start(hotelUrl, new CrawlSettings());



















            //Console.WriteLine(); 

            //var path = Config.AppSettings<string>("RemoteConfig", "BotConfig.config");
            //Console.Write(path);
            //var local = FileUtility.GetDiskFilePath(path);



            //string sourcePath = "E:/GitHub/ItemDoc/src/Plugin/Sop.FileServer/bin/Debug/pdf2temp/demo1.xlsx";
            //string targetPath = FileUtility.Combine(Pdf2Html.Instance().tempPath, "demo1.pdf"); 

            //var isok1 = Pdf2Html.Instance().ExcelToPDF(sourcePath, targetPath);
            //2秒后开启该线程，然后每隔4s调用一次





            //string sourcePath2 = "D:\\csharp.pdf";
            //string targetPath2 = FileUtility.Combine(Pdf2Html.Instance().TempPath, "demo2.docx");
            //var isok2 = Pdf2Html.Instance().PdfToFile(sourcePath2, targetPath2);


            //string sourcePath3 = "E:/GitHub/ItemDoc/src/Plugin/Sop.FileServer/bin/Debug/pdf2temp/demo3.pptx";
            //string targetPath3 = FileUtility.Combine(Pdf2Html.Instance().tempPath, "demo3.pdf");
            //var isok3 = Pdf2Html.Instance().PowerPointToPDF(sourcePath3, targetPath3);



            //string sourcePath4 = "E:/GitHub/ItemDoc/src/Plugin/Sop.FileServer/bin/Debug/pdf2temp/demo4.pdf";
            //string targetPath4 = FileUtility.Combine(Pdf2Html.Instance().tempPath, "html");
            //var isok4 = Pdf2Html.Instance().PDF2HTML(sourcePath4, targetPath4);

            //string sourcePath5 = "E:/GitHub/ItemDoc/src/Plugin/Sop.FileServer/bin/Debug/pdf2temp/demo4.pdf";
            //string targetPath5 = FileUtility.Combine(Pdf2Html.Instance().tempPath, "thumbnail.jpg");
            //var isok5 = Pdf2Html.Instance().GetPDFToThumbnail(sourcePath5, targetPath5);


            //string sourcePath6 = "E:/GitHub/ItemDoc/src/Plugin/Sop.FileServer/bin/Debug/pdf2temp/demo4.pdf";
            //string targetPath6 = FileUtility.Combine(Pdf2Html.Instance().tempPath, "preview.html");
            //var isok6 = Pdf2Html.Instance().PDFToHTML(sourcePath6, targetPath6);


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
