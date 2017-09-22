using BlogsMigration;
using HiBlogs.Core.Entities;
using HiBlogs.EntityFramework.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ToolConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        public static async Task MainAsync()
        {
            Console.WriteLine("请输入数据库连接字符串：");
            var connection = Console.ReadLine();
            Console.WriteLine("正在读取迁移博客...");
            using (HiBlogsDbContext db = new HiBlogsDbContext(connection))
            {
                var userName = "zhaopei";
                if (!db.Users.Where(t => t.UserName == userName).Any())
                    throw new Exception("不存在此用户");
                BlogMigrationcs blogMigration = new BlogMigrationcs();
                var blogs = await blogMigration.CnblogsMigrationToHiBlogAsync(userName, true);
                foreach (var blog in blogs)
                {
                    Console.WriteLine("正在迁移" + blog.Url);
                    db.Blogs.Add(new Blog()
                    {
                        Content = blog.Content,
                        Title = blog.Title,
                        OldPublishTiem = blog.OldPublishTiem
                    });
                }
                await db.SaveChangesAsync();
            }
            Console.WriteLine("全部迁移成功");
            Console.Read();
        }
    }
}
