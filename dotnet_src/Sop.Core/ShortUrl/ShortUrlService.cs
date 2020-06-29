using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Sop.Core
{
    public class ShortUrlService : IShortUrlService
    {

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string MD5(string str)
        {
            byte[] array = System.Text.Encoding.UTF8.GetBytes(str);
            array = new MD5CryptoServiceProvider().ComputeHash(array);
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text += array[i].ToString("x").PadLeft(2, '0');
            }
            return text;
        }


        /// <summary>
        /// 获取Url别名
        /// </summary>
        /// <param name="url">需要处理的Url</param>
        /// <param name="shortUrlCount"></param>
        /// <returns>返回Url别名</returns>
        public string[] GetShortUrl(string url, int shortUrlCount = 4)
        {
            var sdsd = ConfigurationManager.GetSection<AppSettings>("AppSettings");
            ;
            var ds = ConfigurationManager.GetAppSettings();
            if (shortUrlCount < 4)
                shortUrlCount = 4;
            //要使用生成URL的字符
            string[] chars = new string[]{
                "a","b","c","d","e","f","g","h",
                "i","j","k","l","m","n","o","p",
                "q","r","s","t","u","v","w","x",
                "y","z","0","1","2","3","4","5",
                "6","7","8","9","A","B","C","D",
                "E","F","G","H","I","J","K","L",
                "M","N","O","P","Q","R","S","T",
                "U","V","W","X","Y","Z"
            };
            //对传入网址进行MD5加密
            string urlSalt = MD5(url);
            string[] aliases = new string[shortUrlCount];
            for (int i = 0; i < shortUrlCount; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                int int32 = 0x3FFFFFFF & Convert.ToInt32("0x" + urlSalt.Substring(i * 8, 8), 16);
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & int32;
                    //把取得的字符相加
                    aliases[i] += chars[index];
                    //每次循环按位右移5位
                    int32 = int32 >> 5;
                }
            }
            return aliases;
        }
        



        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Url(string url)
        {
            Regex re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
            MatchCollection mc = re.Matches(url);
            foreach (Match m in mc)
            {
                url = url.Replace(m.Result("${url}"),
                    newValue: string.Format("<a href='{0}'>{0}</a>", m.Result("${url}")));
            }
            return url;
        }

    }
}