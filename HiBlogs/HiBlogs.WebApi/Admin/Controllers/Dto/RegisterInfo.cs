using System;
using System.Collections.Generic;
using System.Text;
using HiBlogs.Core.Entities;

namespace HiBlogs.WebApi.Api.Controllers.Dto
{
    public class RegisterInfo
    {
        public User User { get; set; }
        public string Passwod { get; set; }
    }
}
