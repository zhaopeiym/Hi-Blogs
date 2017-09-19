using HiBlogs.Core.Entities;
using HiBlogs.Definitions;
using HiBlogs.EntityFramework.EntityFramework;
using HiBlogs.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Talk.OAuthClient;

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

        private IOAuthClient GetOAuthClient(AuthType authType)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string callbackUrl = string.Empty;

            if (authType == AuthType.QQ)
            {
                clientId = ConfigurationManager.GetSection("OAuthClient:TencentQQClient:ClientId");
                clientSecret = ConfigurationManager.GetSection("OAuthClient:TencentQQClient:ClientSecret");
                callbackUrl = ConfigurationManager.GetSection("OAuthClient:TencentQQClient:CallbackUrl");
            }
            else if (authType == AuthType.Sina)
            {
                clientId = ConfigurationManager.GetSection("OAuthClient:SinaClient:ClientId");
                clientSecret = ConfigurationManager.GetSection("OAuthClient:SinaClient:ClientSecret");
                callbackUrl = ConfigurationManager.GetSection("OAuthClient:SinaClient:CallbackUrl");
            }
            return OAuthClientFactory.GetOAuthClient(clientId, clientSecret, callbackUrl, authType);
        }

        /// <summary>
        /// 获取社交帐号认证地址[QQ]
        /// </summary>
        /// <returns></returns>
        public string GetOAuthQQUrl()
        {
            return GetOAuthClient(AuthType.QQ).GetAuthUrl();
        }
        /// <summary>
        /// 获取社交帐号认证地址[新浪]
        /// </summary>
        /// <returns></returns>
        public string GetOAuthSinaUrl()
        {
            return GetOAuthClient(AuthType.Sina).GetAuthUrl();
        }

        /// <summary>
        /// 获取社交登录用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<AccessTokenObject> GetOAuthUser(string code, string type)
        {
            AuthType authType = AuthType.QQ;
            switch (type)
            {
                case "qq":
                    authType = AuthType.QQ;
                    break;
                case "sina":
                    authType = AuthType.Sina;
                    break;
            }
            var client = GetOAuthClient(authType);
            var accessToken = await client.GetAccessToken(code);

            var user = await _dbContext.Users.Where(t => t.OpenId == accessToken.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                var accessUser = await client.GetUserInfo(accessToken);
                user = new User
                {
                    UserName = DateTime.Now.Ticks.ToString(),
                    OpenId = accessToken.UserId,
                    Email = DateTime.Now.Ticks.ToString() + "@temp.com",
                    Nickname = accessUser.Name
                };
                var temp = await _userManager.CreateAsync(user, accessUser.Id.ToLower() + "temp");
                await _userManager.AddToRoleAsync(user, RoleNames.Average);
            }
            await _signInManager.SignInAsync(user, true);

            return accessToken;
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
