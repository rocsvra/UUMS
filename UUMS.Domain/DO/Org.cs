using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public class Org
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 关联用户
        /// </summary>
        public ICollection<User> Users { get; set; }
        /// <summary>
        /// 关联岗位
        /// </summary>
        public ICollection<Job> Jobs { get; set; }
    }
}
