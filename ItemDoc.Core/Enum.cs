using System.ComponentModel.DataAnnotations;

namespace ItemDoc.Core
{
    #region TorF 正确&错误\启用&禁用
    /// <summary>
    /// TorF正确&错误\启用&禁用
    /// </summary>
    public enum TorF
    {
        /// <summary>
        /// 错误(False)否 禁用
        /// </summary>
        F = 0,
        /// <summary>
        /// 正确(True)是 启用
        /// </summary>
        T = 1
    }
    #endregion


    #region 用户相关的枚举

    #region 验证码使用场景 VerifyScenarios
    /// <summary>
    /// 验证码使用场景
    /// </summary>
    public enum VerifyScenarios
    {
        /// <summary>
        /// 登录时
        /// </summary>
        Login = 1,

        /// <summary>
        /// 提交内容时
        /// </summary>
        Post = 2,

        /// <summary>
        /// 注册时
        /// </summary>
        Register = 3
    }
  #endregion



  #region 用户账号状态(封禁、正常等) UsersStatus
  /// <summary>
  /// 用户账号状态
  /// </summary>

  #endregion

  #region 男或女 BoyorGril
  /// <summary>
  /// 男或女
  /// </summary>
  public enum BoyorGril
    {
        /// <summary>
        /// 女
        /// </summary>
        Gril = 0,
        /// <summary>
        /// 男
        /// </summary>
        Boy = 1
    }


    #endregion

    /// <summary>
    /// 用户账号登录账号类型
    /// </summary>
    public enum UsersLoginType
    {
        /// <summary>
        /// AccountEmail
        /// </summary>
        AccountEmail,
        /// <summary>
        /// AccountName
        /// </summary>
        AccountName,
        /// <summary>
        /// AccountMobile
        /// </summary>
        AccountMobile,
        /// <summary>
        /// AccountIDcard
        /// </summary>
        AccountIDcard
    }


    #endregion





    /// <summary>
    /// 访问计数类型
    /// </summary>
    public enum ViewCountTypeStatus
    {
        /// <summary>
        /// 每次访问记录
        /// </summary>
        [Display(Name = "每次访问")]
        EveryView = 0,
        /// <summary>
        /// 每天访问记录1次
        /// </summary>、
        [Display(Name = "每天访问")]
        EveryDayView = 1,

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "访问一次")]
        EveryOneView = 2,
    }
}
