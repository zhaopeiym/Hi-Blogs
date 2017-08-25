using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core.AuditedEntitys
{
    public abstract class FullAuditedEntity<TKey> : ModifyAuditedEntity<TKey>
    {
        /// <summary>
        /// 是否被删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        public virtual int? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
}
