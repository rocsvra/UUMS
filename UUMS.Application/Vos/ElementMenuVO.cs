using System;
using System.Collections.Generic;

namespace UUMS.Application.Vos
{
    /// <summary>
    /// ElementUI菜单结构
    /// </summary>
    public class ElementMenuVO
    {
        /// <summary>
        /// 菜单id
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 总是显示（顶层）
        /// </summary>
        public bool alwaysShow { get; set; }
        /// <summary>
        /// 隐藏
        /// </summary>
        public bool hidden { get; set; }
        /// <summary>
        /// 重定向
        /// </summary>
        public string redirect { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string component { get; set; }
        /// <summary>
        /// 菜单元数据
        /// </summary>
        public ElementMenuMeta meta { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<ElementMenuVO> children { get; set; }
    }

    /// <summary>
    /// 菜单元数据
    /// </summary>
    public class ElementMenuMeta
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public bool noCache { get; set; }
    }
}
