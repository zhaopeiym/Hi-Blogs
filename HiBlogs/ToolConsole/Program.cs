using BlogsMigration;
using HiBlogs.EntityFramework.EntityFramework;
using System;
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
                BlogMigrationcs blogMigration = new BlogMigrationcs();
                var blogs = await blogMigration.CnblogsMigrationToHiBlogAsync("zhaopei", true);
                foreach (var blog in blogs)
                {
                    Console.WriteLine("正在迁移" + blog.Url);
                    db.Blogs.Add(new HiBlogs.Core.Blog()
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
