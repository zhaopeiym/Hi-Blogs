using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core.AuditedEntitys
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public TKey Id { get; set; }
    }
}
