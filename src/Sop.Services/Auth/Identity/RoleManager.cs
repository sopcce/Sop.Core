using Sop.Services.Auth.Model;
using Microsoft.AspNet.Identity;

namespace Sop.Services.Auth.Identity
{
  public class RoleManager : RoleManager<Role, string>
    {
        public RoleManager(IRoleStore<Role, string> store): base(store)
        {
            this.RoleValidator = new RoleValidator<Role, string>(this);
        }
    }
}
