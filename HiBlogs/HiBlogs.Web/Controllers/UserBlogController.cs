using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HiBlogs.EntityFramework.EntityFramework;
using HiBlogs.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HiBlogs.Web.Controllers
{
    public class UserBlogController : Controller
    {
        private readonly HiBlogsDbContext db;
        public UserBlogController(HiBlogsDbContext db)
        {
            this.db = db;
        }
        // GET: /<controller>/
        public IActionResult Blog(string userName, int blogId)
        {
            var blog = db.Blogs.Where(t => t.User.UserName == userName && t.Id == blogId)
               .Select(t => new BlogViewModel
               {
                   Title = t.Title,
                   Content = t.Content
               }).FirstOrDefault();
            return View(blog);
        }
    }
}
