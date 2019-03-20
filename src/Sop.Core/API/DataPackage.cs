using System.Net;

namespace Sop.Core.API
{

    /// <summary>
    /// API数据包
    /// </summary>
    public class DataPackage
    {
        /// <summary>
        /// 类型
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public object Paging { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }


        /// <summary>
        /// 数据包
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        public DataPackage(HttpStatusCode code, object data = null, string msg = null)
        {
            this.Code = code;
            this.Data = data;
            this.Msg = msg;
        }

        /// <summary>
        /// 全文检索数据包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paging"></param>
        /// <param name="msg"></param>
        public DataPackage(object data = null, object paging = null, string msg = null)
        {
            this.Data = data;
            this.Paging = paging;
            this.Msg = msg;
        }
    }



}
