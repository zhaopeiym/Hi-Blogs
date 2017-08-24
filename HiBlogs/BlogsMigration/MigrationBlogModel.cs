using System;
using System.Collections.Generic;
using System.Text;

namespace BlogsMigration
{
    /// <summary>
    /// 要迁移的博客模型
    /// </summary>
    public class MigrationBlogModel
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
        /// 原链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 原博客发表时间
        /// </summary>
        public string OldPublishTiem { get; set; }
    }
}
