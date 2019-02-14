using System;
using OpenQA.Selenium;

namespace ItemDoc.ConsoleBot.Models
{
    /// <summary>
    /// The crawl settings.
    /// </summary>
    [Serializable]
    public class CrawlSettings
    {


        public CrawlSettings()
        {
            this.RequestOption = new RequestOptions();
            this.ProxyOption = new ProxyOptions();
        }
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CrawlerType CrawlerType { get; set; } = CrawlerType.PhantomJs;
        /// <summary>
        /// 
        /// </summary>
    
        /// <summary>
        /// 延时时间
        /// </summary>
        public bool AutoSpeedLimit { get; set; } = false;


        public Action<IWebDriver> Action { get; set; }

        public Func<IWebDriver, bool> Condition { get; set; }



        public string ScriptCode { get; set; }

        public object[] ScriptArgs { get; set; }


        public int Timeout { get; set; }

        public string Path { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public RequestOptions RequestOption { get; set; }
        /// <summary>
        /// 代理设置
        /// </summary>
        public ProxyOptions ProxyOption { get; set; }
    }

    public enum CrawlerType
    {
        /// <summary>
        /// 
        /// </summary>
        PhantomJs = 1,
        /// <summary>
        /// 
        /// </summary>
        HttpWebRequest = 2
    }
}