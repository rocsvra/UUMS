using System;
using System.Collections.Generic;
using System.Linq;
using UUMS.Application.Vos;
using UUMS.Domain.DO;

namespace UUMS.Application.Util
{
    public static class Ext4ElementMenu
    {

        public static List<Guid> GetParentIds(this IEnumerable<Menu> items, Guid id)
        {
            List<Guid> pids = new List<Guid>();
            AddPid(items, id, ref pids);
            pids.Reverse();
            return pids;
        }

        public static void AddPid(IEnumerable<Menu> items, Guid id, ref List<Guid> pids)
        {
            var item = items.Where(o => o.Id == id).First();
            if (item.ParentId == null) { return; }
            pids.Add((Guid)item.ParentId);
            var pitem = items.Where(o => o.Id == item.ParentId).First();
            if (pitem.ParentId != null)
            {
                AddPid(items, (Guid)pitem.ParentId, ref pids);
            }
        }

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
                var vo = new ElementMenuVO()
                {
                    id = subItem.Id,
                    name = subItem.Name,
                    alwaysShow = subItem.AlwaysShow,
                    hidden = subItem.Hidden,
                    redirect = subItem.Redirect,
                    path = subItem.Path,
                    component = subItem.Component,
                    sortNo = subItem.SortNo,
                    meta = new ElementMenuMeta
                    {
                        title = subItem.Title,
                        icon = subItem.Icon,
                        noCache = subItem.NoCache
                    },
                    children = GetTreeChildren(items, subItem.Id)
                };
                if (vo.children.Count == 0) vo.children = null;
                nodes.Add(vo);
            }
            return nodes;
        }
    }
}
