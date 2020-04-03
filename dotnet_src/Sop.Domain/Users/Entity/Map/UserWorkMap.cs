using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Domain.Entity.Map
{
    /// <summary>
    /// </summary>
    public class UserWorkMap : BaseMapEntityTypeConfiguration<UserWork>
    {
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<UserWork> builder)
        {
            builder.ToTable("sop_user_work");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.UserId).HasMaxLength(64).IsRequired();
            base.Configure(builder);
        }
    }
}