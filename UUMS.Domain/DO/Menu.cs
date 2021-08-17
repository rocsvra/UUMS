using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 总是显示（顶层）
        /// </summary>
        public bool AlwaysShow { get; set; }
        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Hidden { get; set; }
        /// <summary>
        /// 重定向
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
        /// 关联角色
        /// </summary>
        public ICollection<Role> Roles { get; set; }
        /// <summary>
        /// 所属客户端
        /// </summary>
        public Client Client { get; set; }
    }
}
