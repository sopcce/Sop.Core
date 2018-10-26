using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Model;
using System;
using System.Linq;

namespace ItemDoc.Services.Servers
{
    public class UsersService
    {


        public IRepository<UsersInfo> _usersRepository { get; set; }
        public IRepository<UsersExternalLoginInfo> _usersExternalLoginRepository { get; set; }

        #region _usersRepository

        public void Update(UsersInfo info)
        {
            _usersRepository.Update(info);
        }

        public UsersInfo Get(object id)
        {
            return _usersRepository.Get(id);

        }

        public void Delete(UsersInfo info)
        {
            _usersRepository.Delete(info);

        }

        public void Create(UsersInfo info)
        {
            _usersRepository.Create(info);

            //_usersRepository.Update(info);
        }


        public UsersInfo GetByUserName(string userName)
        {
            return _usersRepository.Fetch(n => n.UserName == userName, null)?.SingleOrDefault();
        }

        /// <summary>
        /// Gets the by userid.
        /// </summary>
        /// <param name="userId">The primary key.</param>
        /// <returns></returns>
        public UsersInfo GetByUserId(string userId)
        {
            return _usersRepository.Fetch(n => n.UserName == userId, null).SingleOrDefault();
        }

        /// <summary>
        /// Inserts the specified information.
        /// </summary>
        /// <param name="info">The information.</param>
        public void Insert(UsersInfo info)
        {
            //info.PassWord = SetPassword(info.UserName, info.PassWord, info.PassWordEncryption);
            _usersRepository.Create(info);

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
            var info = GetByUserName(uName);
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
            UsersInfo info = GetByUserName(uName);
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


        #endregion


        #region _usersExternalLoginRepository


        public void Create(UsersExternalLoginInfo info)
        {
            _usersExternalLoginRepository.Create(info);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UsersExternalLoginInfo GetExternalLoginInfo(object id)
        {
            return _usersExternalLoginRepository.Get(id);
        }

      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        public UsersExternalLoginInfo GetExternalLoginInfoByProviderKey(string providerKey)
        {
            return _usersExternalLoginRepository.Fetch(n => n.ProviderKey == providerKey, null)?.SingleOrDefault();
        }
        #endregion
    }

}
