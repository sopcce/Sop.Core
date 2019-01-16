namespace ItemDoc.ConsoleBot.Events
{
    /// <summary>
    /// 爬虫启动事件
    /// </summary>
    public class OnStartEventArgs
    {
        public string URL { get; set; }// 爬虫URL地址

        public OnStartEventArgs(string url)
        {
            this.URL = url;
        }
    }
}
