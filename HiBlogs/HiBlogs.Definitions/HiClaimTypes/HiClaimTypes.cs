using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace HiBlogs.Definitions.HiClaimTypes
{
    public class HiClaimTypes
    {
        /// <summary>
        /// 账号
        /// </summary>
        public static string UserName { get; } = ClaimTypes.Name;
        /// <summary>
        /// id
        /// </summary>
        public static string UserId { get; } = ClaimTypes.NameIdentifier;
        /// <summary>
        /// 角色
        /// </summary>
        public static string Role { get; } = ClaimTypes.Role;
        /// <summary>
        /// 昵称
        /// </summary>
        public static string Nickname { get; }= "http://www.haojima.net/identity/claims/nickName";
    }
}
