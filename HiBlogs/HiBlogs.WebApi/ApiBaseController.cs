using HiBlogs.EntityFramework.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.WebApi
{
    [Route("api/[controller]/[Action]")]
    public class ApiBaseController
    {
        protected HiBlogsDbContext Db { get; }
        public ApiBaseController(HiBlogsDbContext dbContext)
        {
            Db = dbContext;
        }
    }
}
