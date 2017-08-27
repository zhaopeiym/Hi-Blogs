using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiBlogs.Web.Authorization
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute//Attribute
    {
        public MvcAuthorizeAttribute()
        {

        }
    }
}
