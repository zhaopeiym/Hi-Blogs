using HiBlogs.Core.AuditedEntitys;
using HiBlogs.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    /// 角色 权限关联表
    /// </summary>
    public class RolePermissionName: Entity<int>
    {
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public PermissionNamesEnum PermissionName { get; set; }
    }
}
