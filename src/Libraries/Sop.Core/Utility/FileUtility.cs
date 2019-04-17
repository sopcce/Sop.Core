using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Sop.Data.Utility;

namespace Sop.Core.Utility
{
    public class FileUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDiskFilePath(string filePath)
        {
            var result = "";
            if (filePath.IndexOf(":\\", StringComparison.Ordinal) != -1 || filePath.IndexOf("\\\\", StringComparison.Ordinal) != -1)
                result = filePath;
            try
            {
                if (Config.IsHosted)
                {
                    filePath = filePath.Replace('/', System.IO.Path.DirectorySeparatorChar).Replace("~", "");
                    result = Combine(Directory.GetCurrentDirectory(), filePath);
                }
                else
                {
                    filePath = filePath.Replace('/', System.IO.Path.DirectorySeparatorChar).Replace("~", "");
                    result = Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath);

                }
                string fileName = Path.GetFileName(result);
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    string newResult = result?.Replace(fileName, "");
                    if (!Directory.Exists(newResult))
                    {
                        var info = string.IsNullOrWhiteSpace(fileName)
                            ? Directory.CreateDirectory(result)
                            : Directory.CreateDirectory(newResult);
                    }
                }
                else
                {
                    if (!Directory.Exists(result))
                    {
                        Directory.CreateDirectory(result);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = filePath;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            if (paths.Length == 0)
                throw new ArgumentException("please input path");
            var builder = new StringBuilder();
            var spliter = "\\";
            var firstPath = paths[0];
            if (firstPath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                spliter = "/";
            if (!firstPath.EndsWith(spliter))
                firstPath = firstPath + spliter;
            builder.Append(firstPath);
            for (var i = 1; i < paths.Length; i++)
            {
                var nextPath = paths[i];
                if (nextPath.StartsWith("/") || nextPath.StartsWith("\\"))
                    nextPath = nextPath.Substring(1);
                if (i != paths.Length - 1)
                {
                    if (nextPath.EndsWith("/") || nextPath.EndsWith("\\"))
                    {
                        nextPath = nextPath.Substring(0, nextPath.Length - 1) + spliter;
                    }
                    else
                    {
                        nextPath = nextPath + spliter;
                    }
                }

                builder.Append(nextPath);
            }

            return builder.ToString();
        }
        /// <summary>
        ///  /*"upload/image/{yyyy}{mm}{dd}/{time}{rand:6}", /* 上传保存路径,可以自定义保存路径和文件名格式 */
        ///  /* {rand:6} 会替换成随机数,后面的数字是随机数的位数 */
        ///  /* {time} 会替换成时间戳 */
        ///  /* {yyyy} 会替换成四位年份 */
        ///  /* {yy}   会替换成两位年份 */
        ///  /* {MM}   会替换成两位月份 */
        ///  /* {dd}   会替换成两位日期 */
        ///  /* {hh}  会替换成两位小时 */
        ///  /* {mm}  会替换成两位分钟 */
        ///  /* {ss}  会替换成两位秒 */
        ///  /* {ffff} 会替换成两位秒 */
        ///  /* 非法字符 \ : * ? " < > | */
        /// </summary>
        /// <param name="originFileName"></param>
        /// <param name="pathFormat"></param>
        /// <returns></returns>
        public static string Format(string originFileName, string pathFormat)
        {
            if (String.IsNullOrWhiteSpace(pathFormat))
            {
                pathFormat = "{rand:6}";
            }
            pathFormat = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(pathFormat,
                new MatchEvaluator(delegate (Match match)
                {
                    var digit = 6;
                    if (match.Groups.Count > 2)
                    {
                        digit = Convert.ToInt32(match.Groups[2].Value);
                    }
                    var rand = new Random();
                    return rand.Next((int)Math.Pow(10, digit), (int)Math.Pow(10, digit + 1)).ToString();
                }));

            pathFormat = pathFormat.Replace("{time}", DateTime.Now.Ticks.ToString());
            pathFormat = pathFormat.Replace("{yyyy}", DateTime.Now.Year.ToString());
            pathFormat = pathFormat.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            pathFormat = pathFormat.Replace("{MM}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Minute.ToString("D2"));
            pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));
            pathFormat = pathFormat.Replace("{ffff}", DateTime.Now.Millisecond.ToString("D2"));
            return pathFormat;
        }
    }
}