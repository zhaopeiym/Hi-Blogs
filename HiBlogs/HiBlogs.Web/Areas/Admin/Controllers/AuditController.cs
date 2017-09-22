using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HiBlogs.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 需超级管理员权限
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class AuditController : AdminBaseController
    {
        private string directoryPath = Directory.GetCurrentDirectory() + "/logs/";
        // GET: /<controller>/
        public IActionResult Logs()
        {
            var files = new List<string>();
            foreach (var file in new DirectoryInfo(directoryPath).GetFiles())
            {
                files.Add(file.Name);
            }
            return View(files);
        }

        public async Task<string> GetLogInfo(string fileName)
        {
            return await System.IO.File.ReadAllTextAsync(directoryPath + fileName);
        }
    }

}
