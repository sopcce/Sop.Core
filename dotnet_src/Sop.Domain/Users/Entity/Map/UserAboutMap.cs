using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Domain.Entity.Map
{
    /// <summary>
    /// </summary>
    public class UserAboutMap : BaseMapEntityTypeConfiguration<UserAbout>
    {
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<UserAbout> builder)
        {
            builder.ToTable("sop_user_about");
            builder.HasKey(n => n.Id);
            base.Configure(builder);
        }
    }
}