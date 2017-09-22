using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiBlogs.Core.Entities
{
    public class Blog : EntityBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadNumber { get; set; } = 0;

        /// <summary>
        /// 评论数量
        /// </summary>
        public int RemarksNumber { get; set; } = 0;

        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool IsHome { get; set; } = false;

        /// <summary>
        /// 是否显示在个人主页
        /// </summary>
        public bool IsMyHome { get; set; } = true;

        /// <summary>
        /// 是否是转发文章
        /// </summary>
        public bool IsForwarding { get; set; } = false;

        /// <summary>
        /// 原链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 原博客发表时间
        /// </summary>
        public string OldPublishTiem { get; set; }
        

        /// <summary>
        ///  用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary>       
        [ForeignKey("BlogId")]
        public virtual ICollection<BlogBlogTag> BlogBlogTags { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary> 
        [ForeignKey("BlogId")]
        public virtual ICollection<BlogBlogType> BlogBlogTypes { get; set; }

        /// <summary>
        /// 博客评论
        /// </summary>      
        [ForeignKey("BlogId")]
        public virtual ICollection<Remark> Remarks { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
