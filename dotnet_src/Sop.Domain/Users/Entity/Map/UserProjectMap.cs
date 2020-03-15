using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Domain.Entity.Map
{
    public class UserProjectMap : BaseMapEntityTypeConfiguration<UserProject>
    {
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.ToTable("sop_user_project");
            builder.HasKey(n => n.Id);
            base.Configure(builder);
        }
    }
}