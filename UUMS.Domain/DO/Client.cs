using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class Client
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否需要菜单
        /// </summary>
        public bool NeedMenu { get; set; }

        public ICollection<Menu> Menus { get; set; }
    }
}
