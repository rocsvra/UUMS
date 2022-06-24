using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    public  class FssInfo
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 文件名（虚拟磁盘路径/数据两级目录/文件名）
        /// 不包含组名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 后缀名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }


        public List<User> Users { get; set; }
    }
}
