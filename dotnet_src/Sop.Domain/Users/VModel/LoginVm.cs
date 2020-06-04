namespace Sop.Domain.VModel
{
    /// <summary>
    /// 用户登录业务实体类
    /// </summary>
    public class LoginVm
    {
        /// <summary>
        /// 用户登录名
        /// </summary>  
        public string UserName { get; set; }
        /// <summary>
        /// 用户登录密码
        /// </summary> 
        public string PassWord { get; set; }
        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool RememberMe { get; set; }
        /// <summary>
        /// 用户注册验证码
        /// </summary>  
        public string CaptchaCode { get; set; }


    }
}