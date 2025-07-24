using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DNC.CM
{
    public static class StringHelper
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2;
        }

        public static string ToReplaceKeywork(string keywork)
        {
            keywork = keywork.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                keywork = keywork.Replace(((char)i).ToString(), " ");
            }
            keywork = keywork.Replace(".", ",");
            keywork = keywork.Replace(";", ",");
            keywork = keywork.Replace(":", ",");
            keywork = keywork.Replace("  ", ",");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = keywork.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('Đ', 'đ');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", ",").ToLower();
            }
            return str2;
        }
        public static string ToReplaceTitle(string title)
        {
            title = title.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                title = title.Replace(((char)i).ToString(), " ");
            }
           
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = title.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('Đ', 'đ');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = "/"+str2.Replace("--", "-").ToLower();
            }
            return str2;
        }
    }
}
