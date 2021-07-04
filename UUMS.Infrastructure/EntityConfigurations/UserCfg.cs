using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UUMS.Domain.DO;

namespace UUMS.Infrastructure.EntityConfigurations
{
    class UserCfg : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(o => new { o.Id });
            builder.Property(o => o.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");
            builder.Property(o => o.Account)
                .IsRequired()
                .HasColumnType("varchar(30)");
            builder.Property(o => o.Password)
                .IsRequired()
                .HasColumnType("varchar(100)");
            builder.Property(o => o.Sex)
                .HasColumnType("bit");
            builder.Property(o => o.Mobile)
                .IsRequired()
                .HasColumnType("varchar(50)");
            builder.Property(o => o.Mail)
                .IsRequired()
                .HasColumnType("varchar(100)");
            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.HasAlternateKey(o => o.Account);
        }
    }
}
