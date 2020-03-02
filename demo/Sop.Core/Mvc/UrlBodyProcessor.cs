using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sop.Core.Mvc
{
    /// <summary>
    /// 解析URL
    /// </summary>
    public class UrlBodyProcessor
    {
        /// <summary>
        /// 解析URL
        /// </summary>
        /// <param name="body">存URL内容</param>
        /// <returns></returns>
        public string Process(string body)
        {
            if (string.IsNullOrEmpty(body) || !(body.Contains("http://") || body.Contains("https://") || body.Contains("ftp://")))
            {
                return body;
            }
            string regexRule = @"(http|ftp|https):\/\/[^(\r\n)^ ^\,^\u3002^\uff1b^\uff0c^\uff1a^\u201c^\u201d^\uff08^\uff09^\u3001^\uff1f^\u300a^\u300b^\<]*";

            string url = string.Empty;
            List<string> urls = new List<string>();
            Regex rg = new Regex(regexRule, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matches = rg.Matches(body);

            if (matches != null)
            {
                foreach (Match m in matches)
                {
                    if (string.IsNullOrEmpty(m.Value))
                        continue;

                    url = m.Value;
                    if (!string.IsNullOrEmpty(url))
                    {
                        urls.Add(url);
                    }
                }
            }

            foreach (var strUrl in urls)
            {
                body = body.Replace(strUrl, string.Format("<a class='font-bold hby-operation' href=\"{0}\" target=\"_blank\">{0}</a>", strUrl));
            }
            return body;
        }
    }
}
