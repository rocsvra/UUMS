using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class OrgCfg : IEntityTypeConfiguration<Org>
    {
        public void Configure(EntityTypeBuilder<Org> builder)
        {
            builder.HasKey(o => new { o.Id });
            builder.Property(o => o.Name).IsRequired();

            builder.HasMany(o => o.Users).WithOne(o => o.Org);
            builder.HasMany(o => o.Jobs).WithOne(o => o.Org);
        }
    }
}
