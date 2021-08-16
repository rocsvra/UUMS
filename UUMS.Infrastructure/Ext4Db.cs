using AdunTech.CommonDomain;
using AdunTech.CommonInfra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UUMS.Domain.IRepositories;
using UUMS.Infrastructure.Repositories;

namespace UUMS.Infrastructure
{
    public static class Ext4Db
    {
        public static void RegisterRepository(this IServiceCollection services, string connectionString)
        {
            //注册DbContext
            var migrationsAssembly = typeof(Ext4Db).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<UumsDbContext>(options =>
                options.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly)),
                ServiceLifetime.Scoped);
            services.AddTransient<IDbContext, UumsDbContext>();
            //注册工作单元
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //注册仓储
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
        }
    }
}
