using System;
using System.Collections.Generic;

namespace UUMS.Domain.DO
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : EntityBase
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号/工号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool? Sex { get; set; } 
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public Org? Org { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// 头像文件Id
        /// </summary>
        public Guid? AvatarFileId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public FssInfo? AvatarFile { get; set; }
    }
}
