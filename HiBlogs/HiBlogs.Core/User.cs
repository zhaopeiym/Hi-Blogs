using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core
{
    public class User: EntityBase
    {
        public string Name { get; set; }
        public string Passwod { get; set; }
    }
}
