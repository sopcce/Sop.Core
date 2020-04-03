using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Domain.Entity.Map
{
    /// <summary>
    /// </summary>
    public class UserMap : BaseMapEntityTypeConfiguration<User>
    {
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("sop_user");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.UserId).HasMaxLength(64).IsRequired();
            base.Configure(builder);
        }
    }
}