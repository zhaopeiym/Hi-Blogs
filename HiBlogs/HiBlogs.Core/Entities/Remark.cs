using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    /// 评论
    /// </summary>
    public class Remark: EntityBase
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public bool Top { get; set; } = false;

        /// <summary>
        /// 记录IP（以后可能更加ip显示地区）
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 评论的博客
        /// </summary>
        public int BlogId { get; set; }

        [ForeignKey("BlogId")]
        /// <summary>
        /// 博客
        /// </summary>
        public virtual Blog Blog { get; set; }
    }
}
