using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public class UserRole : Entity<int>
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
