using BlogsMigration;
using System;
using System.Threading.Tasks;

namespace ToolConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.WriteLine("Hello World!");
        }

        public static async Task MainAsync()
        {
            BlogMigrationcs blogMigration = new BlogMigrationcs();
            await blogMigration.CnblogsMigrationToHiBlogAsync("zhaopei", true);
        }
    }
}
