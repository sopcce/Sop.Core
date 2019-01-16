using ItemDoc.ConsoleBot.Events;
using ItemDoc.Core.WebCrawler;
using ItemDoc.Core.WebCrawler.Events;
using System;
using System.Threading.Tasks;
using ItemDoc.ConsoleBot.Models;


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
        /// <param name="url"></param>
        /// <param name="script"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task Start(string url, Script script, Operation operation);

        Task<string> Start(string url, ProxyOptions proxyOptions = null);

    }
}






