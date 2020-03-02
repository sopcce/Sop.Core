using Sop.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Mapping
{
    public class LinksInfoMapping : ClassMapping<LinksInfo>
    {
        public LinksInfoMapping()
        {
            Table("sop_links");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native));
            Property(t => t.OwnerId);
            Property(t => t.LinkName);
            Property(t => t.LinkUrl);
            Property(t => t.Title);
            Property(t => t.Target);
            Property(t => t.ImageUrl);
            Property(t => t.Description);
            Property(t => t.IsEnabled);
            Property(t => t.DisplayOrder);
            Property(t => t.DateCreated);
            Property(t => t.LastModified);
        }
    }
}
