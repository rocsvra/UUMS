using AdunTech.CommonInfra;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.Infrastructure.Repositories
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IDbContext dbContext) : base(dbContext) { }

        public override Menu Find(object id)
        {
            return DataSet
                .Include(itm => itm.Client)
                .FirstOrDefault(itm => itm.Id == (Guid)id);
        }
    }
}
