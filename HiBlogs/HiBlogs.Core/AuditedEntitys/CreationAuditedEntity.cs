using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core.AuditedEntitys
{
    /// <summary>
    /// 创建时间和创建人
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class CreationAuditedEntity<TKey> : Entity<TKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual int CreatorUserId { get; set; }

    }
}
