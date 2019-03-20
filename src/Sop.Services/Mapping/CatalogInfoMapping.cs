using Sop.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Mapping
{
    public class CatalogInfoMapping : ClassMapping<CatalogInfo>
    {
        public CatalogInfoMapping()
        {
            Table("item_catalog");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native));
            Property(t => t.UserId);
            Property(t => t.ItemId);
            Property(t => t.Name);
            Property(t => t.TenantId);
            Property(t => t.Description);
            Property(t => t.ParentId);
            Property(t => t.ParentIdList);
            Property(t => t.ChildCount);
            Property(t => t.Depth);
            Property(t => t.Enabled);
            Property(t => t.ContentCount);
            Property(t => t.ViewCountType);
            Property(t => t.DisplayOrder);
            Property(t => t.DateCreated);
 
            Property(t => t.Tags);
            Property(t => t.Color);
            Property(t => t.Url);
            Property(t => t.BackColor);
 

        }
    }
}
