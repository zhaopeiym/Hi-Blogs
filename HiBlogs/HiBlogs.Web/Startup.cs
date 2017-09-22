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
using HiBlogs.Definitions.Config;
using HiBlogs.Application;

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
            var assemblyWeb = Assembly.GetExecutingAssembly();
            var assemblyApplication = ApplicationModule.GetAssembly();

            // 自动注入
            AutoInjection(services, assemblyApplication);
            AutoInjection(services, assemblyWeb);
            // 日志配置
            LogConfig();
            //redis  http://www.cnblogs.com/savorboard/p/5592948.html
            services.AddDistributedRedisCache(o => o.Configuration = AppConfig.RedisConnection);
            //session （session存放在redis）
            services.AddSession();
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
            services.AddDbContext<HiBlogsDbContext>(options => options.UseMySql(AppConfig.MySqlConnection));
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
            app.UseSession();
            RouteConfig(app);
        }

        #region 自定义设置

        /// <summary>
        /// 自动注入
        /// </summary>
        private void AutoInjection(IServiceCollection services, Assembly assembly)
        {
            //获取标记了ISingletonDependency接口的接口
            var singletonInterfaceDependency = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(ISingletonDependency)))
                    .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".ISingletonDependency")))
                    .ToList();
            //获取标记了ISingletonDependency接口的类
            var singletonTypeDependency = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(ISingletonDependency)))
                    .ToList();

            //获取标记了ITransientDependency接口的接口
            var transientInterfaceDependency = assembly.GetTypes()
                   .Where(t => t.GetInterfaces().Contains(typeof(ITransientDependency)))
                   .SelectMany(t => t.GetInterfaces().Where(f => !f.FullName.Contains(".ITransientDependency")))
                   .ToList();
            //获取标记了ITransientDependency接口的类
            var transientTypeDependency = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(ITransientDependency)))
                    .ToList();
            
            //自动注入标记了 ISingletonDependency接口的 接口
            foreach (var interfaceName in singletonInterfaceDependency)
            {
                var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (type != null)
                    services.AddSingleton(interfaceName, type);
            }
            //自动注入标记了 ISingletonDependency接口的 类
            foreach (var type in singletonTypeDependency)
            {             
                services.AddSingleton(type, type);
            }

            //自动注入标记了 ITransientDependency接口的 接口
            foreach (var interfaceName in transientInterfaceDependency)
            {
                var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (type != null)
                    services.AddTransient(interfaceName, type);
            }
            //自动注入标记了 ITransientDependency接口的 类
            foreach (var type in transientTypeDependency)
            {
                services.AddTransient(type, type);
            }
        }

        /// <summary>
        /// 日志配置
        /// </summary>      
        private void LogConfig()
        {
            //nuget导入
            //Serilog.Extensions.Logging
            //Serilog.Sinks.RollingFile
            //Serilog.Sinks.Async
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

        private void RouteConfig(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "areaRoute",
                   template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                  name: "userBlog",
                  template: "{userName}/{blogId}.html/{controller=UserBlog}/{action=Blog}");
            });
        }

        #endregion

        #region 初始化数据库数据
        /// <summary>
        /// 初始化数据库数据
        /// </summary>
        public void InitDBData()
        {
            using (HiBlogsDbContext _dbContext = new HiBlogsDbContext(AppConfig.MySqlConnection))
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
