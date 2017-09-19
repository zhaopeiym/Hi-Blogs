using HiBlogs.Core.AuditedEntitys;
using HiBlogs.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiBlogs.Core.Entities
{
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// 第三方登录唯一标识
        /// </summary>
        public string OpenId { get; set; }
        public string Nickname { get; set; }
    }
}
