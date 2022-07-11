using Ardalis.Specification;
using System;
using System.Collections.Generic;
using UUMS.Domain.DO;

namespace UUMS.Application.Specifications
{
    public class RoleFilterSpecification : Specification<Role>
    {
        public RoleFilterSpecification(Guid clientId, int pageIndex, int pageSize, string name)
            : base()
        {
            Query.Where(o => o.ClientId == clientId
                    && (string.IsNullOrEmpty(name) || o.Name.Contains(name)))
                .OrderBy(o => o.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
        }

        public RoleFilterSpecification(Guid clientId, string name)
            : base()
        {
            Query.Where(o =>
                o.ClientId == clientId
                && (string.IsNullOrEmpty(name) || o.Name.Contains(name)));
        }

        public RoleFilterSpecification(Guid roleId, bool withMenus, bool withUsers)
        {
            if (withMenus && withUsers)
            {
                Query.Where(o => o.Id == roleId)
                    .Include(o => o.Menus)
                    .Include(o => o.Users);
            }
            else if (withMenus)
            {
                Query.Where(o => o.Id == roleId)
                    .Include(o => o.Menus);
            }
            else if (withUsers)
            {
                Query.Where(o => o.Id == roleId)
                    .Include(o => o.Users);
            }
        }

        public RoleFilterSpecification(List<Guid> roleids)
            : base()
        {
            Query.Include(o => o.Menus).Where(o => roleids.Contains(o.Id));
        }
    }
}
