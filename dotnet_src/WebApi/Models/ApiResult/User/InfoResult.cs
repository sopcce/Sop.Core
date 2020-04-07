namespace WebApi.Models.ApiResult
{
    public class InfoResult
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Userid { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string[] Roles { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; set; }
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
        public string Introduction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get; set; } = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif";

    }
}
