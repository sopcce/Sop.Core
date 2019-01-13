using System;
using System.Threading.Tasks;
using ItemDoc.ConsoleBot.WebCrawler.Events;
using ItemDoc.Core.WebCrawler;
using ItemDoc.Core.WebCrawler.Events;
//using OpenQA.Selenium;
//using OpenQA.Selenium.PhantomJS;

namespace ItemDoc.ConsoleBot.WebCrawler
{
    public interface ICrawler
    {
        /// <summary>
        /// 爬虫启动事件
        /// </summary>
        event EventHandler<OnStartEventArgs> OnStart;
        /// <summary>
        /// 爬虫完成事件
        /// </summary>
        event EventHandler<OnCompletedEventArgs> OnCompleted;
        /// <summary>
        /// 爬虫出错事件
        /// </summary>
        event EventHandler<OnErrorEventArgs> OnError;
        /// <summary>
        /// //启动爬虫进程
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="script"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task Start(Uri uri, Script script, Operation operation); 

    }
}






