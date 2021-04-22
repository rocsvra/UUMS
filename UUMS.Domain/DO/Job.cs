using System;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 岗位
    /// </summary>
    public class Job
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public Guid OrgId { get; set; }
        /// <summary>
        /// 所属机构
        /// </summary>
        public Org Org { get; set; }
    }
}
