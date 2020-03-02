using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sop.Core.WebUtility
{
    public class WebComUtility
    {
        #region 域名
        /// <summary>
        /// 验证字符串是否是域名
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns></returns>
        public static bool IsDomain(string str)
        {
            string pattern = @"^[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+$";
            return IsMatch(pattern, str);
        }
        /// <summary>
        /// 判断一个字符串，是否匹配指定的表达式(区分大小写的情况下)
        /// </summary>
        /// <param name="expression">正则表达式</param>
        /// <param name="str">要匹配的字符串</param>
        /// <returns></returns>
        public static bool IsMatch(string expression, string str)
        {
            try
            {
                Regex reg = new Regex(expression);
                if (string.IsNullOrEmpty(str))
                    return false;
                return reg.IsMatch(str);
            }
            catch (Exception e)
            {
                return false;
            }


        }
        /// <summary>
        /// 匹配获取字符串中所有的域名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> MatchsDomain(string input)
        {
            string pattern = @"[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+";
            return Matchs(input, pattern);
        }
        /// <summary>
        /// 匹配结果  返回匹配结果的数组
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static List<string> Matchs(string input, string expression)
        {
            List<string> list = new List<string>();
            MatchCollection collection = Regex.Matches(input, expression, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match item in collection)
            {
                if (item.Success)
                {
                    list.Add(item.Value);
                }
            }
            return list;
        } 
        #endregion
    }
}
