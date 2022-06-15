using System;

namespace UUMS.Application.Dtos
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class ClientDto
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
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }
    }
}
