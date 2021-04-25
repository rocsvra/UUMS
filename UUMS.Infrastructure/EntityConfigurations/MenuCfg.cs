using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class MenuCfg : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menu");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Redirect).HasMaxLength(200);
            builder.Property(o => o.Path).HasMaxLength(200);
            builder.Property(o => o.Component).HasMaxLength(100);
            builder.Property(o => o.Title).HasMaxLength(100);
            builder.Property(o => o.Icon).HasMaxLength(50);
            builder.Property(o => o.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(o => o.LastUpdatedBy).HasMaxLength(50);

            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(o => o.Client)
                .WithMany(o => o.Menus)
                .HasForeignKey(o => o.ClientId);
        }
    }
}
