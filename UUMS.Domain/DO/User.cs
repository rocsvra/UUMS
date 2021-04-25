using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNo { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public Guid OrgId { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public Org Org { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public ICollection<Role> Roles { get; set; }
    }
}
