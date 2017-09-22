using System;
using System.Collections.Generic;
using System.Text;

namespace HiBlogs.Infrastructure
{
    public static class StringExtend
    {
        /// <summary>
        /// 替换所有的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldValues"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceAll(this string str,string[] oldValues, string newValue)
        {
            foreach (var oldValue in oldValues)
            {
                str = str.Replace(oldValue, newValue);
            }
            return str;
        }
    }
}
