﻿using Ardalis.Specification;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        public UserFilterSpecification(Expression<Func<User, bool>> criteria)
            : base()
        {
            Query.Include(o=>o.AvatarFile).Where(criteria);
        }

        public UserFilterSpecification(int pageIndex, int pageSize, string name)
             : base()
        {
            Query.Where(o => string.IsNullOrEmpty(name) || o.Name.Contains(name))
                .OrderBy(o => o.Account)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
        }

        public UserFilterSpecification(Guid id)
        {
            Query.Include(o => o.Roles).Where(o => o.Id == id);
        }
    }
}
