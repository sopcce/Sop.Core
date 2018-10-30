using ItemDoc.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Mapping
{
    public class FileServerInfoMapping : ClassMapping<FileServerInfo>
    {
        public FileServerInfoMapping()
        {
            // ReSharper disable once StringLiteralTypo
            Table("item_fileserver");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native));

            Property(t => t.ServerId);
            Property(t => t.ServerName);
            Property(t => t.ServerEnName);
            Property(t => t.ServerUrl);
            Property(t => t.RootPath);
            Property(t => t.VirtualPath);
            Property(t => t.DiskPath);
            Property(t => t.MaxAmount);
            Property(t => t.CurAmount);
            Property(t => t.Size);
            Property(t => t.Enabled);
            Property(t => t.DisplayOrder);

            Property(t => t.DateCreated);

        }
    }
}
