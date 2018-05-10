using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;



namespace ItemDoc.Framework.Text
{
    public static class StringUtility
    {
        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] data)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 转为MYSQL IN 查寻格式
        /// </summary>
        /// <returns></returns>
        public static string ToMySqlInString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            var strs = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(",", strs.Select(c => "'" + c + "'").ToArray());
        }

        public static string RandomString(int size)
        {
            int number;
            char code;
            string randomStr = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                randomStr += code.ToString();
            }
            return randomStr;
        }



        #region 将HTML标签转换
        /// <summary>
        /// 将HTML标签转换
        /// </summary>
        /// <param name="html"></param>
        public static string EscapeHtml(string html)
        {
            html = html.Replace("&amp;", "&");
            html = html.Replace("\\\\", "\\");
            html = html.Replace("\"", "\\\"");
            html = html.Replace("&lt;", "<");
            html = html.Replace("&gt;", ">");
            return html;
        }
        #endregion

        #region 判断List是否为空
        public static bool IsNullOrEmpty<T>(T[] list)
        {
            return list == null || list.Length <= 0;
        }
        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            return list == null || list.Count <= 0;
        }
        public static bool IsNullOrEmpty<t1,t2>(Dictionary<t1,t2> list)
        {
            return list == null || list.Count <= 0;
        }
        #endregion

        #region 01.截取文本中指定段落
        /// <summary>
        /// 截取文本中指定段落
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <param name="start">起始匹配串</param>
        /// <param name="end">结束匹配串</param>
        /// <returns></returns>
        public static string Intercept(string text, string start, string end)
        {
            string segment = null;
            int startPos = text.IndexOf(start);
            if (startPos != -1)
            {
                startPos += start.Length;
                if (end == null)
                {
                    segment = text.Substring(startPos);
                }
                else
                {
                    int endPos = text.IndexOf(end, startPos);
                    segment = endPos != -1 ? text.Substring(startPos, endPos - startPos) : text.Substring(startPos);
                }
            }
            return segment;
        }
        #endregion

    }
}
