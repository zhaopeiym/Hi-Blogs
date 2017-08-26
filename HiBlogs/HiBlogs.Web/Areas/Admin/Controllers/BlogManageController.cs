using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HiBlogs.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 博客管理
    /// </summary>
    public class BlogManageController : AdminBaseController
    {
        [Area("Admin")]
        // GET: /<controller>/
        public IActionResult Blogs()
        {
            return View();
        }
    }
}
