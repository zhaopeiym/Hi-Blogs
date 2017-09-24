using HiBlogs.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Definitions
{
    public class SqlConnection
    {
        public static string MySqlConnection { get; } = ConfigurationManager.Configuration.GetConnectionString("MySqlConnection");
    }
}
