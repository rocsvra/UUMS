using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class FssInfoCfg : IEntityTypeConfiguration<FssInfo>
    {
        public void Configure(EntityTypeBuilder<FssInfo> builder)
        {
            builder.ToTable("FssInfo");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.FileName).IsRequired().HasMaxLength(500);
            builder.Property(o => o.Extension).HasMaxLength(20);
            builder.Property(o => o.FileSize);
            builder.Property(o => o.ContentType);
            builder.Property(o => o.CreatedAt).IsRequired();
        }
    }
}
