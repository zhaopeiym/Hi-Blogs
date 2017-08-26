using HiBlogs.EntityFramework.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiBlogs.WebApi.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class BlogManageController : Controller
    {
        private readonly HiBlogsDbContext _db;

        public BlogManageController(HiBlogsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 获取所有博客（DOTO:后续需要分页）
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetBlogs()
        {
            return await _db.Blogs.Select(t => new
            {
                t.Title,
                t.Url,
                t.Id
            }).ToListAsync();
        }
    }
}
