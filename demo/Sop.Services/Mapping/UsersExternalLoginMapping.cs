using Sop.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Mapping
{
    public class UsersExternalLoginMapping : ClassMapping<UsersExternalLoginInfo>
    {
        public UsersExternalLoginMapping()
        {
            Table("sop_users_external_login");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Assigned)); 
            Property(t => t.LoginProvider);
            Property(t => t.ProviderKey);
            Property(t => t.UserId); 
            Property(t => t.DateCreated); 

        }
    }
}
