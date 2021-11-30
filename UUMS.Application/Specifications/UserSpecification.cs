using Ardalis.Specification;
using System.Linq;
using UUMS.Domain.DO;

namespace UUMS.Application.Specifications
{
    public class UserFilterSpecification : Specification<User>
    {
        public UserFilterSpecification(string name)
           : base()
        {
            Query.Where(o => string.IsNullOrEmpty(name) || o.Name.Contains(name));
        }

        public UserFilterSpecification(string account, string password)
            : base()
        {
            Query.Where(o => o.Account == account);
        }

        public UserFilterSpecification(int pageIndex, int pageSize, string name)
             : base()
        {
            Query.Where(o => string.IsNullOrEmpty(name) || o.Name.Contains(name))
                .OrderBy(o => o.Account)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
        }
    }
}
