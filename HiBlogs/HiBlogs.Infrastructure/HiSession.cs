using HiBlogs.Definitions;
using HiBlogs.Definitions.HiClaimTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HiBlogs.Infrastructure
{
    public class HiSession : IHiSession
    {
        public int? UserId { get; }

        public string UsreName { get; }

        public string UserNickname { get; }

        public HiSession()
        { }

        public HiSession(ClaimsPrincipal User)
        {
            // UserId
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == HiClaimTypes.UserId);
            if (string.IsNullOrEmpty(userIdClaim?.Value))
                return;
            if (int.TryParse(userIdClaim.Value, out int userId))
                UserId = userId;

            // UsreName
            var userNameClaim = User.Claims.FirstOrDefault(c => c.Type == HiClaimTypes.UserName);
            UsreName = userNameClaim?.Value;

            //Nickname
            var nicknameClaim = User.Claims.FirstOrDefault(c => c.Type == HiClaimTypes.Nickname);
            UserNickname = nicknameClaim?.Value;

            var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == HiClaimTypes.Role);
        }
    }
}
