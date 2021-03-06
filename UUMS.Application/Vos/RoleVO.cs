using System;

namespace UUMS.Application.Vos
{
    public class RoleVO
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
        /// 客户端
        /// </summary>
        public Guid ClientId { get; set; }
    }
}
