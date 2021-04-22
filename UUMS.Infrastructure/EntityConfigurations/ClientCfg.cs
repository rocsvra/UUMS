using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class ClientCfg : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(o => new { o.Id });
            builder.Property(o => o.Name).IsRequired();
            builder.Property(o => o.NeedMenu).IsRequired().HasMaxLength(50);

            builder.HasMany(o => o.Menus).WithOne(o => o.Client);
        }
    }
}
