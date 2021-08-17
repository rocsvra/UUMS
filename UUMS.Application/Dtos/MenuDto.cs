using System;

namespace UUMS.Application.Dtos
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        public Guid ClientId { get; set; }
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
    }
}
