﻿using System;

namespace UUMS.Application.Dtos
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
