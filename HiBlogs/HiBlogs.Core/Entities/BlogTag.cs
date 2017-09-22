using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    /// 博客标签
    /// </summary>
    public class BlogTag : CreationAuditedEntity<int>
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 属于这个用户的标签
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// 关联的博客
        /// </summary>
        [ForeignKey("TagId")]
        public virtual ICollection<BlogBlogTag> BlogBlogTags { get; set; }

    }
}
