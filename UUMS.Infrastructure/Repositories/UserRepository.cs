using AdunTech.CommonInfra;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext) { }

        public override User Find(object id)
        {
            return DataSet
                .Include(itm => itm.Roles)
                .FirstOrDefault(itm => itm.Id == (Guid)id);
        }

        public void ModifyRoles(User user)
        {

        }
    }
}
