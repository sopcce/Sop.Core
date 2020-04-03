using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Domain.Entity.Map
{
    //


    /// <summary>
    /// </summary>
    public class UserExternalLoginMap : BaseMapEntityTypeConfiguration<UserExternalLogin>
    {
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<UserExternalLogin> builder)
        {
            builder.ToTable("sop_user_external_login");
            builder.HasKey(n => n.Id);
            base.Configure(builder);
        }
    }
}