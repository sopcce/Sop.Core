using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ItemDoc.Framework.Utilities;
using ItemDoc.Framework.Utility;
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
    [Remote("ValidateLoginNameIsExsit", "User", ErrorMessage = "用户不存在")]
    public string UserLoginName { get; set; }
    /// <summary>
    /// 用户登录密码
    /// </summary>
    [Required(ErrorMessage = "用户登录密码不能为空!")]
    public string UserLoginPassWord { get; set; }
    /// <summary>
    /// 是否记住密码
    /// </summary>
    public bool RememberPassword { get; set; }
    /// <summary>
    /// 用户注册验证码
    /// </summary>
    [Remote("ValidateCaptchaCode", "User", ErrorMessage = "验证码不正确")]
    [Required(ErrorMessage = "验证码不能为空!")]
    public string UserLoginCaptchaCode { get; set; }











    public UsersInfo AsUserLoginInfo()
    {
      var info = new UsersInfo();
      info.UserId = Guid.NewGuid().ToString();
      info.UserName = UserLoginName;
      info.NickName = "";
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
