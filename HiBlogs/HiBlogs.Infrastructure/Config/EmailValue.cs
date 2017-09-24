using HiBlogs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Definitions
{
    public static class EmailConfig
    {
        public static string From { get; } = ConfigurationManager.GetSection("Mail:From");
        public static string Passwod { get; } = ConfigurationManager.GetSection("Mail:Passwod");
        public static string Host { get; } = ConfigurationManager.GetSection("Mail:Host");
    }
}
