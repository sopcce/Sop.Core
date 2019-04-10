using System;
using System.Net;
using OpenQA.Selenium;

namespace Sop.FileServer.Events
{
    /// <summary>
    /// 爬虫完成事件
    /// </summary>
    public class OnCompletedEventArgs
    {
        public String URL { get; private set; }// 爬虫URL地址
        public int ThreadId { get; private set; }// 任务线程ID
        public string PageSource { get; private set; }// 页面源代码

        /// <summary>
        /// 页码状态
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        public IWebDriver WebDriver { get; private set; }
        public long Milliseconds { get; private set; }// 爬虫请求执行事件
        public OnCompletedEventArgs(string url, int threadId, long milliseconds, string pageSource, IWebDriver driver)
        {
            this.URL = url;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
            this.PageSource = pageSource;
            this.WebDriver = driver;
        }
        public OnCompletedEventArgs(string url, int threadId, long milliseconds, HttpStatusCode statusCode)
        {
            this.URL = url;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
            this.StatusCode = statusCode;
        }
        public OnCompletedEventArgs(string url, int threadId, long milliseconds, string pageSource)
        {
            this.URL = url;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
            this.PageSource = pageSource;
        }

    }
}
