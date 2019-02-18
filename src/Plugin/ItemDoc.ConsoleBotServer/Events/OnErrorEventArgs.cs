using System;

namespace ItemDoc.Core.WebCrawler.Events
{
    public class OnErrorEventArgs
    {
        public string Url { get; set; }

        public Exception Exception { get; set; }

        public OnErrorEventArgs(String url,Exception exception) {
            this.Url = url;
            this.Exception = exception;
        }
    }
}
