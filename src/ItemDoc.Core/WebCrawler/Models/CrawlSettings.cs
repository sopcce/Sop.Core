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
            this.AutoSpeedLimit = false;
            this.Options = new RequestOptions();
        }

        /// <summary>
        /// 延时时间
        /// </summary>
        public bool AutoSpeedLimit { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public RequestOptions Options { get; set; }

    }

}