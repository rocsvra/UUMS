using Ardalis.Specification;
using UUMS.Domain.DO;

namespace UUMS.Application.Specifications
{
    /// <summary>
    /// 客户端分页过滤
    /// </summary>
    public class ClientFilterSpecification : Specification<Client>
    {
        public ClientFilterSpecification(int pageIndex, int pageSize, string name)
            : base()
        {
            Query.Where(o => string.IsNullOrEmpty(name) || o.Name.Contains(name))
                .OrderBy(o => o.SortNo)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
        }

        public ClientFilterSpecification(string name)
            : base()
        {
            Query.Where(o => string.IsNullOrEmpty(name) || o.Name.Contains(name));
        }
    }
}
