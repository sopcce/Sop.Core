using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Core.WebCrawler.Models
{
    public class RequestOptions
    {
        /// <summary>
        /// 请求方式，GET或POST
        /// </summary>
        public string Method { get; set; } = "GET";
        /// <summary>
        ///   获取或设置 <see cref="M:System.Net.HttpWebRequest.GetResponse" /> 和 <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> 方法的超时值（以毫秒为单位）。
        /// </summary>
        /// <returns>
        ///   请求超时前等待的毫秒数。
        ///    默认值是 100,000 毫秒（100 秒）。
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   指定的值是小于零，且不是 <see cref="F:System.Threading.Timeout.Infinite" />。
        /// </exception>
        public int Timeout { get; set; } = 100 * 1000;
        /// <summary>
        /// 上一级历史记录链接
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        ///   //设置User-Agent，伪装成Google Chrome浏览器
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
        /// <summary>
        /// URL
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// 启用长连接
        /// </summary>
        public bool KeepAlive { get; set; } = true;
        /// <summary>
        /// 禁止自动跳转
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = false;
        /// <summary>
        /// 定义最大连接数
        /// </summary>
        public int ConnectionLimit { get; set; } = int.MaxValue;
        /// <summary>
        /// 请求次数
        /// </summary>
        public int RequestNum { get; set; } = 3;
        /// <summary>
        ///   获取或设置HTTP 标头的值。
        /// </summary> 
        public string Accept { get; set; } = "*/*";
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";

        /// <summary>
        /// 头部信息
        /// </summary>
        public WebHeaderCollection WebHeader { get; set; } = new WebHeaderCollection();

        /// <summary>
        /// 定义请求Cookie字符串
        /// </summary>
        public string RequestCookies { get; set; }
        /// <summary>
        /// //定义Cookie容器
        /// </summary>
        public CookieContainer CookiesContainer { get; set; }
        /// <summary>
        /// 异步参数数据
        /// </summary>
        public string XhrParams { get; set; }
        /// <summary>
        /// 获取或设置请求的媒体类型
        /// </summary>
        public string MediaType { get; set; } = "text/html";
    }
}
