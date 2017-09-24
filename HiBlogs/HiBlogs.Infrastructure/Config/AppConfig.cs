using HiBlogs.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Definitions.Config
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// mysql 数据库连接
        /// </summary>
        public static string MySqlConnection { get; } = ConfigurationManager.Configuration.GetConnectionString("MySqlConnection");

        /// <summary>
        /// redis 连接
        /// </summary>
        public static string RedisConnection { get; } = ConfigurationManager.GetSection("RedisConnection");

        /// <summary>
        /// 加密解密键
        /// </summary>
        public static string DESKey { get; } = ConfigurationManager.GetSection("DESKey");
    }
}
