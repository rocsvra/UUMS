using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using UUMS.Domain.DO;

namespace UUMS.Application.Specifications
{
    public class MenuFilterSpecification : Specification<Menu>
    {
        public MenuFilterSpecification(Guid clientId, string name)
            : base()
        {
            Query.Where(o =>
                  (string.IsNullOrEmpty(name) || o.Name.Contains(name))
                  && o.ClientId == clientId);
        }

        public MenuFilterSpecification(List<Guid> menuids)
            : base()
        {
            Query.Where(o => menuids.Contains(o.Id));
        }
    }
}
