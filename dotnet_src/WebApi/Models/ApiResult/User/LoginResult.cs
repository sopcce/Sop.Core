using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ApiResult
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Userid { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string Username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

    }
}
