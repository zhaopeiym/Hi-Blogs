using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HiBlogs.EntityFramework.EntityFramework;
using Microsoft.EntityFrameworkCore;
using HiBlogs.Core.Entities;
using HiBlogs.Core.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using HiBlogs.Definitions;
using System.Reflection;
using HiBlogs.Definitions.Dependency;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;

namespace HiBlogs.Web
{
    public class Startup
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string connection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 自动注入
            AutoInjection(services);
            // 日志配置
            LogConfig();
            //Identity
            services.AddIdentity<User, Role>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };

            }).AddEntityFrameworkStores<HiBlogsDbContext>().AddDefaultTokenProviders();
            //修改默认登录、和退出链接
            //https://github.com/aspnet/Security/issues/1310            
            services.ConfigureApplicationCookie(identityOptionsCookies =>
            {
                identityOptionsCookies.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                identityOptionsCookies.LoginPath = "/Admin/Account/Login";
                //identityOptionsCookies.LogoutPath = "...";
            });
            //Mvc
            services.AddMvc();
            //数据库连接
            services.AddDbContext<HiBlogsDbContext>(options => options.UseMySql(SqlConnection.MySqlConnection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //注册Serilog日志框架
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "areaRoute",
                   template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        #region 自定义设置

        /// <summary>
        /// 自动注入
        /// </summary>
        private void AutoInjection(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var singletonDependency = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISingletonDependency)))
                    .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".ISingletonDependency")))
                    .ToList();

            var transientDependency = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITransientDependency)))
                   .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".ITransientDependency")))
                   .ToList();

            //自动注入标记了 ISingletonDependency接口的 接口
            foreach (var interfaceName in singletonDependency)
            {
                var obj = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (obj != null)
                    services.AddSingleton(interfaceName, obj);
            }

            //自动注入标记了 ITransientDependency接口的 接口
            foreach (var interfaceName in transientDependency)
            {
                var obj = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (obj != null)
                    services.AddTransient(interfaceName, obj);
            }
        }

        /// <summary>
        /// 日志配置
        /// </summary>      
        private void LogConfig()
        {
            Log.Logger = new LoggerConfiguration()
                                 .Enrich.FromLogContext()
                                 .MinimumLevel.Debug()
                                 .MinimumLevel.Override("System", LogEventLevel.Information)
                                 .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                 .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.Async(
                                     a => a.RollingFile("logs/log-{Date}-Debug.txt")
                                 ))
                                 .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.Async(
                                     a => a.RollingFile("logs/log-{Date}-Information.txt")
                                 ))
                                 .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.Async(
                                     a => a.RollingFile("logs/log-{Date}-Warning.txt")
                                 ))
                                 .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.Async(
                                     a => a.RollingFile("logs/log-{Date}-Error.txt")
                                 ))
                                 .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.Async(
                                     a => a.RollingFile("logs/log-{Date}-Fatal.txt")
                                 ))
                                 .CreateLogger();
        }

        #endregion

        #region 初始化数据库数据
        /// <summary>
        /// 初始化数据库数据
        /// </summary>
        public void InitDBData()
        {
            using (HiBlogsDbContext _dbContext = new HiBlogsDbContext(SqlConnection.MySqlConnection))
            {
                if (!_dbContext.Roles.Any())
                {
                    var roleAdmin = new Role { Name = "Administrator" };
                    var roleAverage = new Role { Name = "Average" };
                    var userAdministrator = new User() { UserName = "Administrator", Email = "123@123.com", };
                    var userbenny = new User() { UserName = "benny", Email = "benny@123.com", };
                    userAdministrator.PasswordHash = new PasswordHasher<User>().HashPassword(userAdministrator, "123qwe");
                    userbenny.PasswordHash = new PasswordHasher<User>().HashPassword(userbenny, "123qwe");

                    _dbContext.Roles.Add(roleAdmin);//添加角色
                    _dbContext.Roles.Add(roleAverage);//添加角色
                    _dbContext.Users.Add(userAdministrator);//添加用户 
                    _dbContext.Users.Add(userbenny);//添加用户 
                    _dbContext.SaveChanges();

                    //给用户添加角色
                    _dbContext.UserRoles.Add(new IdentityUserRole<int>() { RoleId = roleAdmin.Id, UserId = userAdministrator.Id });
                    _dbContext.UserRoles.Add(new IdentityUserRole<int>() { RoleId = roleAverage.Id, UserId = userbenny.Id });
                    _dbContext.SaveChanges();

                }
            }
        }
        #endregion
    }
}
