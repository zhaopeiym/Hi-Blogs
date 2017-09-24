using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using HiBlogs.EntityFramework.EntityFramework;

namespace HiBlogs.WebApi.Controllers
{
    public class RemarkController : ApiBaseController
    {
        public RemarkController(HiBlogsDbContext dbContext)
            : base(dbContext)
        {
        }

        public void save(string mesg)
        {            
            Db.Remarks.Add(new Core.Entities.Remark()
            {
                BlogId = 1,
                Content = mesg,
                CreatorUserId = 1
            });
        }
    }
}
