using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    /// 博客类型
    /// </summary>
    public class BlogType: CreationAuditedEntity<int>
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }      

        /// <summary>
        /// 属于这个用户的类型
        /// </summary>
        [ForeignKey("CreatorUserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// 关联的博客
        /// </summary>
        [ForeignKey("TypeId")]
        public virtual ICollection<BlogBlogType> BlogBlogTypes { get; set; }
    }
}
