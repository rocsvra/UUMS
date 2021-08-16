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
        /// 是否拥有菜单
        /// </summary>
        public bool HasMenu { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public List<Menu> Menus { get; set; }
    }
}
