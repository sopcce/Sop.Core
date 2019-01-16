using System;
using OpenQA.Selenium;

namespace ItemDoc.Core.WebCrawler
{
    /// <summary>
    /// 操作
    /// </summary>
    public class Operation
    {
        public int Timeout { get; set; }

        public Action<IWebDriver> Action { get; set; }

        public Func<IWebDriver, bool> Condition { get; set; }

    }
}
