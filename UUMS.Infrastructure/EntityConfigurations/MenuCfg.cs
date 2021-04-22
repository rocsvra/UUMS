using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class MenuCfg : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(o => o.CreatedBy).IsRequired();

            builder.HasOne(o => o.Client)
                .WithMany(o => o.Menus)
                .HasForeignKey(o => o.ClientId);
        }
    }
}
