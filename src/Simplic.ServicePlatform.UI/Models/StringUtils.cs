﻿using System.Text;

namespace Simplic.ServicePlatform.UI
{
    public static class StringUtils
    {

        /// <summary>
        /// Formats the size of given string. <br/>
        /// If size exceeds string size it will be filled with whitespaces.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FormatStringSize(string s, int size)
        {
            if (size <= 0) return string.Empty;
            var fstring = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                if (i < s.Length)
                {
                    fstring.Append(s[i]);
                    continue;
                }
                fstring.Append(" ");
            }
            return fstring.ToString();
        }
    }
}
