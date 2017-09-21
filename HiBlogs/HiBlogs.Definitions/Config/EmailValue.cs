using HiBlogs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Definitions
{
    public static class EmailConfig
    {
        public static string From { get; } = ConfigurationManager.GetSection("Mial:From");
        public static string Passwod { get; } = ConfigurationManager.GetSection("Mial:Host");
        public static string Host { get; } = ConfigurationManager.GetSection("Mial:Passwod");
    }
}
