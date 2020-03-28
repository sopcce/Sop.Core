using System;
using System.Collections.Generic;

namespace Sop.Core.Authorize
{
    public class JwtTokenVm
    { 
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; } 
        /// <summary>
        /// 角色
        /// </summary>
        public string[] Role { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Expires { get;  set; }
    }


}
