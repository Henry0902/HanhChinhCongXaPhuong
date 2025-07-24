using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DNC.CM
{
    public class CommonFunction
    {

        public static DateTime minDate = DateTime.Parse("01/01/1900");
        public static DateTime maxDate = DateTime.Parse("01/01/3000");
        /// <summary>
        /// HƯNG PV - Hàm định dạng ngày tháng
        /// </summary>
        public static string FormatDate(DateTime d)
        {
            return d.ToString("HH:mm:ss - dd/MM/yyyy");
        }

        public static string FormatShortDate(DateTime d)
        {
            return d.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// HƯNG PV - Hàm cắt chuỗi
        /// </summary>
        public static string FormatText(string sStr, int number = 100)
        {
            sStr = sStr.Replace('_', ' ');
            sStr = sStr.Replace("%20", " ");
            if (number >= sStr.Length)
            {
                return sStr;
            }
            int last = sStr.LastIndexOf(' ', number);
            if (last > 0)
            {
                return sStr.Substring(0, last).Replace('_', ' ') + "...";
            }
            else
            {
                string sResul = string.Empty;
                if (sStr.Length > number)
                {
                    sResul = sStr.Substring(0, number - 1);
                }
                else
                {
                    sResul = sStr;
                }
                return sResul;
            }
        }

        /// <summary>
        /// HƯNG PV - Chuyển chuỗi Tiếng Việt có dấu thành không có dấu
        /// </summary>
        public static string ConvertTiengVietCoDauThanhKhongDauV1(string sTiengVietCoDau)
        {
            //---------------------------------a^
            sTiengVietCoDau = sTiengVietCoDau.Replace("ấ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ầ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẩ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẫ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ậ", "a");
            //---------------------------------A^
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ấ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ầ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẩ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẫ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ậ", "A");
            //---------------------------------a(
            sTiengVietCoDau = sTiengVietCoDau.Replace("ắ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ằ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẳ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẵ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ặ", "a");
            //---------------------------------A(
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ắ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ằ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẳ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẵ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ặ", "A");
            //---------------------------------a
            sTiengVietCoDau = sTiengVietCoDau.Replace("á", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("à", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ả", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ã", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ạ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("â", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ă", "a");
            //---------------------------------A
            sTiengVietCoDau = sTiengVietCoDau.Replace("Á", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("À", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ả", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ã", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ạ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Â", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ă", "A");
            //---------------------------------e^
            sTiengVietCoDau = sTiengVietCoDau.Replace("ế", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ề", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ể", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ễ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ệ", "e");
            //---------------------------------E^
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ế", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ề", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ể", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ễ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ệ", "E");
            //---------------------------------e
            sTiengVietCoDau = sTiengVietCoDau.Replace("é", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("è", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẻ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẽ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẹ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ê", "e");
            //---------------------------------E
            sTiengVietCoDau = sTiengVietCoDau.Replace("É", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("È", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẻ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẽ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẹ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ê", "E");
            //---------------------------------i
            sTiengVietCoDau = sTiengVietCoDau.Replace("í", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ì", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỉ", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ĩ", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ị", "i");
            //---------------------------------I
            sTiengVietCoDau = sTiengVietCoDau.Replace("Í", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ì", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỉ", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ĩ", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ị", "I");
            //---------------------------------o^
            sTiengVietCoDau = sTiengVietCoDau.Replace("ố", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ồ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ổ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỗ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ộ", "o");
            //---------------------------------O^
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ố", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ồ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ổ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ô", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ộ", "O");
            //---------------------------------o*
            sTiengVietCoDau = sTiengVietCoDau.Replace("ớ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ờ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ở", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỡ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ợ", "o");
            //---------------------------------O*
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ớ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ờ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ở", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỡ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ợ", "O");
            //---------------------------------u*
            sTiengVietCoDau = sTiengVietCoDau.Replace("ứ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ừ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ử", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ữ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ự", "u");
            //---------------------------------U*
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ứ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ừ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ử", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ữ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ự", "U");
            //---------------------------------y
            sTiengVietCoDau = sTiengVietCoDau.Replace("ý", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỳ", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỷ", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỹ", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỵ", "y");
            //---------------------------------Y
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ý", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỳ", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỷ", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỹ", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỵ", "Y");
            //---------------------------------DD
            sTiengVietCoDau = sTiengVietCoDau.Replace("Đ", "D");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Đ", "D");
            sTiengVietCoDau = sTiengVietCoDau.Replace("đ", "d");
            //---------------------------------o
            sTiengVietCoDau = sTiengVietCoDau.Replace("ó", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ò", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỏ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("õ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ọ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ô", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ơ", "o");
            //---------------------------------O
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ó", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ò", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỏ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Õ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ọ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ô", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ơ", "O");
            //---------------------------------u
            sTiengVietCoDau = sTiengVietCoDau.Replace("ú", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ù", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ủ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ũ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ụ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ư", "u");
            //---------------------------------U
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ú", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ù", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ủ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ũ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ụ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ư", "U");
            //--------------------------------- 
            sTiengVietCoDau = sTiengVietCoDau.Trim();

            //Thay thế dấu trắng bằng - để truyền trên url
            sTiengVietCoDau = sTiengVietCoDau.Replace("  ", " ");
            sTiengVietCoDau = sTiengVietCoDau.Replace(" ", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace("\"", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace("(", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace(")", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace(":", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace(",", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace("?", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace(".", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace("--", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace("/", "-");
            sTiengVietCoDau = Regex.Replace(sTiengVietCoDau, "[\"|:|?|,|.|--]", "-");
            sTiengVietCoDau = sTiengVietCoDau.Replace("--", "-");

            return sTiengVietCoDau;
        }
        /// <summary>
        /// Chuyển chuỗi Tiếng Việt có dấu thành không có dấu
        /// </summary>
        public static string ConvertTiengVietCoDauThanhKhongDau(string sTiengVietCoDau)
        {
            //---------------------------------a^
            sTiengVietCoDau = sTiengVietCoDau.Replace("ấ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ầ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẩ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẫ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ậ", "a");
            //---------------------------------A^
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ấ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ầ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẩ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẫ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ậ", "A");
            //---------------------------------a(
            sTiengVietCoDau = sTiengVietCoDau.Replace("ắ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ằ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẳ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẵ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ặ", "a");
            //---------------------------------A(
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ắ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ằ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẳ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẵ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ặ", "A");
            //---------------------------------a
            sTiengVietCoDau = sTiengVietCoDau.Replace("á", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("à", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ả", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ã", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ạ", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("â", "a");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ă", "a");
            //---------------------------------A
            sTiengVietCoDau = sTiengVietCoDau.Replace("Á", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("À", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ả", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ã", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ạ", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Â", "A");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ă", "A");
            //---------------------------------e^
            sTiengVietCoDau = sTiengVietCoDau.Replace("ế", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ề", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ể", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ễ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ệ", "e");
            //---------------------------------E^
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ế", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ề", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ể", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ễ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ệ", "E");
            //---------------------------------e
            sTiengVietCoDau = sTiengVietCoDau.Replace("é", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("è", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẻ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẽ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ẹ", "e");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ê", "e");
            //---------------------------------E
            sTiengVietCoDau = sTiengVietCoDau.Replace("É", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("È", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẻ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẽ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ẹ", "E");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ê", "E");
            //---------------------------------i
            sTiengVietCoDau = sTiengVietCoDau.Replace("í", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ì", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỉ", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ĩ", "i");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ị", "i");
            //---------------------------------I
            sTiengVietCoDau = sTiengVietCoDau.Replace("Í", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ì", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỉ", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ĩ", "I");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ị", "I");
            //---------------------------------o^
            sTiengVietCoDau = sTiengVietCoDau.Replace("ố", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ồ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ổ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỗ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ộ", "o");
            //---------------------------------O^
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ố", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ồ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ổ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ô", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ộ", "O");
            //---------------------------------o*
            sTiengVietCoDau = sTiengVietCoDau.Replace("ớ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ờ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ở", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỡ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ợ", "o");
            //---------------------------------O*
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ớ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ờ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ở", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỡ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ợ", "O");
            //---------------------------------u*
            sTiengVietCoDau = sTiengVietCoDau.Replace("ứ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ừ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ử", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ữ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ự", "u");
            //---------------------------------U*
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ứ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ừ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ử", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ữ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ự", "U");
            //---------------------------------y
            sTiengVietCoDau = sTiengVietCoDau.Replace("ý", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỳ", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỷ", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỹ", "y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỵ", "y");
            //---------------------------------Y
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ý", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỳ", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỷ", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỹ", "Y");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỵ", "Y");
            //---------------------------------DD
            sTiengVietCoDau = sTiengVietCoDau.Replace("Đ", "D");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Đ", "D");
            sTiengVietCoDau = sTiengVietCoDau.Replace("đ", "d");
            //---------------------------------o
            sTiengVietCoDau = sTiengVietCoDau.Replace("ó", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ò", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ỏ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("õ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ọ", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ô", "o");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ơ", "o");
            //---------------------------------O
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ó", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ò", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ỏ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Õ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ọ", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ô", "O");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ơ", "O");
            //---------------------------------u
            sTiengVietCoDau = sTiengVietCoDau.Replace("ú", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ù", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ủ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ũ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ụ", "u");
            sTiengVietCoDau = sTiengVietCoDau.Replace("ư", "u");
            //---------------------------------U
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ú", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ù", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ủ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ũ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ụ", "U");
            sTiengVietCoDau = sTiengVietCoDau.Replace("Ư", "U");
            //--------------------------------- 
            sTiengVietCoDau = sTiengVietCoDau.Trim();
            sTiengVietCoDau = sTiengVietCoDau.Replace("  ", " ");

            return sTiengVietCoDau;
        }
        /// <summary>
        /// HƯNG PV - Hàm clear dữ liệu
        /// </summary>
        public static string STRING_ClearSign(string _text)
        {
            Regex regex = new Regex("\\\\p{IsCombiningDiacriticalMarks}+");
            string temp = _text.Normalize(NormalizationForm.FormD);
            _text = regex.Replace(temp, string.Empty).Replace("\\u0111", "d").Replace("\\u0110", "D");
            return _text;
        }
        /// <summary>
        /// HƯNG PV - Định dạng kiểu dữ liệu số đếm
        /// </summary>
        public static string NUMBER_Format(object obj, bool zeroIsEmptyString = true)
        {
            try
            {
                string result = string.Format("{0:#,##}", double.Parse(obj.ToString()));
                if (string.IsNullOrEmpty(result.Trim()))
                {
                    result = "0";
                }
                result = result.Replace(",", ".");
                if (zeroIsEmptyString & result == "0")
                {
                    return "";
                }
                return result;

            }
            catch (Exception ex)
            {
            }
            return "";
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, "^([0-9a-z]+[-._+&])*[0-9a-z]+@([-0-9a-z]+[.])+[a-z]{2,6}$", RegexOptions.IgnoreCase);
        }

        public static bool IsValidURL(string url)
        {
            return Regex.IsMatch(url, "^(http|https|ftp)\\://[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\\-\\._\\?\\,\\'/\\\\\\+&%\\$#\\=~])*[^\\.\\,\\)\\(\\s]$");
        }
        public static bool IsValidInt(string val)
        {
            return Regex.IsMatch(val, "^[1-9]\\d*\\.?[0]*$");
        }


        /// <returns>trả về 1 chuỗi string đã xóa thẻ</returns>
        public static string DecodeContent(string html)
        {
            const string regex = "<.*?>";//patern regex
            if (!string.IsNullOrEmpty(html))
            {
                html = HttpUtility.HtmlDecode(html);
                return Regex.Replace(html, regex, string.Empty);
            }
            return string.Empty;
        }

        /// <summary>
        /// VietDQ - Tạo Files trên Server với các Files được upload lên hệ thống
        /// </summary>
        /// 

        public static string createFile(string Paths, HttpPostedFileBase files)
        {
            try
            {
                var fileName = Path.GetFileName(files.FileName);
                files.SaveAs(Path.Combine(Paths, fileName));
                return fileName;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// DAITC - 
        /// </summary>
        /// <param name="Paths"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string createFile_Ex(string Paths, string extra ,HttpPostedFileBase files)
        {
            try
            {
                var fileName = extra + Path.GetFileName(files.FileName);
                files.SaveAs(Path.Combine(Paths, fileName));
                return fileName;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// VietDQ - Xóa Files trong một thư mục trên Server
        /// </summary>
        /// param name="type" 1.xóa toàn bộ file theo đường dẫn thư mục , 2. xóa toàn bộ thư mục con thuộc đường dẫn , 3.file theo đường dẫn(đường dẫn bao gồm cả tên)
        public static bool deleteFile(string paths, int type)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(paths);
                switch (type)
                {
                    case 1:
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        break;
                    case 2:
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                        break;
                    case 3:
                        File.Delete(paths);
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        ///<summary>
        ///VietDQ - Di chuyển File trên Server
        ///</summary>
        ///<param name="fromPaths" đường dẫn gốc></param>
        ///<param name="toPath" đường dẫn muốn di chuyển đến></param>
        ///<param name="type" kiểu di chuyển "1" chuyển File "2" chuyển thư mục></param>
        public static bool moveFile(string fromPaths, string toPath, int type)
        {
            try
            {
                switch (type)
                {
                    case 1:
                        File.Move(fromPaths, toPath);
                        break;
                    case 2:
                        Directory.Move(fromPaths, toPath);
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
