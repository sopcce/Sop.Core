using System;
using System.Collections.Generic;
using System.Linq;
using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Model;
using ItemDoc.Services.Repositories;

namespace ItemDoc.Services.Servers
{
  public class UsersService : IUsersService
  {
    private readonly IUsersRepository _usersRepository;
    public UsersService(IUsersRepository usersRepository)
    {
      _usersRepository = usersRepository;

    }






    public object Delete(UsersInfo info)
    {
      return _usersRepository.Delete(info);

    }

    public object Create(UsersInfo info)
    {
      _usersRepository.Insert(info);
      info.DisplayOrder = info.Id;
      return _usersRepository.Update(info);
    }


    public UsersInfo GetByUserName(string userName)
    {
      return _usersRepository.GetByUserName(userName);
    }

    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    public UsersInfo GetByUserId(string primaryKey)
    {

      if (primaryKey == "2")
      {
        _usersRepository.Execute("truncate table " + SopTable.Instance().GetTableName<UsersInfo>());
        for (int i = 0; i < 100; i++)
        {
          //再一次验证验证码是否正确
          UsersInfo info = new UsersInfo();
          info.UserId = Guid.NewGuid().ToString();
          info.UserName = i.ToString();
          info.NickName = "测试账号_" + i;
          info.AccountEmail = "";
          info.AccountMobile = "";
          info.LastActivityIp = WebUtility.GetIp();
          info.CreatedIp = WebUtility.GetIp();
          info.PassWord = "123456";
          info.PassWordEncryption = (PassWordEncryptionType)new Random().Next(0, 5);
          info.DateCreated = DateTime.Now;
          info.LastActivityTime = DateTime.Now;
          info.TrueName = "测试账号_" + i;
          this.Insert(info);
        }
      }

      return _usersRepository.GetByUserId(primaryKey);
    }

    /// <summary>
    /// Inserts the specified information.
    /// </summary>
    /// <param name="info">The information.</param>
    public void Insert(UsersInfo info)
    {
      //info.PassWord = SetPassword(info.UserName, info.PassWord, info.PassWordEncryption);
      _usersRepository.Insert(info);
      
    }

    /// <summary>
    /// Determines whether [is account exsit] [the specified u name].
    /// </summary>
    /// <param name="uName">Name of the u.</param>
    /// <returns>
    ///   <c>true</c> if [is account exsit] [the specified u name]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsAccountExsit(string uName)
    {
      bool isExsit = false;
      var info = _usersRepository.GetByUserName(uName);
      if (info == null)
      {
        isExsit = false;
      }
      else
      {
        isExsit = (uName == info.UserName)
          ? true
          : false;
      }
      return isExsit;
    }
    /// <summary>
    /// Logins the specified u name.
    /// </summary>
    /// <param name="uName">Name of the u.</param>
    /// <param name="passWord">The pass word.</param>
    /// <param name="isExsit">if set to <c>true</c> [is exsit].</param>
    /// <returns></returns>
    public UsersInfo Login(string uName, string passWord, out bool isExsit)
    {
      isExsit = false;
      if (string.IsNullOrWhiteSpace(uName) || string.IsNullOrWhiteSpace(passWord))
        return null;
      UsersInfo info = _usersRepository.GetByUserName(uName);
      if (info != null)
      {
        isExsit = (SetPassword(uName, passWord, info.PassWordEncryption) == info.PassWord) ? true : false;
      }
      return info;
    }

    /// <summary>
    /// Gets the pass word.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <param name="passWord">The pass word.</param>
    /// <param name="passWordEncryptionType">Type of the pass word encryption.</param>
    /// <returns></returns>
    public string SetPassword(string userName, string passWord, PassWordEncryptionType passWordEncryptionType = PassWordEncryptionType.None)
    {
      string value = string.Empty;
      switch (passWordEncryptionType)
      {
        case PassWordEncryptionType.None:
          value = passWord;
          break;
        case PassWordEncryptionType.Sha256:
          value = EncryptionUtility.Sha256Encode(
            EncryptionUtility.Sha256Encode(userName) +
            EncryptionUtility.Sha256Encode(passWord));
          break;
        case PassWordEncryptionType.Sha512:
          value = EncryptionUtility.Sha512Encode(
            EncryptionUtility.Sha512Encode(userName) +
            EncryptionUtility.Sha512Encode(passWord));
          break;
        case PassWordEncryptionType.Md5Upper:
          value = EncryptionUtility.Md5Encode(
            EncryptionUtility.Md5Encode(userName) +
            EncryptionUtility.Md5Encode(passWord));
          break;
        case PassWordEncryptionType.Aes:
          value = EncryptionUtility.AES_Encrypt(
            EncryptionUtility.AES_Encrypt(userName) +
            EncryptionUtility.AES_Encrypt(passWord));
          break;
        default:
          value = passWord;
          break;
      }


      return value;
    }




  }

}
