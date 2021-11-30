using Ardalis.Specification;
using System;
using System.Collections.Generic;
using UUMS.Domain.DO;

namespace UUMS.Application.Specifications
{
    public class RoleFilterSpecification : Specification<Role>
    {
        public RoleFilterSpecification(int pageIndex, int pageSize, string name)
            : base()
        {
            Query.Where(o => string.IsNullOrEmpty(name) || o.Name.Contains(name))
                .OrderBy(o => o.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
        }

        public RoleFilterSpecification(string name)
            : base()
        {
            Query.Where(o => string.IsNullOrEmpty(name) || o.Name.Contains(name));
        }

        public RoleFilterSpecification(List<Guid> roleids)
            : base()
        {
            Query.Where(o => roleids.Contains(o.Id));
        }
    }
}
