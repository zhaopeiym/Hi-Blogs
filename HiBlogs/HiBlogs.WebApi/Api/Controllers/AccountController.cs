using HiBlogs.Core.Authorization;
using HiBlogs.Core.Entities;
using HiBlogs.Definitions;
using HiBlogs.EntityFramework.EntityFramework;
using HiBlogs.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiBlogs.WebApi.Api.Controllers
{

    [Route("api/[controller]/[Action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly HiBlogsDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(SignInManager<User> signInManager,
            HiBlogsDbContext hiBlogsDbContext,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = hiBlogsDbContext;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passwod">密码</param>
        /// <param name="rememberMe">是否记住密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Login(string userName, string passwod, bool rememberMe, string returnUrl = null)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, passwod, rememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return returnUrl;
            }
            return "false";
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task InitData()
        {
            if (!_dbContext.Users.Any())
            {
                var roleAdministrator = new Role { Name = RoleNames.Administrator };
                var roleAdmin = new Role { Name = RoleNames.Admin };
                var roleAverage = new Role { Name = RoleNames.Average };
                await _roleManager.CreateAsync(roleAdministrator);
                await _roleManager.CreateAsync(roleAdmin);
                await _roleManager.CreateAsync(roleAverage);
                var user = new User { UserName = "Administrator", Email = "Administrator@haojima.net" };
                await _userManager.CreateAsync(user, "123qwe");
                await _userManager.AddToRoleAsync(user, RoleNames.Administrator);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task LogOff()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwod"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IdentityResult> Register(string userName, string passwod, string email)
        {
            var hasUser = _userManager.Users.Where(t => t.UserName == userName).Any();
            if (hasUser)
            {
                var result = IdentityResult.Failed(new IdentityError()
                {
                    Description = "已存在此用户"
                });
                return result;
            }
            EmailHelper emailHelper = new EmailHelper()
            {
                mailPwd = EmailValue.emailPasswod,
                host = EmailValue.emailHost,
                mailFrom = EmailValue.emailFrom,
                mailSubject = "欢迎您注册 嗨-博客",
                mailBody = EmailHelper.tempBody(userName, " 您的激活码：" + "123"),
                mailToArray = new string[] { email }
            };
            emailHelper.Send();

            var user = new User { UserName = userName, Email = email };
            await _userManager.CreateAsync(user, passwod);
            return await _userManager.AddToRoleAsync(user, RoleNames.Average);
        }

        /// <summary>
        /// 根据链接验证注册信息（发送过去的邮件）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> CheckRegister(string url)
        {

            return true;
        }
    }
}
