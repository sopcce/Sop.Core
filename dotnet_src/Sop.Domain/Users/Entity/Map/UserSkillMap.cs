using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Domain.Entity.Map
{
    public class UserSkillMap : BaseMapEntityTypeConfiguration<UserSkill>
    {
        /// <summary>
        ///     sop_user_skills
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<UserSkill> builder)
        {
            builder.ToTable("sop_user_skills");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.UserId).HasMaxLength(64).IsRequired();
            base.Configure(builder);
        }
    }
}