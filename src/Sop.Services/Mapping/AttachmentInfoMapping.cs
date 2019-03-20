using Sop.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Mapping
{
    public class AttachmentInfoMapping : ClassMapping<AttachmentInfo>
    {
        public AttachmentInfoMapping()
        {
            Table("item_fileattachment");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native));
            Property(t => t.AttachmentId);
            Property(t => t.OwnerId);
            Property(t => t.ServerId);
            Property(t => t.ServerUrlPath);
            Property(t => t.Extension);
            Property(t => t.Size);
            Property(t => t.FileNames);
            Property(t => t.UploadFileName);
            Property(t => t.MimeType);
            Property(t => t.Status);
            Property(t => t.DisplayOrder);
            Property(t => t.Ip);
            Property(t => t.DateCreated);

            Property(t => t.HasThumbnail);
            Property(t => t.HtmlPreview);
            Property(t => t.PageCount);
            Property(t => t.DateCreated);
            


        }
    }
}
