using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HiBlogs.Application
{
    public class ApplicationModule
    {
        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
