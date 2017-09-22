using HiBlogs.Core.AuditedEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiBlogs.Core.Entities
{
    /// <summary>
    /// 回复评论（也就是码楼的第二层）
    /// </summary>
    public class ReplyRemark: EntityBase
    {
        /// <summary>
        /// 回复的评论id
        /// </summary>
        public int RemarkId { get; set; }

        /// <summary>
        /// 回复给用户
        /// </summary>      
        public int? ReplyToUserId { get; set; }

        /// <summary>
        /// 回复给用户的名字（为了兼容迁移博客）
        /// </summary>
        public string ReplyToUserName { get; set; }

        /// <summary>
        /// 备用（以后可能显示地址）
        /// </summary>
        public string IP { get; set; }


        [ForeignKey("RemarkId")]

        public virtual Remark Remark { get; set; }

        /// <summary>
        /// 回复给的用户
        /// </summary>
        [ForeignKey("ReplyToUserId")]

        public virtual User ReplyToUser { get; set; }
    }
}
