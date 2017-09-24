using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Infrastructure.Models
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ReturnResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;//默认是成功

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
      
        /// <summary>
        /// 异常消息
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
