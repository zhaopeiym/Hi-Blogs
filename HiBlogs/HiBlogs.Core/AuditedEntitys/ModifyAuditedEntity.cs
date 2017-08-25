using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core.AuditedEntitys
{
    public abstract class ModifyAuditedEntity<TKey>: CreationAuditedEntity<TKey>
    {
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public virtual long? LastModifierUserId { get; set; }
    }
}
