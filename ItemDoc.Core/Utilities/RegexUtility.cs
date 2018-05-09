using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItemDoc.Core.Utilities
{
    /// <summary>
    /// 正则验证工具类
    /// </summary>
    public class RegexUtility
    {
        /// <summary>
        /// 验证电子邮箱
        /// @字符前可以包含字母、数字、下划线和点号；
        /// @字符后可以包含字母、数字、下划线和点号；
        /// @字符后至少包含一个点号且点号不能是最后一个字符；最后一个点号后只能是字母或数字
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsEmail(string input)
        {
            if (!input.Contains("@"))
                return false;

            string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (Regex.IsMatch(input, expression, RegexOptions.Compiled))
                return false;
            else
            {
                //邮箱名以数字或字母开头；邮箱名可由字母、数字、点号、减号、下划线组成；
                //邮箱名（@前的字符）长度为3～18个字符；邮箱名不能以点号、减号或下划线结尾；
                // 不能出现连续两个或两个以上的点号、减号。
                Regex regex = new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
                return regex.IsMatch(input);
            }

        }
        
        /// <summary>
        /// 判断是否是手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string str)
        {
            Regex regex = new Regex("^13\\d{9}$");
            return regex.IsMatch(str);
        }


        /// <summary>
        /// 判断输入的字符串只包含汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChineseCh(string str)
        {
            Regex regex = new Regex("^[\u4e00-\u9fa5]+$");
            return regex.IsMatch(str);
        }

        #region 验证字符串是否匹配正则表达式描述的规则（私有方法）
        /// <summary>
        /// 验证字符串是否匹配正则表达式描述的规则
        /// </summary>
        /// <param name="inputStr">待验证的字符串</param>
        /// <param name="patternStr">正则表达式字符串</param>
        /// <returns>是否匹配</returns>
        private static bool IsMatch(string inputStr, string patternStr)
        {
            return IsMatch(inputStr, patternStr, false, false);
        }
        #endregion

        #region 验证字符串是否匹配正则表达式描述的规则(指定是否区分大小写）
        /// <summary>
        /// 验证字符串是否匹配正则表达式描述的规则
        /// </summary>
        /// <param name="inputStr">待验证的字符串</param>
        /// <param name="patternStr">正则表达式字符串</param>
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>
        /// <returns>是否匹配</returns>
        public static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase)
        {
            return IsMatch(inputStr, patternStr, ifIgnoreCase, false);
        }
        #endregion

        #region 验证字符串是否匹配正则表达式描述的规则（是否验证空字符串）
        /// <summary>
        /// 验证字符串是否匹配正则表达式描述的规则
        /// </summary>
        /// <param name="inputStr">待验证的字符串</param>
        /// <param name="patternStr">正则表达式字符串</param>
        /// <param name="ifValidateWhiteSpace">是否验证空白字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsMatchIfWithWhiteSpace(string inputStr, string patternStr, bool ifValidateWhiteSpace)
        {
            return IsMatch(inputStr, patternStr, false, ifValidateWhiteSpace);
        }
        #endregion

        #region 验证字符串是否匹配正则表达式描述的规则（是否区分大小写和验证空白字符串）
        /// <summary>
        /// 验证字符串是否匹配正则表达式描述的规则
        /// </summary>
        /// <param name="inputStr">待验证的字符串</param>
        /// <param name="patternStr">正则表达式字符串</param>
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>
        /// <param name="ifValidateWhiteSpace">是否验证空白字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase, bool ifValidateWhiteSpace)
        {
            if (!ifValidateWhiteSpace && string.IsNullOrWhiteSpace(inputStr))
                return false;//如果不要求验证空白字符串而此时传入的待验证字符串为空白字符串，则不匹配
            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配
            else
                regex = new Regex(patternStr);
            return regex.IsMatch(inputStr);
        }
        #endregion
        #region 验证是否只含字母、数字、下划线
        /// <summary>
        /// 验证是否只含字母、数字、下划线
        /// </summary>
        /// <param name="value">被判断的值</param>
        public static bool IsLetter_Num_Underline(string value)
        {
            Regex regex = new Regex("^([a-zA-Z\\d_])+$");
            return regex.IsMatch(value);
        }


        #endregion

        #region 验证是否只含字母、数字、减号（-）
        /// <summary>
        /// 验证是否只含字母、数字、减号（-）
        /// </summary>
        /// <param name="sValue">要验证的值</param>
        public static bool IsLetter_Num_Jianhao(string sValue)
        {
            Regex regex = new Regex("^([a-zA-Z\\d-])+$");
            return regex.IsMatch(sValue);
        }
        #endregion

        #region 得到字符串长度，一个汉字长度为2
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        #endregion
        #region 验证数字(double类型)
        /// <summary>
        /// 验证数字(double类型)
        /// [可以包含负号和小数点]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsNumber(string input)
        {
            //string pattern = @"^-?\d+$|^(-?\d+)(\.\d+)?$";
            //return IsMatch(input, pattern);
            double d = 0;
            if (double.TryParse(input, out d))
                return true;
            else
                return false;
        }
        #endregion

        #region 验证整数
        /// <summary>
        /// 验证整数
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsInteger(string input)
        {
            //string pattern = @"^-?\d+$";
            //return IsMatch(input, pattern);
            int i = 0;
            if (int.TryParse(input, out i))
                return true;
            else
                return false;
        }
        #endregion

        #region 验证非负整数
        /// <summary>
        /// 验证非负整数
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIntegerNotNagtive(string input)
        {
            //string pattern = @"^\d+$";
            //return IsMatch(input, pattern);
            int i = -1;
            if (int.TryParse(input, out i) && i >= 0)
                return true;
            else
                return false;
        }
        #endregion

        #region  验证正整数
        /// <summary>
        /// 验证正整数
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIntegerPositive(string input)
        {
            //string pattern = @"^[0-9]*[1-9][0-9]*$";
            //return IsMatch(input, pattern);
            int i = 0;
            if (int.TryParse(input, out i) && i >= 1)
                return true;
            else
                return false;
        }
        #endregion

        #region 验证小数
        /// <summary>
        /// 验证小数
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsDecimal(string input)
        {
            string pattern = @"^([-+]?[1-9]\d*\.\d+|-?0\.\d*[1-9]\d*)$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证只包含英文字母
        /// <summary>
        /// 验证只包含英文字母
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsEnglishCharacter(string input)
        {
            string pattern = @"^[A-Za-z]+$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证只包含数字和英文字母
        /// <summary>
        /// 验证只包含数字和英文字母
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIntegerAndEnglishCharacter(string input)
        {
            string pattern = @"^[0-9A-Za-z]+$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证只包含汉字
        /// <summary>
        /// 验证只包含汉字
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsChineseCharacter(string input)
        {
            string pattern = @"^[\u4e00-\u9fa5]+$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证数字长度范围（数字前端的0计长度）[若要验证固定长度，可传入相同的两个长度数值]
        /// <summary>
        /// 验证数字长度范围（数字前端的0计长度）
        /// [若要验证固定长度，可传入相同的两个长度数值]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <param name="lengthBegin">长度范围起始值（含）</param>
        /// <param name="lengthEnd">长度范围结束值（含）</param>
        /// <returns>是否匹配</returns>
        public static bool IsIntegerLength(string input, int lengthBegin, int lengthEnd)
        {
            //string pattern = @"^\d{" + lengthBegin + "," + lengthEnd + "}$";
            //return IsMatch(input, pattern);
            if (input.Length >= lengthBegin && input.Length <= lengthEnd)
            {
                int i;
                if (int.TryParse(input, out i))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        #endregion

        #region 验证字符串包含内容
        /// <summary>
        /// 验证字符串包含内容
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <param name="withEnglishCharacter">是否包含英文字母</param>
        /// <param name="withNumber">是否包含数字</param>
        /// <param name="withChineseCharacter">是否包含汉字</param>
        /// <returns>是否匹配</returns>
        public static bool IsStringInclude(string input, bool withEnglishCharacter, bool withNumber, bool withChineseCharacter)
        {
            if (!withEnglishCharacter && !withNumber && !withChineseCharacter)
                return false;//如果英文字母、数字和汉字都没有，则返回false
            StringBuilder patternString = new StringBuilder();
            patternString.Append("^[");
            if (withEnglishCharacter)
                patternString.Append("a-zA-Z");
            if (withNumber)
                patternString.Append("0-9");
            if (withChineseCharacter)
                patternString.Append(@"\u4E00-\u9FA5");
            patternString.Append("]+$");
            return IsMatch(input, patternString.ToString());
        }
        #endregion

        #region 验证字符串包含内容
        /// <summary>
        /// 验证字符串长度范围
        /// [若要验证固定长度，可传入相同的两个长度数值]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <param name="lengthBegin">长度范围起始值（含）</param>
        /// <param name="lengthEnd">长度范围结束值（含）</param>
        /// <returns>是否匹配</returns>
        public static bool IsStringLength(string input, int lengthBegin, int lengthEnd)
        {
            //string pattern = @"^.{" + lengthBegin + "," + lengthEnd + "}$";
            //return IsMatch(input, pattern);
            if (input.Length >= lengthBegin && input.Length <= lengthEnd)
                return true;
            else
                return false;
        }
        #endregion

        #region 验证字符串长度范围（字符串内只包含数字和/或英文字母）
        /// <summary>
        /// 验证字符串长度范围（字符串内只包含数字和/或英文字母）
        /// [若要验证固定长度，可传入相同的两个长度数值]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <param name="lengthBegin">长度范围起始值（含）</param>
        /// <param name="lengthEnd">长度范围结束值（含）</param>
        /// <returns>是否匹配</returns>
        public static bool IsStringLengthOnlyNumberAndEnglishCharacter(string input, int lengthBegin, int lengthEnd)
        {
            string pattern = @"^[0-9a-zA-z]{" + lengthBegin + "," + lengthEnd + "}$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证字符串长度范围
        /// <summary>
        /// 验证字符串长度范围
        /// [若要验证固定长度，可传入相同的两个长度数值]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <param name="withEnglishCharacter">是否包含英文字母</param>
        /// <param name="withNumber">是否包含数字</param>
        /// <param name="withChineseCharacter">是否包含汉字</param>
        /// <param name="lengthBegin">长度范围起始值（含）</param>
        /// <param name="lengthEnd">长度范围结束值（含）</param>
        /// <returns>是否匹配</returns>
        public static bool IsStringLengthByInclude(string input, bool withEnglishCharacter, bool withNumber, bool withChineseCharacter, int lengthBegin, int lengthEnd)
        {
            if (!withEnglishCharacter && !withNumber && !withChineseCharacter)
                return false;//如果英文字母、数字和汉字都没有，则返回false
            StringBuilder patternString = new StringBuilder();
            patternString.Append("^[");
            if (withEnglishCharacter)
                patternString.Append("a-zA-Z");
            if (withNumber)
                patternString.Append("0-9");
            if (withChineseCharacter)
                patternString.Append(@"\u4E00-\u9FA5");
            patternString.Append("]{" + lengthBegin + "," + lengthEnd + "}$");
            return IsMatch(input, patternString.ToString());
        }
        #endregion

        #region 验证字符串字节数长度范围
        /// <summary>
        /// 验证字符串字节数长度范围
        /// [若要验证固定长度，可传入相同的两个长度数值；每个汉字为两个字节长度]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <param name="lengthBegin">长度范围起始值（含）</param>
        /// <param name="lengthEnd">长度范围结束值（含）</param>
        /// <returns></returns>
        public static bool IsStringByteLength(string input, int lengthBegin, int lengthEnd)
        {
            //int byteLength = Regex.Replace(input, @"[^\x00-\xff]", "ok").Length;
            //if (byteLength >= lengthBegin && byteLength <= lengthEnd)
            //{
            //    return true;
            //}
            //return false;
            int byteLength = Encoding.Default.GetByteCount(input);
            if (byteLength >= lengthBegin && byteLength <= lengthEnd)
                return true;
            else
                return false;
        }
        #endregion

        #region 验证日期非正则
        /// <summary>
        /// 验证日期
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsDateTime(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return true;
            else
                return false;
        }
        #endregion

        #region 验证固定电话号码
        /// <summary>
        /// 验证固定电话号码
        /// [3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsTelePhoneNumber(string input)
        {
            string pattern = @"^(((\(0\d{2}\)|0\d{2})[- ]?)?\d{8}|((\(0\d{3}\)|0\d{3})[- ]?)?\d{7})(-\d{3})?$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region  验证手机号码
        /// <summary>
        /// 验证手机号码
        /// [可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，11位手机号前的0可以省略；11位手机号第二位数可以是3、4、5、8中的任意一个]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsMobilePhoneNumber(string input)
        {
            string pattern = @"^(\((\+)?86\)|((\+)?86)?)0?1[3458]\d{9}$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证电话号码（可以是固定电话号码或手机号码）
        /// <summary>
        /// 验证电话号码（可以是固定电话号码或手机号码）
        /// [固定电话：[3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]]
        /// [手机号码：[可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，手机号前的0可以省略；手机号第二位数可以是3、4、5、8中的任意一个]]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsPhoneNumber(string input)
        {
            string pattern = @"^(\((\+)?86\)|((\+)?86)?)0?1[3458]\d{9}$|^(((\(0\d{2}\)|0\d{2})[- ]?)?\d{8}|((\(0\d{3}\)|0\d{3})[- ]?)?\d{7})(-\d{3})?$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证邮政编码
        /// <summary>
        /// 验证邮政编码
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsZipCode(string input)
        {
            //string pattern = @"^\d{6}$";
            //return IsMatch(input, pattern);
            if (input.Length != 6)
                return false;
            int i;
            if (int.TryParse(input, out i))
                return true;
            else
                return false;
        }
        #endregion



        #region 验证网址（可以匹配IPv4地址但没对IPv4地址进行格式验证；IPv6暂时没做匹配）
        /// <summary>
        /// 验证网址（可以匹配IPv4地址但没对IPv4地址进行格式验证；IPv6暂时没做匹配）
        /// [允许省略"://"；可以添加端口号；允许层级；允许传参；域名中至少一个点号且此点号前要有内容]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsURL(string input)
        {
            ////每级域名由字母、数字和减号构成（第一个字母不能是减号），不区分大小写，单个域长度不超过63，完整的域名全长不超过256个字符。在DNS系统中，全名是以一个点“.”来结束的，例如“www.nit.edu.cn.”。没有最后的那个点则表示一个相对地址。 
            ////没有例如"http://"的前缀，没有传参的匹配
            //string pattern = @"^([0-9a-zA-Z][0-9a-zA-Z-]{0,62}\.)+([0-9a-zA-Z][0-9a-zA-Z-]{0,62})\.?$";

            //string pattern = @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&%_\./-~-]*)?$";
            string pattern = @"^([a-zA-Z]+://)?([\w-\.]+)(\.[a-zA-Z0-9]+)(:\d{0,5})?/?([\w-/]*)\.?([a-zA-Z]*)\??(([\w-]*=[\w%]*&?)*)$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 验证IPv4地址
        /// <summary>
        /// 验证IPv4地址
        /// [第一位和最后一位数字不能是0或255；允许用0补位]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIPv4(string input)
        {
            //string pattern = @"^(25[0-4]|2[0-4]\d]|[01]?\d{2}|[1-9])\.(25[0-5]|2[0-4]\d]|[01]?\d?\d)\.(25[0-5]|2[0-4]\d]|[01]?\d?\d)\.(25[0-4]|2[0-4]\d]|[01]?\d{2}|[1-9])$";
            //return IsMatch(input, pattern);
            string[] IPs = input.Split('.');
            if (IPs.Length != 4)
                return false;
            int n = -1;
            for (int i = 0; i < IPs.Length; i++)
            {
                if (i == 0 || i == 3)
                {
                    if (int.TryParse(IPs[i], out n) && n > 0 && n < 255)
                        continue;
                    else
                        return false;
                }
                else
                {
                    if (int.TryParse(IPs[i], out n) && n >= 0 && n <= 255)
                        continue;
                    else
                        return false;
                }
            }
            return true;
        }
        #endregion

        #region 验证IPv6地址
        /// <summary>
        /// 验证IPv6地址
        /// [可用于匹配任何一个合法的IPv6地址]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIPv6(string input)
        {
            string pattern = @"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$";
            return IsMatch(input, pattern);
        }
        #endregion

        #region 身份证省份枚举，为后续开发使用
        /// <summary>
        /// 身份证上数字对应的地址，此处可以用于用户注册地区选择，本类为验证类，如果需要做此验证，请把此段代码定义为枚举。
        /// </summary>
        enum IDAddress
        {
            北京 = 11, 天津 = 12, 河北 = 13, 山西 = 14, 内蒙古 = 15, 辽宁 = 21, 吉林 = 22, 黑龙江 = 23, 上海 = 31, 江苏 = 32, 浙江 = 33,
            安徽 = 34, 福建 = 35, 江西 = 36, 山东 = 37, 河南 = 41, 湖北 = 42, 湖南 = 43, 广东 = 44, 广西 = 45, 海南 = 46, 重庆 = 50, 四川 = 51,
            贵州 = 52, 云南 = 53, 西藏 = 54, 陕西 = 61, 甘肃 = 62, 青海 = 63, 宁夏 = 64, 新疆 = 65, 台湾 = 71, 香港 = 81, 澳门 = 82, 国外 = 91
        }
        #endregion

        #region 验证一代身份证号（15位数）
        /// <summary>
        /// 验证一代身份证号（15位数）
        /// [长度为15位的数字；匹配对应省份地址；生日能正确匹配]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIDCard15(string input)
        {
            //验证是否可以转换为15位整数
            long l = 0;
            if (!long.TryParse(input, out l) || l.ToString().Length != 15)
                return false;
            //验证省份是否匹配
            //1~6位为地区代码，其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码。
            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,";
            if (!address.Contains(input.Remove(2) + ","))
                return false;
            //验证生日是否匹配
            string birthdate = input.Substring(6, 6).Insert(4, "/").Insert(2, "/");
            DateTime dt;
            if (!DateTime.TryParse(birthdate, out dt))
                return false;
            return true;
        }
        #endregion

        #region 验证二代身份证号（匹配对应省份地址；生日能正确匹配；校验码能正确匹配）
        /// <summary>
        /// 验证二代身份证号（18位数，GB11643-1999标准）
        /// [长度为18位；前17位为数字，最后一位(校验码)可以为大小写x；匹配对应省份地址；生日能正确匹配；校验码能正确匹配]
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIDCard18(string input)
        {
            //验证是否可以转换为正确的整数
            long l = 0;
            if (!long.TryParse(input.Remove(17), out l) || l.ToString().Length != 17 || !long.TryParse(input.Replace('x', '0').Replace('X', '0'), out l))
            {
                return false;
            }
            //验证省份是否匹配
            //1~6位为地区代码，其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码。
            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,";
            if (!address.Contains(input.Remove(2) + ","))
            {
                return false;
            }
            //验证生日是否匹配
            string birthdate = input.Substring(6, 8).Insert(6, "/").Insert(4, "/");
            DateTime dt;
            if (!DateTime.TryParse(birthdate, out dt))
            {
                return false;
            }
            //校验码验证
            //校验码：
            //（1）十七位数字本体码加权求和公式 
            //S = Sum(Ai * Wi), i = 0, ... , 16 ，先对前17位数字的权求和 
            //Ai:表示第i位置上的身份证号码数字值 
            //Wi:表示第i位置上的加权因子 
            //Wi: 7 9 10 5 8 4 2 1 6 3 7 9 10 5 8 4 2 
            //（2）计算模 
            //Y = mod(S, 11) 
            //（3）通过模得到对应的校验码 
            //Y: 0 1 2 3 4 5 6 7 8 9 10 
            //校验码: 1 0 X 9 8 7 6 5 4 3 2 
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = input.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != input.Substring(17, 1).ToLower())
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 验证身份证号（不区分一二代身份证号）
        /// <summary>
        /// 验证身份证号（不区分一二代身份证号）
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsIDCard(string input)
        {
            if (input.Length == 18)
                return IsIDCard18(input);
            else if (input.Length == 15)
                return IsIDCard15(input);
            else
                return false;
        }
        #endregion

        #region 验证经度
        /// <summary>
        /// 验证经度
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsLongitude(string input)
        {
            ////范围为-180～180，小数位数必须是1到5位
            //string pattern = @"^[-\+]?((1[0-7]\d{1}|0?\d{1,2})\.\d{1,5}|180\.0{1,5})$";
            //return IsMatch(input, pattern);
            float lon;
            if (float.TryParse(input, out lon) && lon >= -180 && lon <= 180)
                return true;
            else
                return false;
        }
        #endregion

        #region 验证纬度

        /// <summary>
        /// 验证纬度
        /// </summary>
        /// <param name="input">待验证的字符串</param>
        /// <returns>是否匹配</returns>
        public static bool IsLatitude(string input)
        {
            ////范围为-90～90，小数位数必须是1到5位
            //string pattern = @"^[-\+]?([0-8]?\d{1}\.\d{1,5}|90\.0{1,5})$";
            //return IsMatch(input, pattern);
            float lat;
            if (float.TryParse(input, out lat) && lat >= -90 && lat <= 90)
                return true;
            else
                return false;
        }
        #endregion

        /// <summary>
        /// 检查密码是否安全。
        /// </summary>
        /// <param name="Passwords">密码</param>
        /// <param name="LoginCode">登录名(用作安全检查时的参考)</param>
        /// <returns>string.Empty：密码安全，其它：不安全警告信息</returns>
        public static string SafePasswordsCheck(string Passwords, string LoginCode)
        {
            Passwords = ((Passwords == null) ? string.Empty : Passwords.Trim().ToLower());
            LoginCode = ((LoginCode == null) ? string.Empty : LoginCode.Trim().ToLower());
            if (Passwords.Length < 6)
            {
                return "密码长度至少6位！";
            }
            if (Passwords.Equals(LoginCode, StringComparison.CurrentCultureIgnoreCase))
            {
                return "密码不能与登录名相同！";
            }
            if (Passwords.Replace(Passwords.Substring(0, 1), string.Empty) == string.Empty || Passwords.Equals("123456") || Passwords.Equals("520520") || Passwords.Equals("521521") || Passwords.Equals("123321") || Passwords.Equals("123123"))
            {
                return "密码过于简单，太不安全！";
            }
            if (Passwords.Equals("5201314") || Passwords.Equals("1314520") || Passwords.Equals("7758521") || Passwords.Equals("5211314") || Passwords.Equals("1314521") || Passwords.Equals("147258369") || Passwords.Equals("woaini"))
            {
                return "密码过于大众化，太容易被坏人猜到！";
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="value">数字</param>
        public static bool IsNumber1(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
            return regex.IsMatch(value);
        }
        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="value">邮编</param>
        /// <returns>是否符合</returns>
        /// <author>PZ</author>
        public static bool IsPostCode(string value)
        {
            Regex regex = new Regex("^\\d{5,6}$");
            return regex.IsMatch(value);
        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="value">手机号码</param>
        public static bool IsMobile(string value)
        {
            Regex regex = new Regex("(13|15|18|12|)\\d{9}");
            return regex.IsMatch(value);
        }
        /// <summary>
        /// 验证是否只含字母、数字
        /// </summary>
        /// <param name="value">被判断的值</param>
        public static bool IsLetter_Num(string value)
        {
            Regex regex = new Regex("^[A-Za-z0-9]+$");
            return regex.IsMatch(value);
        }


        /// <summary>
        /// 判断是否是合法网址
        /// </summary>
        /// <param name="value">被判断的Url</param>
        /// <returns>true：符合； false：不符合；</returns>
        public static bool IsWebUrl(string value)
        {
            Regex regex = new Regex("^http://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?$");
            return regex.IsMatch(value);
        }
        /// <summary>
        /// 判断目录名是否合法
        /// </summary>
        /// <param name="value">被判断的值</param>
        /// <returns>true：合法； false：不合法；</returns>
        public static bool IsSafeFolder(string value)
        {
            return RegexUtility.IsLetter_Num(value);
        }
        /// <summary>
        /// 判断文件名是否合法
        /// </summary>
        /// <param name="value">被判断的值</param>
        /// <returns>true：合法； false：不合法；</returns>
        public static bool IsSafeFileName(string value)
        {
            string value2 = value.Replace(".", string.Empty);
            return RegexUtility.IsLetter_Num_Underline(value2);
        }
        /// <summary>
        /// 清理多余的标点符号，以免撑坏版面。
        /// </summary>
        /// <param name="value">原始文本</param>
        /// <returns>处理后的文本</returns>
        public static string ClearExcess(string value)
        {
            value = value.Replace("!!!!", "");
            value = value.Replace("????", "");
            value = value.Replace(",,,,", "");
            value = value.Replace("....", "");
            value = value.Replace("。。。。", "");
            value = value.Replace("，，，，", "");
            value = value.Replace("！！！！", "");
            value = value.Replace("？？？？", "");
            value = value.Replace("…………", "");
            value = value.Replace("．．．．", "");
            value = value.Replace("、、、、", "");
            return value;
        }
        /// <summary>
        /// 判断是否颜色值
        /// </summary>
        /// <param name="value">颜色值</param>
        /// <returns>true：是；  false：否；</returns>
        public static bool IsColor(string value)
        {
            bool result = false;
            string text = "0123456789ABCDEFabcdef";
            try
            {
                if (value.Length == 7)
                {
                    char[] array = value.ToCharArray();
                    if (array[0] == '#')
                    {
                        bool flag = true;
                        for (int i = 1; i < 7; i++)
                        {
                            if (!text.Contains(array[i].ToString()))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #region 根据配置对指定字符串进行 MD5 加密
        /// <summary>
        /// 根据配置对指定字符串进行 MD5 加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetMD5(string s)
        {
            return null;
        }
        #endregion

        #region 获得两个日期的间隔
        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="DateTime1">日期一。</param>
        /// <param name="DateTime2">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }
        #endregion

        #region 格式化日期时间
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");
                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");
                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");
                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");
                case "4":
                    return dateTime1.ToString("MM-dd");
                case "5":
                    return dateTime1.ToString("MM/dd");
                case "6":
                    return dateTime1.ToString("MM月dd日");
                case "7":
                    return dateTime1.ToString("yyyy-MM");
                case "8":
                    return dateTime1.ToString("yyyy/MM");
                case "9":
                    return dateTime1.ToString("yyyy年MM月");
                default:
                    return dateTime1.ToString();
            }
        }
        #endregion

        #region 得到随机日期
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        #endregion

        #region HTML转行成TEXT
        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
                @"<script[^>]*?>.*?</script>",
                @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                @"([\r\n])[\s]+",
                @"&(quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"&#(\d+);",
                @"-->",
                @"<!--.*\n"
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }
        #endregion

        #region 判断对象是否为空
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion

        #region 验证日期是否合法
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return true;
            }

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsNullOrEmpty(date))
                {
                    return false;
                }

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion

                return false;
            }
        }
        #endregion

        #region 前面补零
        /// <summary>
        /// 不住位数的数字，前面补零
        /// </summary>
        /// <param name="value">要补足的数字</param>
        /// <param name="size">不齐的位数</param>
        /// <returns></returns>
        public static string Zerofill(string value, int size)
        {
            string tmp = "";
            for (int i = 0; i < size - value.Length; i++)
            {
                tmp += "0";
            }

            return tmp + value;
        }
        #endregion

        #region 过滤掉 html代码
        /// <summary>
        /// 过滤html标签
        /// </summary>
        /// <param name="strHtml">html的内容</param>
        /// <returns></returns>
        public static string StripHTML(string strHtml)
        {
            string[] aryReg ={
                @"<script[^>]*?>.*?</script>",

                @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                @"([\r\n])[\s]+",
                @"&(quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"&#(\d+);",
                @"-->",
                @"<!--.*\n"
            };

            string[] aryRep = {
                "",
                "",
                "",
                "\"",
                "&",
                "<",
                ">",
                " ",
                "\xa1",//chr(161),
                "\xa2",//chr(162),
                "\xa3",//chr(163),
                "\xa9",//chr(169),
                "",
                "\r\n",
                ""
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(newReg, RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, newReg);
            }
            //strOutput.Replace("<", "");
            //strOutput.Replace(">", "");
            strOutput = strOutput.Replace("\r\n", "");
            return strOutput;
        }
        #endregion

        #region 传入URL返回网页的html代码

        /// <summary>
        /// 传入URL返回网页的html代码
        /// </summary>
        /// <param name="Url">URL</param>
        /// <returns></returns>
        public static string GetUrltoHtml(string Url)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                // Read an HTTP-specific property
                //if (wResp.GetType() ==HttpWebResponse)
                //{
                //DateTime updated  =((System.Net.HttpWebResponse)wResp).LastModified;
                //}
                // Get the response stream.
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                System.IO.StreamReader reader = new System.IO.StreamReader(respStream, System.Text.Encoding.GetEncoding("gb2312"));
                return reader.ReadToEnd();

            }
            catch (System.Exception ex)
            {
                var asd = ex.Message;
            }
            return "";
        }

        #endregion

        #region 验证——数字部分
        /// <summary>
        /// 判断是否是实数，是返回true 否返回false。可以传入null。
        /// </summary>
        /// <param name="strVal">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(string strVal)
        {
            //System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("-?([0]|([1-9]+\\d{0,}?))(.[\\d]+)?$");  
            //return reg1.IsMatch(strVal);  
            //string tmp="";
            //判断是否为null 和空字符串
            if (strVal == null || strVal.Length == 0)
                return false;
            //判断是否只有.、-、 -.
            if (strVal == "." || strVal == "-" || strVal == "-.")
                return false;

            //记录是否有多个小数点
            bool isPoint = false;            //是否有小数点

            //去掉第一个负号，中间是不可以有负号的
            strVal = strVal.TrimStart('-');

            foreach (char c in strVal)
            {
                if (c == '.')
                    if (isPoint)
                        return false;
                    else
                        isPoint = true;

                if ((c < '0' || c > '9') && c != '.')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断是否为整数。是返回true 否返回false。可以传入null。
        /// </summary>
        /// <param name="strVal">要判断的字符</param>
        /// <returns></returns>
        public static bool IsInt(string strVal)
        {
            if (strVal == null || strVal.Length == 0)
                return false;
            //判断是否只有.、-、 -.
            if (strVal == "." || strVal == "-" || strVal == "-.")
                return false;

            //去掉第一个负号，中间是不可以有负号的
            if (strVal.Substring(0, 1) == "-")
                strVal = strVal.Remove(0, 1);

            foreach (char c in strVal)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断是否为ID串。是返回true 否返回false。可以传入null。
        /// </summary>
        /// <example >
        /// ,1,2,3,4,5,6,7,
        /// </example>
        /// <param name="strVal">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsIDString(string strVal)
        {
            bool flag = false;
            if (strVal == null)
                return false;
            if (strVal == "")
                return true;
            //判断是否只有 ,
            if (strVal == ",")
                return false;

            //判断第一位是否是,号
            if (strVal.Substring(0, 1) == ",")
                return false;

            //判断最后一位是否是,号
            if (strVal.Substring(strVal.Length - 1, 1) == ",")
                return false;

            foreach (char c in strVal)
            {
                if (c == ',')
                    if (flag) return false; else flag = true;

                else if ((c >= '0' && c <= '9'))
                    flag = false;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 转换为整数。不是整数的话，返回“-1”
        /// </summary>
        /// <param name="str">要转换的字符</param>
        /// <returns></returns>
        public static int StringToInt(string str)
        {
            //判断是否是数字，是数字返回数字，不是数字返回-1
            if (IsInt(str))
                return Int32.Parse(str);
            else
                return -1;
        }

        /// <summary>
        /// 转换为实数。不是实数的话，返回“-1”
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float StrTofloat(string str)
        {
            //判断是否是数字，是数字返回数字，不是数字返回-1
            if (IsNumeric(str))
                return float.Parse(str);
            else
                return -1;
        }

        /// <summary>
        /// 验证是否是GUID
        /// 6454bc76-5f98-de11-aa4c-00219bf56456
        /// </summary>
        /// <returns></returns>
        public static bool IsGUID(string strVal)
        {
            if (strVal == null)
                return false;

            if (strVal == "")
                return false;

            strVal = strVal.TrimStart('{');
            strVal = strVal.TrimEnd('}');

            //长度必须是36位
            if (strVal.Length != 36)
                return false;

            foreach (char c in strVal)
            {
                if (c == '-')
                    continue;
                else if (c >= 'a' && c <= 'f')
                    continue;
                else if (c >= 'A' && c <= 'F')
                    continue;
                else if ((c >= '0' && c <= '9'))
                    continue;
                else
                    return false;
            }
            return true;
        }
        #endregion

        #region 验证——处理字符串部分

        /// <summary>
        /// 去掉两边的空格，把“'”替换为“＇”SBC
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns></returns>
        public static string StringReplaceToSBC(string str)
        {
            //过滤不安全的字符
            string tstr;
            tstr = str.Trim();
            return tstr.Replace("'", "＇");
        }

        /// <summary>
        /// 去掉两边的空格，把“'”替换为“''”DBC
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns></returns>
        public static string StringReplaceToDBC(string str)
        {
            //过滤不安全的字符
            string tstr;
            tstr = str.Trim();
            return tstr.Replace("'", "''");
        }
        /// <summary>
        /// 去掉两边的空格，把“'”替换为“”
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns></returns>
        public static string StringReplaceToEmpty(string str)
        {
            //过滤不安全的字符
            string tstr;
            tstr = str.Trim();
            return tstr.Replace("'", "");
        }

        #endregion

        #region 验证——时间部分

        /// <summary>
        /// 转换时间。不正确的话，返回当前时间
        /// </summary>
        /// <param name="isdt">要转换的字符串</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string isdt)
        {
            //判断时间是否正确
            DateTime mydt;
            try
            {
                mydt = Convert.ToDateTime(isdt);
            }
            catch
            {
                //时间格式不正确
                return mydt = DateTime.Now;
            }

            return mydt;
        }

        /// <summary>
        /// 判断是否是正确的时间格式。正确返回“true”，不正确返回提示信息。
        /// 
        /// </summary>
        /// <param name="isdt">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsDateTime1(string isdt)
        {
            //判断时间是否正确
            DateTime mydt;
            try
            {
                mydt = Convert.ToDateTime(isdt);
                return true;
            }
            catch
            {
                //时间格式不正确
                //errorMsg = "您填的时间格式不正确，请按照2004-1-1的形式填写。";
                return false;
            }


        }
        #endregion

        #region 生成查询条件
        /// <summary>
        /// 组成查询字符串
        /// </summary>
        /// <param name="columnName">字段名</param>
        /// <param name="keyword">查询条件</param>
        /// <param name="hasContent">是否已经有查询条件了，true：加and；false：不加and</param>
        /// <param name="colType">1：数字；2：字符串，精确查询；3：字符串，模糊查询，包括时间查询</param>
        /// <returns></returns>
        public static string GetSearchString(string columnName, string keyword, ref bool hasContent, int colType)
        {
            if (keyword == "" || keyword == "0")
            {
                return "";
            }
            else
            {
                System.Text.StringBuilder tmp = new System.Text.StringBuilder();
                switch (colType)
                {
                    case 1:
                        //数字
                        tmp.Append(columnName);
                        tmp.Append(" = ");
                        tmp.Append(keyword);
                        break;
                    case 2:
                        //字符串，精确查询
                        tmp.Append(columnName);
                        tmp.Append(" = '");
                        tmp.Append(keyword);
                        tmp.Append("' ");
                        break;
                    case 3:
                        //字符串，模糊查询，包括时间查询
                        tmp.Append(columnName);
                        tmp.Append(" like '% ");
                        tmp.Append(keyword);
                        tmp.Append("%' ");

                        break;
                }
                if (hasContent)
                    tmp.Insert(0, " and ");

                hasContent = true;
                return tmp.ToString();

            }

        }
        #endregion



    }
}
