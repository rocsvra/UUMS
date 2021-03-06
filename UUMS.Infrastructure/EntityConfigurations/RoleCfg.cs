using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class RoleCfg : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Description).HasMaxLength(200);
            builder.Property(o => o.Enabled).IsRequired();
            builder.Property(o => o.CreatedAt).IsRequired();
            builder.Property(o => o.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(o => o.LastUpdatedAt);
            builder.Property(o => o.LastUpdatedBy).HasMaxLength(50);


            builder.HasOne(o => o.Client)
                .WithMany(o => o.Roles)
                .HasForeignKey(o => o.ClientId);
        }
    }
}
