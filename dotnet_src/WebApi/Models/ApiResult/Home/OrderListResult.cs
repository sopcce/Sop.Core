using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ApiResult
{
    public class OrderListResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string order_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float price { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string status { get; set; }

    }
}
