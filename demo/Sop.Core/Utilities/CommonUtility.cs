using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sop.Core.Utilities
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static class CommonUtility
    {
        /// <summary>
        /// 
        /// </summary>
        public static Random RandomNumber = new Random(DateTime.Now.Millisecond);
        /// <summary>
        ///  /*"upload/image/{yyyy}{mm}{dd}/{time}{rand:6}", /* 上传保存路径,可以自定义保存路径和文件名格式 */
        ///  /* {rand:6} 会替换成随机数,后面的数字是随机数的位数 */
        ///  /* {time} 会替换成时间戳 */
        ///  /* {yyyy} 会替换成四位年份 */
        ///  /* {yy} 会替换成两位年份 */
        ///  /* {MM} 会替换成两位月份 */
        ///  /* {dd} 会替换成两位日期 */
        ///  /* {hh} 会替换成两位小时 */
        ///  /* {mm} 会替换成两位分钟 */
        ///  /* {ss} 会替换成两位秒 */
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
        #region 截取指定长度字符串
        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString">要处理的字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>返回处理后的字符串</returns>
        public static string Trim1(string inputString, int len)
        {
            bool isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        #endregion

        
        /// <summary>
        /// 对字符串进行截字(区分单字节及双字节字符)
        /// </summary>
        /// <remarks>
        /// 一个字符指双字节字符，单字节字符仅算半个字符
        /// </remarks>
        /// <param name="rawString">待截字的字符串</param>
        /// <param name="charLimit">截字的长度，按双字节计数</param>
        /// <param name="appendString">截去字的部分用替代字符串</param>
        /// <returns>截字后的字符串</returns>
        public static string Trim(string rawString, int charLimit, string appendString= "...")
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length <= charLimit)
            {
                return rawString;
            }
            int num = System.Text.Encoding.UTF8.GetBytes(rawString).Length;
            if (num <= charLimit * 2)
            {
                return rawString;
            }
            charLimit = charLimit * 2 - System.Text.Encoding.UTF8.GetBytes(appendString).Length;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            int num2 = 0;
            for (int i = 0; i < rawString.Length; i++)
            {
                char c = rawString[i];
                stringBuilder.Append(c);
                num2 += ((c > '\u0080') ? 2 : 1);
                if (num2 >= charLimit)
                {
                    break;
                }
            }
            return stringBuilder.Append(appendString).ToString();
        }
        /// <summary>
        /// Unicode转义序列
        /// </summary>
        /// <param name="rawString">待编码的字符串</param>
        public static string UnicodeEncode(string rawString)
        {
            if (rawString == null || rawString == string.Empty)
            {
                return rawString;
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < rawString.Length; i++)
            {
                int num = (int)rawString[i];
                string text;
                if (num > 126)
                {
                    stringBuilder.Append("\\u");
                    text = num.ToString("x");
                    for (int j = 0; j < 4 - text.Length; j++)
                    {
                        stringBuilder.Append("0");
                    }
                }
                else
                {
                    text = ((char)num).ToString();
                }
                stringBuilder.Append(text);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 清除xml中的不合法字符
        /// </summary>
        /// <remarks>
        /// <para>无效字符：</para>
        /// <list type="number">
        /// <item>0x00 - 0x08</item>
        /// <item>0x0b - 0x0c</item>
        /// <item>0x0e - 0x1f</item>
        /// </list>        
        /// </remarks>
        /// <param name="rawXml">待清理的xml字符串</param>
        public static string CleanInvalidCharsForXML(string rawXml)
        {
            if (string.IsNullOrEmpty(rawXml))
            {
                return rawXml;
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            char[] array = rawXml.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int num = System.Convert.ToInt32(array[i]);
                if ((num < 0 || num > 8) && (num < 11 || num > 12) && (num < 14 || num > 31))
                {
                    stringBuilder.Append(array[i]);
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 清理Sql注入特殊字符
        /// </summary>
        /// <remarks>
        /// 需清理字符：'、--、exec 、' or
        /// </remarks>
        /// <param name="sql">待处理的sql字符串</param>
        /// <returns>清理后的sql字符串</returns>
        public static string StripSQLInjection(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                string pattern = "((\\%27)|(\\'))\\s*((\\%6F)|o|(\\%4F))((\\%72)|r|(\\%52))";
                string pattern2 = "(\\%27)|(\\')|(\\-\\-)";
                string pattern3 = "\\s+exec(\\s|\\+)+(s|x)p\\w+";
                sql = Regex.Replace(sql, pattern, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern2, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern3, string.Empty, RegexOptions.IgnoreCase);
                sql = sql.Replace("%", "[%]");
            }
            return sql;
        }


        #region 提供常用字符串处理的相关函数

        /// <summary>
        /// 全局使用随机类实例，更易产生不重复随机数(尤其循环中使用)。
        /// </summary>


        /// <summary>
        /// 生成一个字符串唯一码OnlyID，全长30个字符以内。(日期数字格式)
        /// </summary>
        /// <returns>OnlyID</returns>
        public static string GenerateOnlyID()
        {
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssffff"), RandomNumber.Next(0, 10));
        }
        /// <summary>
        /// 生成一个字符串唯一码OnlyID，全长30个字符以内。(特定编码格式)
        /// </summary>
        /// <param name="FirstLetter">编码首字母(或字符串)</param>
        /// <param name="SplitLetter">分割字母(或字符串)</param>
        /// <returns>OnlyID</returns>
        public static string GenerateOnlyID(string FirstLetter, string SplitLetter)
        {
            if (string.IsNullOrEmpty(FirstLetter))
            {
                FirstLetter = "S";
            }
            if (string.IsNullOrEmpty(SplitLetter))
            {
                SplitLetter = "-";
            }
            return string.Format("{0}{1}{2}{3}", new object[]
            {
                FirstLetter,
                DateTime.Now.ToString("yyMM"),
                SplitLetter,
                DateTime.Now.ToString("ddHHmmssfff")
            });
        }
        /// <summary>
        /// 从左侧截取指定长度的字符串。
        /// </summary>
        /// <param name="StrValue">原始数据</param>
        /// <param name="LeftCount">截取长度</param>
        /// <param name="OmitSign">附加省略号标记，可为空</param>
        /// <returns>截取后的数据</returns>
        public static string GetStringLeft(string StrValue, int LeftCount, string OmitSign)
        {
            string text = string.Empty;
            if (string.IsNullOrEmpty(StrValue))
            {
                return text;
            }
            if (StrValue.Length > LeftCount)
            {
                text = StrValue.Substring(0, LeftCount);
                if (!string.IsNullOrEmpty(OmitSign))
                {
                    text += OmitSign;
                }
            }
            else
            {
                text = StrValue;
            }
            return text;
        }
        ///// <summary>
        ///// 从左侧截取指定长度的字符串。
        ///// </summary>
        ///// <param name="ObjValue">原始数据，执行操作时自动将其转换为字符串</param>
        ///// <param name="LeftCount">截取长度</param>
        ///// <param name="OmitSign">附加省略号标记，可为空</param>
        ///// <returns>截取后的数据</returns>
        //public static string GetStringLeft(object ObjValue, int LeftCount, string OmitSign)
        //{
        //    string empty = string.Empty;
        //    if (ObjValue == null)
        //    {
        //        return empty;
        //    }
        //    return StringUtility.GetStringLeft(Convert.ToString(ObjValue), LeftCount, OmitSign);
        //}
        /// <summary>
        /// 生成省略形式的IP地址，如192.168.100.*
        /// </summary>
        /// <param name="FullIp">完整的IP地址</param>
        /// <returns>省略形式的IP地址</returns>
        public static string IpAddressOmission(string FullIp)
        {
            if (string.IsNullOrEmpty(FullIp))
            {
                return string.Empty;
            }
            int num = FullIp.LastIndexOf('.');
            if (num <= 0)
            {
                return string.Empty;
            }
            return FullIp.Substring(0, num + 1) + "*";
        }
        /// <summary>
        /// 将指定的阿拉伯数字转换至对应的中文大写文字，如“1”对应“一”
        /// </summary>
        /// <param name="TheNumber">仅支持到万位的数字转换，超出部分显示不佳</param>
        /// <returns>对应字符串</returns>
        public static string ConvertToChineseNumber(int TheNumber)
        {
            if (TheNumber < 0)
            {
                return TheNumber.ToString();
            }
            string[] array = new string[]
            {
                "",
                "十",
                "百",
                "千",
                "万"
            };
            StringBuilder stringBuilder = new StringBuilder();
            string text = TheNumber.ToString();
            string value = string.Empty;
            int i = 0;
            int num = text.Length - 1;
            while (i < text.Length)
            {
                string text2 = string.Empty;
                switch (text[i])
                {
                    case '0':
                        text2 = "零";
                        break;
                    case '1':
                        text2 = "一";
                        break;
                    case '2':
                        text2 = "二";
                        break;
                    case '3':
                        text2 = "三";
                        break;
                    case '4':
                        text2 = "四";
                        break;
                    case '5':
                        text2 = "五";
                        break;
                    case '6':
                        text2 = "六";
                        break;
                    case '7':
                        text2 = "七";
                        break;
                    case '8':
                        text2 = "八";
                        break;
                    case '9':
                        text2 = "九";
                        break;
                }
                if (num >= array.Length)
                {
                    stringBuilder.Append(text2);
                }
                else
                {
                    if (text2.Equals("零"))
                    {
                        value = text2;
                    }
                    else
                    {
                        stringBuilder.Append(value);
                        stringBuilder.Append(text2);
                        stringBuilder.Append(array[num]);
                        value = string.Empty;
                    }
                }
                i++;
                num--;
            }
            if (stringBuilder.Length == 0)
            {
                stringBuilder.Append(value);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 把一个数值转换为“1.8万”这种短模式，小于1万的原样显示
        /// </summary>
        /// <param name="Number">数值</param>
        /// <param name="Scale">保留小数点精度</param>
        /// <returns>以万计数的显示字符串</returns>
        public static string NumberToShortString(int Number, int Scale)
        {
            int num = Math.Abs(Number);
            if (num < 10000)
            {
                return Number.ToString();
            }
            double num2 = (double)num / 10000.0;
            if (Scale < 1)
            {
                Scale = 1;
            }
            string text = num2.ToString("0.".PadRight(Scale + 2, '0'));
            text += "万";
            if (Number <= 0)
            {
                return string.Format("-{0}", text);
            }
            return text;
        }
        /// <summary>
        /// 对指定的纯文本进行自动排版
        /// </summary>
        /// <param name="Contents">要排版的原始纯文本</param>
        /// <returns>排版后的内容</returns>
        public static string AutoTypesetting(string Contents)
        {
            string text = System.Environment.NewLine;
            if (!Contents.Contains(text))
            {
                text = "\n";
                if (!Contents.Contains(text))
                {
                    text = "\r";
                }
            }
            string text2 = "$*$-p-$*$";
            Contents = Contents.Replace("\u3000", "  ");
            Contents = Contents.Replace(text + text, text2);
            Contents = Contents.Replace(text + "\t", text2);
            Contents = Contents.Replace(text + '\t'.ToString(), text2);
            Contents = Contents.Replace(text + " ", text2);
            List<string[]> list = new List<string[]>
            {
                new string[]
                {
                    "第一",
                    "第二",
                    "第三",
                    "第四",
                    "第五",
                    "第六",
                    "第七",
                    "第八",
                    "第九",
                    "第十"
                },
                new string[]
                {
                    "一、",
                    "二、",
                    "三、",
                    "四、",
                    "五、",
                    "六、",
                    "七、",
                    "八、",
                    "九、",
                    "十、"
                },
                new string[]
                {
                    "一，",
                    "二，",
                    "三，",
                    "四，",
                    "五，",
                    "六，",
                    "七，",
                    "八，",
                    "九，",
                    "十，"
                },
                new string[]
                {
                    "一：",
                    "二：",
                    "三：",
                    "四：",
                    "五：",
                    "六：",
                    "七：",
                    "八：",
                    "九：",
                    "十："
                },
                new string[]
                {
                    "1、",
                    "2、",
                    "3、",
                    "4、",
                    "5、",
                    "6、",
                    "7、",
                    "8、",
                    "9、",
                    "10、"
                },
                new string[]
                {
                    "1.",
                    "2.",
                    "3.",
                    "4.",
                    "5.",
                    "6.",
                    "7.",
                    "8.",
                    "9.",
                    "10."
                },
                new string[]
                {
                    "①",
                    "②",
                    "③",
                    "④",
                    "⑤",
                    "⑥",
                    "⑦",
                    "⑧",
                    "⑨",
                    "⑩"
                },
                new string[]
                {
                    "⒈",
                    "⒉",
                    "⒊",
                    "⒋",
                    "⒌",
                    "⒍",
                    "⒎",
                    "⒏",
                    "⒐",
                    "⒑"
                },
                new string[]
                {
                    "⑴",
                    "⑵",
                    "⑶",
                    "⑷",
                    "⑸",
                    "⑹",
                    "⑺",
                    "⑻",
                    "⑼",
                    "⑽"
                },
                new string[]
                {
                    "㈠",
                    "㈡",
                    "㈢",
                    "㈣",
                    "㈤",
                    "㈥",
                    "㈦",
                    "㈧",
                    "㈨",
                    "㈩"
                }
            };
            foreach (string[] current in list)
            {
                if ((Contents.Contains(text + current[0]) || Contents.StartsWith(current[0])) && Contents.Contains(text + current[1]))
                {
                    for (int i = 0; i < current.Length; i++)
                    {
                        Contents = Contents.Replace(text + current[i], text2 + current[i]);
                    }
                }
                if ((Contents.Contains(" " + current[0]) || Contents.StartsWith(current[0])) && Contents.Contains(" " + current[1]))
                {
                    for (int j = 0; j < current.Length; j++)
                    {
                        Contents = Contents.Replace(" " + current[j], text2 + current[j]);
                    }
                }
            }
            Contents = Contents.Replace("\r", string.Empty).Replace("\n", string.Empty);
            Contents = Contents.Replace("\t", string.Empty);
            Contents = Contents.Replace('\t'.ToString(), string.Empty);
            Contents = Contents.Replace(" ", string.Empty);
            while (Contents.Contains(text2 + text2))
            {
                Contents = Contents.Replace(text2 + text2, text2);
            }
            if (Contents.StartsWith(text2))
            {
                Contents = Contents.Substring(text2.Length);
            }
            if (Contents.EndsWith(text2))
            {
                Contents = Contents.Substring(0, Contents.Length - text2.Length);
            }
            Contents = Contents.Replace(text2, System.Environment.NewLine + System.Environment.NewLine + "\u3000\u3000");
            Contents = string.Format("\u3000\u3000{0}", Contents);
            return Contents;
        }

        #endregion



        #region 字符串操作类
        #region 123
        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        /// <summary>
        /// 把字符串转 按照, 分割 换为数据
        /// </summary>
        /// <param name="str">把字符串转</param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new Char[] { ',' });
        }

        /// <summary>
        /// 把List按照分隔符组装成string类型  
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in list)
            {
                sb.Append(kvp.Value + ",");
            }
            if (list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
        }


        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion




        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 把字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="o_str"></param>
        /// <param name="sepeater"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }


        #region 将字符串样式转换为纯字符串
        /// <summary>
        ///  将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        /// <summary>
        /// 将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if ((str != null) && (str != ""))
            {
                strArray = new Regex(splitstr).Split(str);
            }
            return strArray;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="String"></param>
        /// <param name="IsDel"></param>
        /// <returns></returns>
        public static string SqlSafeString(string String, bool IsDel)
        {
            if (IsDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }
            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }

        #region 获取正确的Id，如果不是正整数，返回0
        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }
        #endregion
        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }
        #endregion
        #region 快速验证一个字符串是否符合指定的正则表达式。
        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
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

        #region 截取指定长度字符串
        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString">要处理的字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>返回处理后的字符串</returns>
        public static string ClipString(string inputString, int len)
        {
            bool isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "…";
            return tempString;
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
            if (data.GetType() == typeof(System.String))
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
            if (data.GetType() == typeof(System.String))
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


        #endregion
        #region 字符串转换和操作

        /// <summary>
        /// 将指定普通文本转换成HTML文本, 返回转换后HTML文本
        /// </summary>
        /// <param name="content">要转换的普通文本</param>
        /// <returns>HTML文本</returns>
        public static string TextToHtml(string content)
        {
            StringBuilder sb = new StringBuilder(content);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\"", "&quot;");
            //     sb.Replace("\'", "&#39;");
            sb.Replace(" ", "&nbsp;");
            sb.Replace("\t", "&nbsp;&nbsp;");
            sb.Replace("\r", "");
            sb.Replace("\n", "<br />");
            return sb.ToString();
            //  return ShitEncode(sb.ToString());
            //return content.Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").
            //    Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "").Replace("\n", "<br />");
        }

        /// <summary>
        /// 将指定HTML文本转换成普通文本, 返回转换后普通文本
        /// </summary>
        /// <param name="content">要转换的HTML文本</param>
        /// <returns>普通文本</returns>
        public static string HtmlToText(string content)
        {
            StringBuilder sb = new StringBuilder(content);
            sb.Replace("<br />", "\n");
            sb.Replace("<br/>", "\n");
            //  sb.Replace("\r", "");
            sb.Replace("&nbsp;&nbsp;", "\t");
            sb.Replace("&nbsp;", " ");
            sb.Replace("&#39;", "\'");
            sb.Replace("&quot;", "\"");
            sb.Replace("&gt;", ">");
            sb.Replace("&lt;", "<");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }






        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码</param>   
        ///<returns>已经去除后的文字</returns>   
        public static string RemoveHtml(string Htmlstring)
        {
            //删除脚本和嵌入式CSS   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);

            //删除HTML   
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            Htmlstring = regex.Replace(Htmlstring, "");
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");

            return Htmlstring;
        }

        /// <summary>
        /// 编码成 sql 文本可以接受的格式
        /// </summary>
        public static string SqlEncode(string s)
        {
            if (null == s || 0 == s.Length)
            {
                return string.Empty;
            }
            return s.Trim().Replace("'", "''");
        }

        /// <summary>
        /// string型转换为int型,转换失败返回缺省值
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="def">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int def)
        {
            if (IsInt(str))
            {
                return int.Parse(str);
            }
            else
            {
                return def;
            }
        }

        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetFileName(string url)
        {
            if (url == null)
            {
                return "";
            }
            //是否有参数
            if (url.IndexOf("?") != -1)
            {
                //去掉参数
                string noquery = url.Substring(0, url.IndexOf("?"));

                //根据/分组
                string[] filenames = noquery.Split(new char[] { '/' });

                //文件名
                string filename = filenames[filenames.Length - 1];

                return filename;
            }
            else
            {
                return System.IO.Path.GetFileName(url);
            }
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 将DateTime时间换成中文
        /// </summary>
        /// <example>
        /// 2012-12-21 12:12:21.012 → 1月前
        /// 2011-12-21 12:12:21.012 → 1年前
        /// </example>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static string DateToChineseString(DateTime datetime)
        {
            TimeSpan ts = DateTime.Now - datetime;
            if ((int)ts.TotalDays >= 365)
            {
                return (int)ts.TotalDays / 365 + "年前";
            }
            if ((int)ts.TotalDays >= 30 && ts.TotalDays <= 365)
            {
                return (int)ts.TotalDays / 30 + "月前";
            }
            if ((int)ts.TotalDays == 1)
            {
                return "昨天";
            }
            if ((int)ts.TotalDays == 2)
            {
                return "前天";
            }
            if ((int)ts.TotalDays >= 3 && ts.TotalDays <= 30)
            {
                return (int)ts.TotalDays + "天前";
            }
            if ((int)ts.TotalDays == 0)
            {
                if ((int)ts.TotalHours != 0)
                {
                    return (int)ts.TotalHours + "小时前";
                }
                else
                {
                    if ((int)ts.TotalMinutes == 0)
                    {
                        return "1分钟前";
                    }
                    else
                    {
                        return (int)ts.TotalMinutes + "分钟前";
                    }
                }
            }
            return datetime.ToString("yyyy年MM月dd日 HH:mm");
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串(过时)
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            return CutString(str, startIndex, length, string.Empty);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="length">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int length)
        {
            return CutString(str, 0, length, string.Empty);
        }

        /// <summary>
        /// 截取字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string CutString(string str, int length, string def)
        {
            return CutString(str, 0, length, def);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string CutString(string str, int startIndex, int length, string def)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            if (str.Length - startIndex <= length)
            {
                length = str.Length - startIndex;
                def = string.Empty;
            }
            try
            {
                return str.Substring(startIndex, length) + def;
            }
            catch
            {
                return str + def;
            }
        }


        #endregion

        #region 字符串判断

        /// <summary>
        /// 检测指定字符串是否有SQL危险字符
        /// 没有返回true
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 判断指定字符串是否为合法IP地址
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsIP(string str)
        {
            return Regex.IsMatch(str, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        ///  判断指定字符串是否合法的日期格式
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsData(string str)
        {
            DateTime dt;
            return DateTime.TryParse(str, out dt);
            //try
            //{
            //    System.DateTime.Parse(value);
            //}
            //catch
            //{
            //    return false;
            //}
            //return true;
        }

        /// <summary>
        /// 判断指定的字符串是否为数字
        /// </summary>
        /// <param name="str">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsInt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return Regex.IsMatch(str, @"^(0|[1-9]\d*)$");
        }

        /// <summary>
        /// 判断指定的字符串是否为Url地址
        /// </summary>
        /// <param name="str">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsHttpUrl(string str)
        {
            //return Regex.IsMatch(WebUrl, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            //    return Regex.IsMatch(WebUrl, @"http://");
            return str.IndexOf("http://") != -1;
        }

        /// <summary>
        /// 判断指定的字符串是否为合法Email
        /// </summary>
        /// <param name="str">指定的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsEmail(string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        #endregion




        #endregion








        #region  long GetLongID()
        /// <summary>
        /// 生成long类型int字符串
        /// </summary>
        /// <returns>返回19位long类型数字</returns>
        public static long GetLongID()
        {
            byte[] buffer = System.Guid.NewGuid().ToByteArray();
            long id = BitConverter.ToInt64(buffer, 0);
            return id;
        }
        #endregion

        #region 生成小写GUID(NDBP不区分大小写)+string GetGUID(string TypeNDBP)
        /// <summary>
        /// 生成小写GUID(NDBP不区分大小写)
        /// </summary>
        /// <param name="TypeNDBP">N:32位数,D:-32位数,B:{-}32位数,P:(-)32位数</param>
        /// <returns>返回32位小写GUID</returns>
        /// <example>
        /// eg:   StringParse.GetGUID("n").ToUpper()
        /// </example>
        public static string GetGUID(string TypeNDBP = "N")
        {
            string str = System.Guid.NewGuid().ToString(TypeNDBP).ToLower();
            return str;

        }
        #endregion












        #region  /******************************** 随机数字  **************************************/



        /// <summary>
        /// 获取字符串中规定个数的随机字符
        /// </summary>
        /// <param name="CodeCount">字符串长度</param>
        /// <param name="allChar">字符串类型 如：X,X,X,X,X,X,X,X</param>
        /// <returns>返回随机字符串</returns>
        public static object GetRandomCode(int CodeCount, string allChar = null)
        {
            if (allChar == null)
                allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
            //替换用户输入的，情况，需要判断，临时标记
            string[] allCharArray = allChar.Split(',');
            object RandomCode = "";
            int temp = -1;

            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    RandomNumber = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = RandomNumber.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = RandomNumber.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }

        /// <summary>
        /// 获取随机生成不重复数字字符串
        /// </summary>
        /// <param name="codeCount">指定的长度</param>
        /// <returns>随机不重复数字字符串</returns>
        public static object GetRandomNum(int codeCount)
        {
            int rep = 0;
            object obj = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                obj = obj + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return obj;
        }

        /// <summary>
        /// 获取随机生成字符串（数字和字母混和）
        /// </summary>
        /// <param name="codeCount">制定长度</param>
        /// <returns>随机字符串（数字和字母混和）</returns>
        public static object GetRandomNumAndABC(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        #endregion


    }
}
