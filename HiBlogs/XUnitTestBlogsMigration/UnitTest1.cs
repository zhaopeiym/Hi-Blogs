using BlogsMigration;
using System;
using Xunit;

namespace XUnitTestBlogsMigration
{
    public class UnitTest1
    {
        [Fact]
        public async System.Threading.Tasks.Task Test1Async()
        {
            BlogMigrationcs blog = new BlogMigrationcs();
            await blog.CnblogMigrationToHiBlogAsync("http://www.cnblogs.com/zhaopei/p/7397402.html", true);
        }
    }
}
