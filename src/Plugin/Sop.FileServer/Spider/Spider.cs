using System;
using System.Collections.Generic;
using System.Linq;
using Sop.FileServer.Spider.Core;
using Sop.FileServer.Spider.Core.Pipeline;
using Sop.FileServer.Spider.Core.Processor;
using QA = OpenQA.Selenium;
using UI = OpenQA.Selenium.Support.UI;


namespace Sop.FileServer.Spider
{
    public class Spider
    {
        #region private

        private readonly Dictionary<string, Dictionary<string, object>> _headers =
           new Dictionary<string, Dictionary<string, object>>();
        public readonly List<IRequestBuilder> RequestBuilders = new List<IRequestBuilder>();
        private readonly List<Request> _requests = new List<Request>();

        /// <summary>
        /// All pipelines for spider.
        /// </summary>
        public readonly List<IPipeline> Pipelines = new List<IPipeline>();

        /// <summary>
        /// Storage all processors for spider.
        /// </summary>
        public readonly List<IPageProcessor> PageProcessors = new List<IPageProcessor>();
        private readonly Dictionary<string, IEnumerable<object>> _script =
            new Dictionary<string, IEnumerable<object>>();

        private readonly List<Operation> _operation = new List<Operation>();
        #endregion


        /// <summary>
        /// 报头
        /// </summary>
        /// <param name="host"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual Spider AddHeaders(string host, Dictionary<string, object> headers)
        {
            var key = host.ToLower();
            if (_headers.ContainsKey(key))
            {
                foreach (var kv in headers)
                {
                    _headers[key].Add(kv.Key, kv.Value);
                }
            }
            else
            {
                _headers.Add(key, headers);
            }
            return this;
        }

        /// <summary>
        /// 请求 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public virtual Spider AddRequestBuilder(IRequestBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            RequestBuilders.Add(builder);
            return this;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="extras"></param>
        /// <returns></returns>
        public virtual Spider AddRequest(string url, Dictionary<string, dynamic> extras)
        {
            return AddRequests(new Request(url, extras));
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public virtual Spider AddRequests(params string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException(nameof(urls));
            }

            return AddRequests(urls.AsEnumerable());
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public virtual Spider AddRequests(IEnumerable<string> urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException(nameof(urls));
            }

            return AddRequests(urls.Select(u => new Request(u, null)));
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual Spider AddRequest(Request request)
        {
            return AddRequests(request);
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public virtual Spider AddRequests(params Request[] requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            return AddRequests(requests.AsEnumerable());
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public virtual Spider AddRequests(IEnumerable<Request> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            foreach (var request in requests)
            {
                _requests.Add(request);
            }

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public virtual Spider AddExecuteScript(string script)
        {

            if (script == null)
            {
                throw new ArgumentNullException(nameof(script));
            }
            _script.Add(script, null);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual Spider AddExecuteScript(string script, params object[] args)
        {

            if (script == null)
            {
                throw new ArgumentNullException(nameof(script));
            }
            _script.Add(script, args.AsEnumerable());
            return this;
        }
        public virtual Spider AddExecuteOperation(Operation operation)
        {
            if (operation != null)
                _operation.Add(operation);
            return this;
        }



        /// <summary>
        /// 页面解析
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public virtual Spider AddPageProcessor(IPageProcessor processor)
        {
            return AddPageProcessors(processor);
        }

        /// <summary>
        /// 页面解析
        /// </summary>
        /// <param name="processors"></param>
        /// <returns></returns>
        public virtual Spider AddPageProcessors(params IPageProcessor[] processors)
        {
            if (processors == null)
            {
                throw new ArgumentNullException(nameof(processors));
            }

            return AddPageProcessors(processors.AsEnumerable());
        }

        /// <summary>
        /// 页面解析
        /// </summary>
        /// <param name="processors"></param>
        /// <returns></returns>
        public virtual Spider AddPageProcessors(IEnumerable<IPageProcessor> processors)
        {
            if (processors != null)
            {

                foreach (var processor in processors)
                {
                    if (processor != null)
                    {
                        PageProcessors.Add(processor);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// 页面解析
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public virtual Spider AddPipeline(IPipeline pipeline)
        {
            return AddPipelines(pipeline);
        }

        /// <summary>
        /// Set pipelines for Spider
        /// </summary>
        /// <param name="pipelines">数据管道</param>
        /// <returns>爬虫</returns>
        public virtual Spider AddPipelines(params IPipeline[] pipelines)
        {
            if (pipelines == null)
            {
                throw new ArgumentNullException(nameof(pipelines));
            }

            return AddPipelines(pipelines.AsEnumerable());
        }

        /// <summary>
        /// Set pipelines for Spider
        /// </summary>
        /// <param name="pipelines">数据管道</param>
        /// <returns>爬虫</returns>
        public virtual Spider AddPipelines(IEnumerable<IPipeline> pipelines)
        {
            if (pipelines == null)
            {
                throw new ArgumentNullException(nameof(pipelines));
            }
            foreach (var pipeline in pipelines)
            {
                if (pipeline != null)
                {
                    Pipelines.Add(pipeline);
                }
            }

            return this;
        }














    }
}