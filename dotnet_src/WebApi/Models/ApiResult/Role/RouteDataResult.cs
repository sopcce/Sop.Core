using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ApiResult
{
    public class RouteChildren
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Component { get; set; }
    }

    public class RouteDataResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Hidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RouteChildren> Children { get; set; }
    }
}
