using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Definitions
{
    public interface IHiSession
    {
        int? UserId { get; }

        string UsreName { get; }

        string UserNickname { get; }
    }
}
