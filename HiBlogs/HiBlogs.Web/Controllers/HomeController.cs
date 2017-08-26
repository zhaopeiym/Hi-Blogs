using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HiBlogs.Web.Models;
using HiBlogs.EntityFramework.EntityFramework;

namespace HiBlogs.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HiBlogsDbContext _db;

        public HomeController(HiBlogsDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var blogInfos = _db.Blogs.Select(t => new BlogInfoViewModel
            {
                Title = t.Title,
                Id = t.Id
            }).ToList();
            return View(blogInfos);
        }

        public IActionResult Blog(int Id)
        {
            var blog= _db.Blogs.Where(t => t.Id == Id)
                .Select(t => new BlogViewModel
                {
                    Title = t.Title,
                    Content = t.Content
                }).FirstOrDefault();
            return View(blog);
        }
    }
}
