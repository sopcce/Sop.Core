using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ShortUrls
{
    public class ShortUrlHelper
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
        /// <returns>返回Url别名</returns>
        public string[] GetShortUrl(string url, int shorturlCount = 4)
        {
            if (shorturlCount < 4)
                shorturlCount = 4;
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
            string[] aliases = new string[shorturlCount];
            for (int i = 0; i < shorturlCount; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + urlSalt.Substring(i * 8, 8), 16);
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加
                    aliases[i] += chars[index];
                    //每次循环按位右移5位
                    hexint = hexint >> 5;
                }
            }
            return aliases;
        }


        /// <summary>
        /// 获取Url别名
        /// </summary>
        /// <param name="url">需要处理的Url</param>
        /// <param name="type"></param>
        /// <returns>返回Url别名</returns>
        public string GetApiShortUrl(string url, ShortApiUrlType type = ShortApiUrlType.dwn_cn)
        {
            try
            {
                string strPost;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetShortApiUrl(type, out strPost));
                string post = string.Format(strPost, System.Net.WebUtility.UrlEncode(url));
                request.ServicePoint.Expect100Continue = true;
                request.Method = "post";
                request.UserAgent = "toolbar";
                request.ContentLength = post.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Cache-Control", "no-cache");
                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    string encoding = response.ContentEncoding;
                    if (encoding == null || encoding.Length < 1)
                    {
                        encoding = "UTF-8"; //默认编码  
                    }
                    using (StreamReader responseReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        url = responseReader.ReadToEnd();
                    }
                }
               
                return url;
            }
            catch
            {
                return url;
            }
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
                String.Format("<a href='{0}'>{0}</a>", m.Result("${url}")));
            }
            return url;
        }
        private string GetShortApiUrl(ShortApiUrlType type, out string strPost)
        {
            string result = "";
            switch (type)
            {
                case ShortApiUrlType.goo_gl:
                    result = "http://goo.gl/api/url";
                    strPost = "&user=toolbar@google.com&url={0}";
                    break;
                case ShortApiUrlType.dwn_cn:
                    result = "http://dwz.cn/create.php";
                    strPost = "&url={0}";
                    break;
                case ShortApiUrlType.t_cn:
                    //xml:http://api.t.sina.com.cn/short_url/shorten.xml
                    //json:http://api.t.sina.com.cn/short_url/shorten.json
                    //source:应用的appkey
                    //url_long:需要转换的长链接
                    result = "http://api.t.sina.com.cn/short_url/shorten.json?source=1338661855";
                    strPost = "&url_long={0}";
                    break;

                default:
                    strPost = null;
                    break;
            }
            return result;
        }
    }

    /// <summary>
    /// 连接的类型
    /// </summary>
    public enum ShortApiUrlType
    {
        /// <summary>
        /// 
        /// </summary>
        goo_gl = 0,
        /// <summary>
        /// 
        /// </summary>
        dwn_cn = 2,

        t_cn = 3,



    }



}