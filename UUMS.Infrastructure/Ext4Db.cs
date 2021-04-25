using AdunTech.CommonDomain;
using AdunTech.CommonInfra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace UUMS.Infrastructure
{
    public static class Ext4Db
    {
        public static void RegisterDbContexts(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(Ext4Db).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<UumsDbContext>(options => 
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)), ServiceLifetime.Scoped);
            services.AddTransient<IDbContext, UumsDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
