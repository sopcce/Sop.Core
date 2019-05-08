using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Sop.Core.Spider
{
    public class XmlHelper
    {
        /// <summary>
        /// 获取xml
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static XmlDocument GetXmlByUrl(string url)
        {
            var doc = new XmlDocument();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    string response = sr.ReadToEnd();
                    sr.Close();
                    doc.LoadXml(response);
                }
            }
            return doc;
        }


    }
}
