using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Auth.Model;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.ViewModel
{
  /// <summary>
  /// 用户登录业务实体类
  /// </summary>
  public class UsersLoginViewModel
  {
    /// <summary>
    /// 用户登录名
    /// </summary>
    [Required(ErrorMessage = "用户登录名不能为空!")]
    //[Remote("ValidateLoginNameIsExsit", "User", ErrorMessage = "用户不存在")]
    public string UserName { get; set; }
    /// <summary>
    /// 用户登录密码
    /// </summary>
    [Required(ErrorMessage = "用户登录密码不能为空!")]
    public string PassWord { get; set; }
    /// <summary>
    /// 是否记住密码
    /// </summary>
    public bool RememberMe { get; set; }
    /// <summary>
    /// 用户注册验证码
    /// </summary>
    [Remote("ValidateCaptchaCode", "User", ErrorMessage = "验证码不正确")]
    [Required(ErrorMessage = "验证码不能为空!")]
    public string CaptchaCode { get; set; }


  }


}
