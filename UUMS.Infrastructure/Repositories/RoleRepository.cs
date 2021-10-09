using AdunTech.CommonInfra;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbContext dbContext) : base(dbContext) { }

        //public override IEnumerable<Role> Query(Expression<Func<Role, bool>> predicate)
        //{
        //    var roles = DataSet.Where(predicate)
        //        .Include(o => o.Menus);
        //}
    }
}
