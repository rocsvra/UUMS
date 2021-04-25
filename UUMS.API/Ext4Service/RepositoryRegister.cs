using Microsoft.Extensions.DependencyInjection;
using UUMS.Domain.IRepositories;
using UUMS.Infrastructure.Repositories;

namespace UUMS.API
{
    /// <summary>
    /// 注册仓储
    /// </summary>
    public static class RepositoryRegister
    {
        public static void RegisteRepository(this IServiceCollection services)
        {
            services.AddTransient<IClientRepository, ClientRepository>();
        }
    }
}
