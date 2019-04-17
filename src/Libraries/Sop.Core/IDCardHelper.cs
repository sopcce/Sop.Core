using Sop.Core.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Sop.Core
{
    /// <summary>
    /// 身份证号码解析与生成
    /// </summary>
    public class IdCardHelper
    {
        #region private
        /// <summary>
        /// 
        /// </summary>
        private static readonly List<string[]> Areas = new List<string[]>();
        /// <summary>
        /// 获取区域信息
        /// </summary>
        private static void FillAreas()
        {
            XmlDocument docXml = new XmlDocument();
            string file = FileUtility.GetDiskFilePath("/wwwroot/AreaCode/AreaCodeInfo.xml");
            docXml.Load(file);
            XmlNodeList nodelist = docXml.GetElementsByTagName("area");
            foreach (XmlNode node in nodelist)
            {
                string code = node.Attributes["code"].Value;
                string name = node.Attributes["name"].Value;
                IdCardHelper.Areas.Add(new string[] { code, name });
            }
        }
        private IdCardHelper(string idCardNumber)
        {
            this.CardNumber = idCardNumber;
            _analysis();
        }
        /// <summary>
        /// 解析身份证
        /// </summary>
        private void _analysis()
        {
            //取省份，地区，区县
            string provCode = CardNumber.Substring(0, 2).PadRight(6, '0');
            string areaCode = CardNumber.Substring(0, 4).PadRight(6, '0');
            string cityCode = CardNumber.Substring(0, 6).PadRight(6, '0');
            for (int i = 0; i < IdCardHelper.Areas.Count; i++)
            {
                if (provCode == IdCardHelper.Areas[i][0])
                    this.Province = IdCardHelper.Areas[i][1];
                if (areaCode == IdCardHelper.Areas[i][0])
                    this.Area = IdCardHelper.Areas[i][1];
                if (cityCode == IdCardHelper.Areas[i][0])
                    this.City = IdCardHelper.Areas[i][1];
                if (Province != null && Area != null && City != null) break;
            }
            //取年龄
            string ageCode = CardNumber.Substring(6, 8);
            try
            {
                int year = Convert.ToInt16(ageCode.Substring(0, 4));
                int month = Convert.ToInt16(ageCode.Substring(4, 2));
                int day = Convert.ToInt16(ageCode.Substring(6, 2));
                Age = new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception("非法的出生日期");
            }
            //取性别
            string orderCode = CardNumber.Substring(14, 3);
            this.Sex = Convert.ToInt16(orderCode) % 2 == 0 ? 0 : 1;
            //生成Json对象
            Json = @"prov:'{0}',area:'{1}',city:'{2}',year:'{3}',month:'{4}',day:'{5}',sex:'{6}',number:'{7}'";
            Console.WriteLine(Json);
            Json = string.Format(Json, Province, Area, City, Age.Year, Age.Month, Age.Day, (Sex == 1 ? "boy" : "gril"), CardNumber);
            Json = "{" + Json + "}";
        }
        #endregion

        #region public get set
        /// <summary>
        /// 所在省份信息
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 所在地区信息
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 所在区县信息
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public DateTime Age { get; set; }

        /// <summary>
        /// 性别，0为女，1为男
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 生成Javascript对象；
        /// </summary>
        public string Json { get; set; }
        #endregion


        #region public static


        /// <summary>
        /// 解析身份证信息
        /// </summary>
        /// <param name="idCardNumber"></param>
        /// <example>
        ///  IDCardNumber card = IDCardNumber.Get(code);
        /// </example>
        /// <returns>IDCardNumber</returns>
        public static IdCardHelper Get(string idCardNumber)
        {
            if (IdCardHelper.Areas.Count < 1)
                IdCardHelper.FillAreas();
            if (!IdCardHelper.CheckIdCardNumber(idCardNumber))
                throw new Exception("非法的身份证号码");
            //
            IdCardHelper cardInfo = new IdCardHelper(idCardNumber);
            return cardInfo;
        }
        /// <summary>
        /// 校验身份证号码是否合法
        /// </summary>
        /// <param name="idCardNumber"></param>
        /// <returns></returns>
        public static bool CheckIdCardNumber(string idCardNumber)
        {
            //正则验证
            Regex rg = new Regex(@"^\d{17}(\d|X)$");
            Match mc = rg.Match(idCardNumber);
            if (!mc.Success) return false;
            //加权码
            string code = idCardNumber.Substring(17, 1);
            double sum = 0;
            string checkCode = null;
            for (int i = 2; i <= 18; i++)
            {
                sum += int.Parse(idCardNumber[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2, i - 1) % 11);
            }
            string[] checkCodes = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];
            if (checkCode != code) return false;
            //
            return true;
        }
        /// <summary>
        /// 随机生成一个身份证号
        /// </summary>
        /// <returns></returns>
        public static IdCardHelper Radom()
        {
            long tick = DateTime.Now.Ticks;
            return new IdCardHelper(_radomCardNumber((int)tick));
        }
        /// <summary>
        /// 批量生成身份证
        /// </summary>
        /// <param name="count"></param>
        /// <example> 
        /// List<IDCardNumber/> list = IDCardNumber.Radom(number);
        /// </example>
        /// <returns></returns>
        public static List<IdCardHelper> Radom(int count)
        {
            List<IdCardHelper> list = new List<IdCardHelper>();
            for (int i = 0; i < count; i++)
            {
                string cardNumber;
                bool isExits;
                do
                {
                    isExits = false;
                    int tick = (int)DateTime.Now.Ticks;
                    cardNumber = IdCardHelper._radomCardNumber(tick * (i + 1));
                    foreach (IdCardHelper c in list)
                    {
                        if (c.CardNumber == cardNumber)
                        {
                            isExits = true;
                            break;
                        }
                    }

                } while (isExits);
                list.Add(new IdCardHelper(cardNumber));
            }
            return list;
        }


        /// <summary>
        /// 生成随身份证号
        /// </summary>
        /// <param name="seed">随机数种子</param>
        /// <returns></returns>
        public static string _radomCardNumber(int seed)
        {
            if (IdCardHelper.Areas.Count < 1)
                IdCardHelper.FillAreas();
            System.Random rd = new System.Random(seed);
            //随机生成发证地
            string area = "";
            do
            {
                area = IdCardHelper.Areas[rd.Next(0, IdCardHelper.Areas.Count - 1)][0];
            } while (area.Substring(4, 2) == "00");
            //随机出生日期
            DateTime birthday = DateTime.Now;
            birthday = birthday.AddYears(-rd.Next(16, 60));
            birthday = birthday.AddMonths(-rd.Next(0, 12));
            birthday = birthday.AddDays(-rd.Next(0, 31));
            //随机码
            string code = rd.Next(1000, 9999).ToString("####");
            //生成完整身份证号
            string codeNumber = area + birthday.ToString("yyyyMMdd") + code;
            double sum = 0;
            string checkCode = null;
            for (int i = 2; i <= 18; i++)
            {
                sum += int.Parse(codeNumber[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2, i - 1) % 11);
            }
            string[] checkCodes = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];
            codeNumber = codeNumber.Substring(0, 17) + checkCode;
            //
            return codeNumber;
        }
        public static string CreateCardNumber(int codel)
        {
            if (IdCardHelper.Areas.Count < 1)
                IdCardHelper.FillAreas();
            System.Random rd = new System.Random(codel);
            //随机生成发证地
            string area = "";
            do
            {
                area = IdCardHelper.Areas[rd.Next(0, IdCardHelper.Areas.Count - 1)][0];
            } while (area.Substring(4, 2) == "00");
            //随机出生日期
            DateTime birthday = DateTime.Now;
            birthday = birthday.AddYears(-rd.Next(16, 60));
            birthday = birthday.AddMonths(-rd.Next(0, 12));
            birthday = birthday.AddDays(-rd.Next(0, 31));
            //随机码
            string code = rd.Next(1000, 9999).ToString("####");
            //生成完整身份证号
            string codeNumber = area + birthday.ToString("yyyyMMdd") + code;
            double sum = 0;
            string checkCode = null;
            for (int i = 2; i <= 18; i++)
            {
                sum += int.Parse(codeNumber[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2, i - 1) % 11);
            }
            string[] checkCodes = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];
            codeNumber = codeNumber.Substring(0, 17) + checkCode;
            //
            return codeNumber;
        }
        #endregion


    }

}
