using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core.Authorization
{
    /// <summary>
    /// 权限
    /// </summary>
    public enum PermissionNamesEnum
    {
        /// <summary>
        /// 超级管理
        /// </summary>
        Administrator = 1,
        /// <summary>
        /// 管理
        /// </summary>
        Admin = 2,
        /// <summary>
        /// 有js和css权限
        /// </summary>
        Script = 3,
        /// <summary>
        /// 发布
        /// </summary>
        Publish = 4,
        /// <summary>
        /// 浏览
        /// </summary>
        Browse = 5
    }
}
