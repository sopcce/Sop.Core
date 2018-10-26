using ItemDoc.Services.Auth.Model;
using Microsoft.AspNet.Identity;

namespace ItemDoc.Services.Auth.Identity
{
  public class UserManager : UserManager<User, long>
    {
        public UserManager(IUserStore<User, long> store): base(store)
        {
            this.UserLockoutEnabledByDefault = false;
            // this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            // this.MaxFailedAccessAttemptsBeforeLockout = 10;
            this.UserValidator = new UserValidator<User, long>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                //RequireNonLetterOrDigit = false,
                //RequireDigit = false,
                //RequireLowercase = false,
                //RequireUppercase = false,
            };
           

        }

    }
}
