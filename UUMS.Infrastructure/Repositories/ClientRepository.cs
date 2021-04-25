using AdunTech.CommonInfra;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.Infrastructure.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(IDbContext dbContext) : base(dbContext) { }
    }
}
