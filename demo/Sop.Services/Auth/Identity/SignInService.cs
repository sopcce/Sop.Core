using System.Threading.Tasks;
using Sop.Services.Auth.Model;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Sop.Services.Auth.Identity
{
  public class SignInService : SignInManager<User, long>
  {
    public SignInService(UserManager userManager, IAuthenticationManager authenticationManager)
      : base(userManager, authenticationManager)
    {

    }

    public override Task SignInAsync(User user, bool isPersistent, bool rememberBrowser)
    {
      return base.SignInAsync(user, isPersistent, rememberBrowser);
    }
  }
}
