using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiBlogs.Web.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public string UserName { get;  set; }
        public string Password { get;  set; }
        public bool RememberMe { get;  set; }
        public string Email { get; internal set; }
    }
}
