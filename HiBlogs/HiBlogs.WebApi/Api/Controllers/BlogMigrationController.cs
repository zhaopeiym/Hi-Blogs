using BlogsMigration;
using HiBlogs.EntityFramework.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HiBlogs.WebApi.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class BlogMigrationController : Controller
    {
        private readonly HiBlogsDbContext _db;

        public BlogMigrationController(HiBlogsDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<int> MigrationByUserName(string userName)
        {

            BlogMigrationcs blogMigration = new BlogMigrationcs();
            var blogs = await blogMigration.CnblogsMigrationToHiBlogAsync(userName, true);
            foreach (var blog in blogs)
            {
                _db.Blogs.Add(new HiBlogs.Core.Blog()
                {
                    Content = blog.Content,
                    Title = blog.Title,
                    OldPublishTiem = blog.OldPublishTiem
                });
            }
            return await _db.SaveChangesAsync();
        }
    }
}
