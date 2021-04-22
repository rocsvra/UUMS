using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 最后更新用户
        /// </summary>
        public string LastUpdatedBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// 关联菜单
        /// </summary>
        public ICollection<Menu> Menus { get; set; }
        
        /// <summary>
        /// 用户
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
