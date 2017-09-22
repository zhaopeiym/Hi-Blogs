using Microsoft.AspNetCore.Identity;
using System;

namespace HiBlogs.Core.Entities
{
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// 第三方登录唯一标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
