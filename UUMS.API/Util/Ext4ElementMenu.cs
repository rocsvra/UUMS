using System;
using System.Collections.Generic;
using System.Linq;
using UUMS.Domain.DO;

namespace UUMS.API.Util
{
    public static class Ext4ElementMenu
    {
        public static List<ElementMenuVO> ToElementMenu(this IEnumerable<Menu> items)
        {
            return GetTreeChildren(items, null);
        }

        /// <summary>
        /// 递归实现树
        /// </summary>
        /// <param name="items">所有数据</param>
        /// <param name="parentId">子数据的父ID</param>
        /// <returns></returns>
        private static List<ElementMenuVO> GetTreeChildren(IEnumerable<Menu> items, Guid? parentId)
        {
            var subItems = items.Where(o => o.ParentId == parentId).OrderBy(o => o.SortNo);
            var nodes = new List<ElementMenuVO>();
            foreach (var subItem in subItems)
            {
                nodes.Add(new ElementMenuVO()
                {
                    id = subItem.Id,
                    name = subItem.Name,
                    alwaysShow = subItem.AlwaysShow,
                    hidden = subItem.Hidden,
                    redirect = subItem.Redirect,
                    path = subItem.Path,
                    component = subItem.Component,
                    meta = new ElementMenuMeta
                    {
                        title = subItem.Title,
                        icon = subItem.Icon,
                        noCache = subItem.NoCache
                    },
                    children = GetTreeChildren(items, subItem.Id)
                });
            }
            return nodes;
        }
    }
}
