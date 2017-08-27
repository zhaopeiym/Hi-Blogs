using HiBlogs.Core.AuditedEntitys;
using HiBlogs.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    ///
    /// </summary>
    public class Role : IdentityRole<int>
    {      
        /// <summary>
        /// 权限
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual ICollection<RolePermissionName> RolePermissionNames { get; set; }
    }
}
