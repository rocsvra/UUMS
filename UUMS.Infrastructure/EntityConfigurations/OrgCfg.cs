using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class OrgCfg : IEntityTypeConfiguration<Org>
    {
        public void Configure(EntityTypeBuilder<Org> builder)
        {
            builder.ToTable("Org");
            builder.HasKey(o => new { o.Id });
            builder.Property(o => o.Name).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Description).HasMaxLength(300);

            builder.HasMany(o => o.Users).WithOne(o => o.Org);
            builder.HasMany(o => o.Jobs).WithOne(o => o.Org);
        }
    }
}
