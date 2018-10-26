using ItemDoc.Framework.Utility;
using ItemDoc.Framework.Validation;
using ItemDoc.Services.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ItemDoc.Services.ViewModel
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class UsersRegisterViewModel
    {


        [Required]
        [StringLength(100, ErrorMessage = "{0} 至少大于{2}位数", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码与确认密码不一致")]
        public string ConfirmPassword { get; set; }


        /// <summary>
        /// 用户登录名
        /// </summary>
        [Required(ErrorMessage = "登录名不能为空!")]
        [Remote("ValidateLoginName", "User", ErrorMessage = "{0}重复，请重新输入")]
        public string UserName { get; set; }


        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空!")]
        [Remote("ValidateCaptchaCode", "User", ErrorMessage = "验证码不正确")]
        public string CaptchaCode { get; set; }
        
    }
}
