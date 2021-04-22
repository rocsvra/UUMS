using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace UUMS.Infrastructure
{
    public static class Ext4Db
    {
        public static void RegisterDbContexts(this IServiceCollection services, string uumsConnConnectionString)
        {
            var migrationsAssembly = typeof(Ext4Db).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<UUMS_Context>(options => options.UseSqlServer(uumsConnConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}
