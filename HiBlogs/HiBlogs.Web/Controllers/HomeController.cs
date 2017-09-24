using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HiBlogs.Web.Models;
using HiBlogs.EntityFramework.EntityFramework;
using HiBlogs.Application;
using HiBlogs.Definitions.Config;
using System.Threading;
using System.Security.Claims;

namespace HiBlogs.Web.Controllers
{
    public class HomeController : MvcBaseController
    {
        private readonly HiBlogsDbContext db;
        public HomeController(HiBlogsDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var blogInfos = db.Blogs.OrderByDescending(t => t.OldPublishTiem)
                   .Select(t => new BlogInfoViewModel
                   {
                       Title = t.Title,
                       Id = t.Id
                   }).ToList();
            ViewBag.userNickname = string.IsNullOrWhiteSpace(HiSession.UserNickname) ? HiSession.UsreName : HiSession.UserNickname;
            return View(blogInfos);
        }

        public IActionResult Blog(int Id)
        {
            var blog = db.Blogs.Where(t => t.Id == Id)
                .Select(t => new BlogViewModel
                {
                    Title = t.Title,
                    Content = t.Content
                }).FirstOrDefault();
            return View(blog);
        }
    }
}
