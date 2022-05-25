using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : EntityBase
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 名称（必填）
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 如果为true，菜单（root），总是显示
        /// 如果为false，没有子菜单则不显示
        /// </summary>
        public bool AlwaysShow { get; set; }
        /// <summary>
        /// 如果为true，侧边栏不显示
        /// 默认值：false
        /// </summary>
        public bool Hidden { get; set; }
        /// <summary>
        /// 重定向
        /// 默认值：noRedirect
        /// </summary>
        public string Redirect { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 菜单显示标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public bool NoCache { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 关联角色
        /// </summary>
        public ICollection<Role> Roles { get; set; }
        /// <summary>
        /// 客户端ID
        /// </summary>
        public Guid ClientId { get; set; }
        /// <summary>
        /// 所属客户端
        /// </summary>
        public Client Client { get; set; }
    }
}
