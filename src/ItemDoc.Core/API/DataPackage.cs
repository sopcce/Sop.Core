using System.Net;

namespace ItemDoc.Core.API
{

    /// <summary>
    /// API数据包
    /// </summary>
    public class DataPackage
    {
        /// <summary>
        /// 类型
        /// </summary>
        public HttpStatusCode code { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public object paging { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string description { get; set; }

        public DataPackage()
        {
        }

        /// <summary>
        /// 数据包
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="desc"></param>
        public DataPackage(HttpStatusCode code, object data = null, string desc = null)
        {
            this.code = code;
            this.data = data;
            this.description = desc;
        }

        /// <summary>
        /// 全文检索数据包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paging"></param>
        /// <param name="desc"></param>
        public DataPackage(object data = null, object paging = null, string desc = null)
        {
            this.data = data;
            this.paging = paging;
            this.description = desc;
        }
    }



}
