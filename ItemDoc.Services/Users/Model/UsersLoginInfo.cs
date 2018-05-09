using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 用户登录实体
  /// </summary>
  [SopTableName("Item_UsersLogin")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class UsersLoginInfo
  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public UsersLoginInfo()
    {
      DateCreated = DateTime.Now;
      LastActivityTime = DateTime.Now;

    }
    public int Id { get; set; }
    /// <summary>
    /// 用户ID  
    /// </summary>
    public string UserId { get; set; }

    public string UserName { get; set; }
    /// <summary>
    /// 邮箱账号
    /// </summary>
    public string AccountEmail { get; set; }
    /// <summary>
    /// 手机账号
    /// </summary>
    public string AccountMobile { get; set; }
    /// <summary>
    /// 邮箱是否激活
    /// </summary>
    public bool IsVerifiedEmail { get; set; }
    /// <summary>
    /// 手机号是否激活
    /// </summary>
    public bool IsVerifiedMobile { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string PassWord { get; set; }
    /// <summary>
    /// 密码加密方式,默认0  不加密
    /// </summary>
    public PassWordEncryptionType PassWordEncryption { get; set; }
    /// <summary>
    /// 真实姓名
    /// </summary>
    public string TrueName { get; set; }
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }
    /// <summary>
    /// 账号状态   封禁、、、、
    /// </summary>
    public UsersStatus Status { get; set; }
    /// <summary>
    /// 状态备注。eg:封禁原因
    /// </summary>
    public string StatusNotes { get; set; }

    /// <summary>
    /// 创建IP
    /// </summary>
    public string CreatedIp { get; set; }

    /// <summary>
    /// 最后活跃Ip
    /// </summary>
    public string LastActivityIp { get; set; }

    /// <summary>
    /// 最后活跃时间
    /// </summary>
    public DateTime LastActivityTime { get; set; }

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    /// <value>
    /// The display order.
    /// </value>
    public int DisplayOrder { get; set; }
    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;
  }




  public enum PassWordEncryptionType
  {
    /// <summary>
    /// The none
    /// </summary>
    None = 0,
    /// <summary>
    /// The MD5 upper
    /// </summary>
    Md5Upper = 1,
    /// <summary>
    /// The sha256
    /// </summary>
    Sha256 = 2,
    /// <summary>
    /// The sha512
    /// </summary>
    Sha512 = 3,
    /// <summary>
    /// The aes
    /// </summary>
    Aes = 4,

  }
  public enum UsersStatus
  {

    /// <summary>
    /// 未激活
    /// </summary>
    Nonactivated = 1,

    /// <summary>
    /// 已激活
    /// </summary>
    Activated = 2,

    /// <summary>
    /// 已封禁
    /// </summary>
    Banned = 3
  }
}
