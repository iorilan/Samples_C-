using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wpfLab1
{
    public static class StrExtensenClass
    {
        public static string GetNormalFormat(this string s)
        {
            s = RemoveExtraSpace(s);
            string[] words = s.Split(' ');
            string ret = "";
            foreach (var word in words)
            {
                ret += StrFstChrUpr(word) + " ";
            }
            return ret;
        }

        public static string RemoveExtraSpace(this string s)
        {
            if (s == null || s.Length <= 1)
            {
                return s;
            }

            bool lastChrIsSpace = false;
            string ret = "";
            foreach (var chr in s)
            {
                if (chr == ' ')
                {
                    if (lastChrIsSpace)
                    {
                        continue;
                    }
                    else
                    {
                        lastChrIsSpace = true;
                        ret += chr;
                    }
                }
                else
                {
                    ret += chr;
                    lastChrIsSpace = false;
                }

            }
            return ret;
        }

        private static string StrFstChrUpr(string s)
        {
            if (s == null || s.Length < 1)
            {
                return s;
            }
           
            string lowerStr = s.ToLower().Remove(0, 1);
            string upperStr = Char.ToUpper(s[0]).ToString();
            return (upperStr + lowerStr);
        }
    }
}
