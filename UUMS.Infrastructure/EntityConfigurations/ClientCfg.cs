using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class ClientCfg : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");
            builder.HasKey(o => new { o.Id });
            builder.Property(o => o.Name).IsRequired().HasMaxLength(50);
            builder.Property(o => o.HasMenu).IsRequired();
            builder.Property(o => o.SortNo).IsRequired();

            builder.HasMany(o => o.Menus).WithOne(o => o.Client).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
