using ItemDoc.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
namespace ItemDoc.Services.Mapping
{


    public class UsersInfoMapping : ClassMapping<UsersInfo>
    {
        public UsersInfoMapping()
        {
            Table("item_users");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Assigned));

            Property(t => t.UserId);
            Property(t => t.UserName);
            Property(t => t.Email);
            Property(t => t.EmailConfirmed);
            Property(t => t.PhoneNumber);
            Property(t => t.PhoneNumberConfirmed);
            Property(t => t.AccountIDcard);
            Property(t => t.UrlToken);
            Property(t => t.PassWord);
            Property(t => t.SecurityStamp);
            Property(t => t.TrueName);
            Property(t => t.NickName);
            Property(t => t.Status);
            Property(t => t.StatusNotes);
            Property(t => t.CreatedIP);
            Property(t => t.ActivityTime);
            Property(t => t.LastActivityTime);
            Property(t => t.ActivityIP);
            Property(t => t.LastActivityIP);
            Property(t => t.DateCreated);
            Property(t => t.DisplayOrder);
            Property(t => t.LockoutEndDateUtc);
            Property(t => t.LockoutEnabled);
            Property(t => t.AccessFailedCount);
            Property(t => t.TwoFactorEnabled);



        }
    }


}