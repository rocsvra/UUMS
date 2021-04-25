using AdunTech.CommonInfra;
using Microsoft.EntityFrameworkCore;
using UUMS.Domain.DO;
using UUMS.Infrastructure.EntityConfigurations;

namespace UUMS.Infrastructure
{
    public class UumsDbContext : DbContext, IDbContext
    {
        public UumsDbContext() { }

        public UumsDbContext(DbContextOptions<UumsDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;User Id=sa;Password=1;Database=UUMS;Persist Security Info=True;");
            }
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Org> Orgs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClientCfg());
            modelBuilder.ApplyConfiguration(new JobCfg());
            modelBuilder.ApplyConfiguration(new MenuCfg());
            modelBuilder.ApplyConfiguration(new OrgCfg());
            modelBuilder.ApplyConfiguration(new RoleCfg());
            modelBuilder.ApplyConfiguration(new UserCfg());
        }
    }
}
