using System;
using System.Collections.Generic;
using System.Net;

namespace ItemDoc.Core.WebCrawler.Models
{
    /// <summary>
    /// The crawl settings.
    /// </summary>
    [Serializable]
    public class CrawlSettings
    {


        public CrawlSettings()
        {
            this.Options = new RequestOptions();
       
        }
        /// <summary>
        /// 
        /// </summary>
        public CrawlerType CrawlerType { get; set; } = CrawlerType.PhantomJS;
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 延时时间
        /// </summary>
        public bool AutoSpeedLimit { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public RequestOptions Options { get; set; }
        /// <summary>
        /// 代理
        /// </summary>
        public string Proxy { get; set; }
    }

    public enum CrawlerType
    {
        /// <summary>
        /// 
        /// </summary>
        PhantomJS = 1,
        /// <summary>
        /// 
        /// </summary>
        HttpWebRequest = 2
    }
}