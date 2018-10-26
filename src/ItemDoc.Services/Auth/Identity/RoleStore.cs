using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemDoc.Services.Auth.Model;
using Microsoft.AspNet.Identity;

namespace ItemDoc.Services.Auth.Identity
{
  public class RoleStore : IRoleStore<Role, string>, IQueryableRoleStore<Role, string>
  {


    public RoleStore(string folderStorage)
    {


    }

    public Task CreateAsync(Role role)
    {
      var ccc = role;
      return Task.FromResult(role);
    }

    public Task DeleteAsync(Role role)
    {
      throw new NotImplementedException();
    }

    public Task<Role> FindByIdAsync(string roleId)
    {
      Role role = null;
      IList<Role> roles = null;
      if (roles == null || roles.Count == 0)
      {
        return Task.FromResult(role);
      }

      role = roles.SingleOrDefault(f => f.Id == roleId);

      return Task.FromResult(role);
    }

    public Task<Role> FindByNameAsync(string roleName)
    {
      Role role = null;
      IList<Role> roles = null;
      if (roles == null || roles.Count == 0)
      {
        return Task.FromResult(role);
      }

      role = roles.SingleOrDefault(f => f.Name == roleName);

      return Task.FromResult(role);
    }

    public Task UpdateAsync(Role role)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {

    }

    public IQueryable<Role> Roles
    {
      get
      {
        return (new List<Role>().AsQueryable<Role>());
      }
    }
  }
}
