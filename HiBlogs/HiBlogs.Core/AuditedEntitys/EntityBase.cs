using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Core.AuditedEntitys
{
    /// <summary>
    /// 包含了 创建、修改、删除 等相关信息
    /// </summary>
    public class EntityBase : DeleterAuditedEntity<int>
    {
    }
}
