

using Sop.Data;

namespace Sop.Core.Api
{
    public class ApiResult<T>
    {
        /// <summary>
        /// 响应CODE
        /// </summary>
        public Code Code { get; set; } = Code.OK;
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Time { get; set; } = 0;

    }

    public class ApiPageResult<T> : ApiResult<T>
    {
        public IPageList<T> Page { get; set; }
          
    }
}
