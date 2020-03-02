using AutoMapper;
using Sop.Framework.Environment;
using Sop.Framework.Utility;
using Sop.Services.Auth.Model;
using Sop.Services.Model;
using Sop.Services.Servers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sop.Services.Auth.Identity
{
    public class UserStore :
          IUserStore<User, long>,
          IUserPasswordStore<User, long>,
          IUserLockoutStore<User, long>,
          IUserTwoFactorStore<User, long>,
          IUserRoleStore<User, long>,
          IUserClaimStore<User, long>,
          IUserLoginStore<User, long>
    {
        private readonly UsersService _usersService;

        public UserStore(string folderStorage)
        {
            _usersService = DiContainer.Resolve<UsersService>();
        }

        #region IUserStore  

        public Task CreateAsync(User user)
        {
            var info = user.MapTo<UsersInfo>();
            //TODO:临时这样，等待后期优化，优化此时体方法，可以使用AutoMap
            _usersService.Create(info);
            return Task.FromResult(user);
        }

        public Task DeleteAsync(User user)
        {
            var info = new UsersInfo()
            {
                Id = user.Id
            };
            _usersService?.Delete(info);
            return Task.FromResult(user);
        }

        public Task<User> FindByIdAsync(long userId)
        {

            var userDb = _usersService?.Get(userId);
            var user = userDb?.MapTo<User>();
            //user.Id = userDb?.UserId;

            return Task.FromResult(user);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var userDb = _usersService?.GetByUserName(userName);
            var user = userDb?.MapTo<User>();
            if (user != null)
            {
                user.Id = userDb.Id;
                return Task.FromResult(user);
            }
            return Task.FromResult<User>(null);
        }

        public Task UpdateAsync(User user)
        {
            user.StatusNotes = "asdasd";
            var usersInfo = _usersService.Get(user.Id);
            user.MapTo(usersInfo);


            _usersService.Update(usersInfo);

            return Task.FromResult(0);
        }

        #endregion

        #region PASSWORD STORE

        public Task<string> GetPasswordHashAsync(User user)
        {
            var passWord = _usersService?.Get(user.Id)?.PassWord;

            return Task.FromResult(passWord);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            var passWord = _usersService?.Get(user.Id)?.PassWord;
            bool isok = user.PassWord == passWord ? true : false;
            return Task.FromResult(isok);
        }

        public Task SetPasswordHashAsync(User user, string password)
        {
          
            // TODO 对密码加密方式，这里使用默认，不做处理。
            user.PassWord = password;
            return Task.FromResult(0);
        }

        #endregion

        #region LOCKOUT STORE

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            var count = _usersService.GetByUserId(user.Id.ToString())?.AccessFailedCount ?? 0;

            return Task.FromResult(count);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            //TODO 记得修改
            //if (!user.LockoutEndDateUtc.Value)
            //{
            //    throw new InvalidOperationException("LockoutEndDate has no value.");
            //}

            return Task.FromResult(new DateTimeOffset(user.LockoutEndDateUtc.Value));
        }
        /// <summary>
        /// 增量访问失败计数  
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region TWO FACTOR

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region USERS - ROLES STORE

        public Task AddToRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("role");
            }


            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<string>>(user.Roles);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("role");
            }
            //user.Roles.tol.Contains(roleName, StringComparer.InvariantCultureIgnoreCase)
            var isok = true;
            return Task.FromResult(isok);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //user.Roles.Remove(roleName);

            return Task.FromResult(0);
        }

        #endregion

        #region USERS - CLAIM STORE

        public Task AddClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            if (user.Claims != null && user.Claims.Any(f => f.Value == claim.Value))
            {
                user.Claims.Add(new UserClaim(claim));
            }

            return Task.FromResult(0);
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<System.Security.Claims.Claim>>(user.Claims.Select(clm => new System.Security.Claims.Claim(clm.Type, clm.Value)).ToList());
        }

        public Task RemoveClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Claims.Remove(new UserClaim(claim));

            return Task.FromResult(0);
        }

        #endregion

        #region User - Logins

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("user");
            }

            if (!user.Logins.Any(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey))
            {
                user.Logins.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                _usersService.Create(new UsersExternalLoginInfo()
                {
                    UserId = user.Id,
                    DateCreated = DateTime.Now,
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey
                });
            }

            return Task.FromResult(true);
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            //TODO ADD
            var loginId = GetLoginId(login);

            var externalInfo = _usersService.GetExternalLoginInfoByProviderKey(login.ProviderKey);
            if (externalInfo == null)
            {
                return Task.FromResult<User>(null);
            }
            var userInfo = _usersService.Get(externalInfo.UserId);
            var user = userInfo?.MapTo<User>();
            return Task.FromResult(user);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        #endregion

        private string GetLoginId(UserLoginInfo login)
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                var clearBytes = Encoding.UTF8.GetBytes(login.LoginProvider + "|" + login.ProviderKey);
                var hashBytes = sha.ComputeHash(clearBytes);
                var sb = new StringBuilder(hashBytes.Length * 2);
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();

            }
        }
        public void Dispose()
        {

        }

    }
}
