using ItemDoc.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Mapping
{
    public class FileServerInfoMapping : ClassMapping<FileServerInfo>
    {
        public FileServerInfoMapping()
        {
            Table("item_items");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native));

            Property(t => t.Enabled);
            Property(t => t.DisplayOrder);
            Property(t => t.DateCreated);

        }
    }
}
