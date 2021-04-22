using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class UserCfg : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(o => new { o.Id });
            builder.Property(o => o.Name).IsRequired().HasColumnType("varchar(50)").HasMaxLength(20);

            builder.HasOne(o => o.Org)
                .WithMany(o => o.Users)
                .HasForeignKey(o => o.OrgId);
        }
    }
}
