using AdunTech.CommonInfra;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext) { }
    }
}
