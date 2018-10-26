using ItemDoc.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Mapping
{
    public class ChangeLogInfoMapping : ClassMapping<ChangeLogInfo>
    {
        public ChangeLogInfoMapping()
        {
            Table("item_catalog");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native));
            Property(t => t.DataDate);
            Property(t => t.Title);
            Property(t => t.Summary);
            Property(t => t.Body);
            Property(t => t.Selected);
            Property(t => t.Enabled);
            Property(t => t.DateCreated);

        }
    }
}
