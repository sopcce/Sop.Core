using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ItemDoc.Framework.Utilities;
using ItemDoc.Framework.Utility;
using ItemDoc.Framework.Validation;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.ViewModel
{
  /// <summary>
  /// 用户注册
  /// </summary>
  public class UsersRegisterViewModel
  {
    /// <summary>
    /// 用户登录名
    /// </summary>
    [Required(ErrorMessage = "登录名不能为空!")]
    [Remote("ValidateLoginName", "User", ErrorMessage = "登录名重复，请重新输入")]
    public string UserLoginName { get; set; }
    /// <summary>
    /// 用户登录密码
    /// </summary>
    [Required(ErrorMessage = "登录密码不能为空!")]
    //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "两次密码输入不一致")]
    public string UserLoginPassWord { get; set; }
    /// <summary>
    /// 用户昵称
    /// </summary>
    public string UserLoginNickName { get; set; }
    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "验证码不能为空!")]
    [Remote("ValidateCaptchaCode", "User", ErrorMessage = "验证码不正确")]
    public string UserLoginCaptchaCode { get; set; }



    public UsersLoginInfo AsUserLoginInfo()
    {
      var isok = Captcha.ValidateCheckCode(UserLoginCaptchaCode);

      //再一次验证验证码是否正确
      UsersLoginInfo info = new UsersLoginInfo();
      info.UserId = Guid.NewGuid().ToString();
    
      info.UserName = UserLoginName;
      info.NickName = UserLoginNickName;
      info.AccountEmail = "";
      info.AccountMobile = "";
      info.LastActivityIp = WebUtility.GetIp();
      info.CreatedIp = WebUtility.GetIp();
      info.PassWord = UserLoginPassWord;
      info.DateCreated = DateTime.Now;
      info.LastActivityTime = DateTime.Now;
      return info;
    }


  }
}
