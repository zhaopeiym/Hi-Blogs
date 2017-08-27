using HiBlogs.Core.AuditedEntitys;
using HiBlogs.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiBlogs.Core.Entities
{
    public class User : IdentityUser<int>
    {
        ///// <summary>
        ///// 用户名（账号）
        ///// </summary>
        //public string Name { get; set; }

        ///// <summary>
        ///// 显示名字
        ///// </summary>
        //public string DisplayName { get; set; }

        ///// <summary>
        ///// 密码
        ///// </summary>
        //public string Passwod { get; set; }

        ///// <summary>
        ///// 关联的角色
        ///// </summary>
        //[ForeignKey("UserId")]
        //public ICollection<UserRole> UserRoles { get; set; }
    }
}
