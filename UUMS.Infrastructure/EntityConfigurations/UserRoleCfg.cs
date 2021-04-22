using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    public class UserRoleCfg : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(o => new { o.UserId, o.RoleId });

            builder.HasOne(o => o.Role)
                .WithMany(o => o.UserRoles)
                .HasForeignKey(o => o.RoleId);

            builder.HasOne(o => o.User)
                .WithMany(o => o.UserRoles)
                .HasForeignKey(o => o.UserId);
        }
    }
}
