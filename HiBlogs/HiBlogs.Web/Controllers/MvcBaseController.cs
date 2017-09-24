using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HiBlogs.EntityFramework.EntityFramework;
using HiBlogs.Web.Models;
using System.Security.Claims;
using HiBlogs.Definitions;
using HiBlogs.Infrastructure;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HiBlogs.Web.Controllers
{
    public class MvcBaseController : Controller
    {
        public IHiSession HiSession
        {
            get
            {
                if (HttpContext != null)
                    return new HiSession(HttpContext.User);
                else
                    return new HiSession();
            }
        }        
    }
}
