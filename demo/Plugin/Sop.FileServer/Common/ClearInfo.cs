using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace URun.WXBSnap.Logic
{
    public static class ClearInfo
    {

        /// <summary>
        /// 去除  <em>标签
        /// </summary>
        public static string ClearEM(this string strText)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return "";
            }
            try
            {
                string html = strText;
                //先将符号转换为标签
                html = html.Replace("&lt;", "<").Replace("&gt;", ">");         
                html = Regex.Replace(html, @"[\t\n]", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"[\r]", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"[\n]", "", RegexOptions.IgnoreCase);
                html = html.Replace("<em>", "").Replace("</em>", "");


                return html;
            }
            catch
            {
                return strText;
            }
        }

        /// <summary>
        /// 去除所有html标签（贴吧论坛数据使用）
        /// </summary>
        public static string ClearHtml(this string strText)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return "";
            }
            try
            {
                string html = strText;
                //先将符号转换为标签
                // html = html.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&copy;", "©").Replace("&reg;", "®").Replace("&times;", "×").Replace("&divide;", "÷");
                html = ToText(html).Replace("&#x2F;", "/");

                html = Regex.Replace(html, @"<[^>]+/?>|</[^>]+>", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"<", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @">", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\0", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\t", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\r", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\n", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
                //html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);

                return html;
            }
            catch
            {
                return strText;
            }
        }

        /// <summary>
        /// 去除所有html标签
        /// </summary>
        public static string ClearHtmlScript(this string strText)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return "";
            }
            try
            {
                string html = strText;
                html = Regex.Replace(html, @"<script([\s\S]+?)/script>", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"<style([\s\S]+?)/style>", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"<[^>]+/?>|</[^>]+>", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"<", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @">", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\0", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\t", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\r", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"\n", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
                html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                html = html.Replace("&copy;", "@");


                return html;
            }
            catch
            {
                return strText;
            }
        }

        /// <summary>
        /// 将 HTML 格式的字符串转换成正常显示的字符串(与 ToHtml 方法相反)
        /// </summary>
        /// <param name="sour">被转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ToText(this string sour)
        {
            if (string.IsNullOrEmpty(sour)) return "";

            // 以下逐一转换
            // 先转换百分号
            sour = sour.Replace("&#37;", "%");
            // 小于号,有三种写法
            sour = sour.Replace("&lt;", "<");
            sour = sour.Replace("&LT;", "<");
            sour = sour.Replace("&#60;", "<");
            // 大于号,有三种写法
            sour = sour.Replace("&gt;", ">");
            sour = sour.Replace("&GT;", ">");
            sour = sour.Replace("&#62;", ">");
            // 单引号
            sour = sour.Replace("&#39;", "'");
            sour = sour.Replace("&#43;", "+");
            // 换行符,得用正则替换
            sour = Regex.Replace(sour, @"\n?<[Bb][Rr]\s*/?>\n?", "\n");
            // 双引号,有三种写法
            sour = sour.Replace("&quot;", "\"");
            sour = sour.Replace("&QUOT;", "\"");
            sour = sour.Replace("&#34;", "\"");
            // 空格,只有两种写法, &NBSP; 浏览器不承认
            sour = sour.Replace("&nbsp;", " ");
            sour = sour.Replace("&#160;", " ");
            // 中文引号
            sour = sour.Replace("&ldquo;", "“");
            sour = sour.Replace("&rdquo;", "”");
            // & 符號,最后才转换
            sour = sour.Replace("&amp;", "&");
            sour = sour.Replace("&AMP;", "&");
            sour = sour.Replace("&#38;", "&");
            sour = sour.Replace("&#167;", "§");
            return sour;
        }

    }
}
