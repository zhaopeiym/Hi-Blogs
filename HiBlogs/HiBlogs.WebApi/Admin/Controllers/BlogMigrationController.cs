using BlogsMigration;
using HiBlogs.Core.Entities;
using HiBlogs.Definitions;
using HiBlogs.EntityFramework.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HiBlogs.WebApi.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [Authorize(Roles = "Administrator")]
    public class BlogMigrationController : Controller
    {
        private readonly HiBlogsDbContext db;
        private readonly UserManager<User> userManager;


        public BlogMigrationController(HiBlogsDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        /// <summary>
        /// 迁移博客
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> MigrationByUserName(string userName)
        {

            BlogMigrationcs blogMigration = new BlogMigrationcs();
            var userId = await db.Users.Where(t => t.UserName == userName).Select(t => t.Id).FirstOrDefaultAsync();
            if (userId <= 0)
            {
                var readomString = "111vvv" + new Random().Next();
                var user = new User
                {
                    UserName = userName,
                    Email = readomString + "@temo.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await userManager.CreateAsync(user, readomString);
                await userManager.AddToRoleAsync(user, RoleNames.Average);
                userId = user.Id;
            }
            var blogs = await blogMigration.CnblogsMigrationToHiBlogAsync(userName, true);
            foreach (var blog in blogs)
            {
                db.Blogs.Add(new Blog()
                {
                    UserId = userId,
                    Content = blog.Content,
                    Title = blog.Title,
                    OldPublishTiem = blog.OldPublishTiem
                });
            }
            return await db.SaveChangesAsync();
        }
    }
}
