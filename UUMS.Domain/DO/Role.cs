using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : EntityBase
    {
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
        /// 关联菜单
        /// </summary>
        public List<Menu> Menus { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        public List<User> Users { get; set; }
    }
}
