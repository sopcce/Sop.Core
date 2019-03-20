using Sop.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Mapping
{
    public class PostInfoMapping : ClassMapping<PostInfo>
    {
        public PostInfoMapping()
        {
            Table("item_posts");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native)); 
            Property(t => t.CatalogId);
            Property(t => t.UserId);
            Property(t => t.Title);
            Property(t => t.Description);
            Property(t => t.Content);
            Property(t => t.HtmlContentPath);
            Property(t => t.ViewCount);
            Property(t => t.DisplayOrder);
            Property(t => t.DateCreated);
            Property(t => t.CreatedIp);



        }
    }
}
