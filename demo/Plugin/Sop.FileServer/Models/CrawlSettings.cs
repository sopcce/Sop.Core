using OpenQA.Selenium;
using System;

namespace Sop.FileServer.Models
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
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public Browsers Browsers { get; set; } = Browsers.HttpWebRequest;
       

       


        public Action<IWebDriver> Action { get; set; }

        public Func<IWebDriver, bool> Condition { get; set; }



        public string ScriptCode { get; set; }

        public object[] ScriptArgs { get; set; }


        public int Timeout { get; set; }

        public string Path { get; set; } = null;



        /// <summary>
        /// 请求参数
        /// </summary>
        public RequestOptions RequestOption { get; set; }
        /// <summary>
        /// 代理设置
        /// </summary>
        public ProxyOptions ProxyOption { get; set; }
    }
 
}