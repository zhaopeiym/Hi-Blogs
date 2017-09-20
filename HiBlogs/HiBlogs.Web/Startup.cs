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
            #region 自动注入
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

            #endregion

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

            services.AddMvc();

            //注意：一定要加 sslmode=none 
            connection = Configuration.GetConnectionString("MySqlConnection");
            EmailValue.emailFrom = Configuration.GetValue<string>("emailFrom");
            EmailValue.emailPasswod = Configuration.GetValue<string>("emailPasswod");
            EmailValue.emailHost = Configuration.GetValue<string>("emailHost");
            services.AddDbContext<HiBlogsDbContext>(options => options.UseMySql(connection));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            //InitDBData();
        }

        #region 初始化数据库数据
        /// <summary>
        /// 初始化数据库数据
        /// </summary>
        public void InitDBData()
        {
            using (HiBlogsDbContext _dbContext = new HiBlogsDbContext(connection))
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
