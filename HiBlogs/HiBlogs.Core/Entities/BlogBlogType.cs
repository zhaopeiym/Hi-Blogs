using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    /// 博客和类型中间表
    /// </summary>
    public class BlogBlogType :Entity<int>
    {
        public int BlogId { get; set; }
        public int TypeId { get; set; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }
        [ForeignKey("TypeId")]

        public virtual BlogType BlogType { get; set; }
    }
}
