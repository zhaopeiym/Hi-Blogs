using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core.AuditedEntitys
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
