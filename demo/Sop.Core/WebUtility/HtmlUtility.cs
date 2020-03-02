using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CodeKicker.BBCode;
using HtmlAgilityPack;
using Sop.Framework.Utility;

namespace Sop.Core.WebUtility
{
  /// <summary>
  /// Html工具类
  /// </summary>
  public class HtmlUtility
  {
    /// <summary>
    /// 多行纯文本型转化为可以在HTML中显示
    /// </summary>
    /// <remarks>
    /// 一般在存储到数据库之前进行转化
    /// </remarks>
    /// <param name="plainText">需要转化的纯文本</param>
    /// <param name="keepWhiteSpace">是否保留空格</param>
    public static string FormatMultiLinePlainTextForStorage(string plainText, bool keepWhiteSpace)
    {
      if (string.IsNullOrEmpty(plainText))
        return plainText;

      if (keepWhiteSpace)
      {
        plainText = plainText.Replace(" ", "&nbsp;");
        plainText = plainText.Replace("\t", "&nbsp;&nbsp;");
      }
      plainText = plainText.Replace("\r\n", System.Environment.NewLine);
      plainText = plainText.Replace("\n", System.Environment.NewLine);

      return plainText;
    }

    /// <summary>
    /// 多行纯文本型转化为可以在TextArea中正常显示
    /// </summary>
    /// <remarks>
    /// 一般在进行编辑前进行转化
    /// </remarks>
    /// <param name="plainText">需要转化的纯文本</param>
    /// <param name="keepWhiteSpace">是否保留空格</param>
    public static string FormatMultiLinePlainTextForEdit(string plainText, bool keepWhiteSpace)
    {
      if (string.IsNullOrEmpty(plainText))
        return plainText;

      string result = plainText;
      result = result.Replace(System.Environment.NewLine, "\n");
      if (keepWhiteSpace)
        result = result.Replace("&nbsp;", " ");

      return result;
    }


    /// <summary>
    /// 清除标签名称中的非法字词
    /// </summary>
    public static string CleanTagName(string appKey)
    {
      //Remark:20090808_zhengw 删除Url中可编码的特殊字符：'#','&','=','/','%','?','+', '$',
      string[] parts = appKey.Split('!', '.', '@', '^', '*', '(', ')', '[', ']', '{', '}', '<', '>', ',', '\\', '\'', '~', '`', '|');
      appKey = string.Join("", parts);
      return appKey;
    }

    /// <summary>
    /// 友好的文件大小信息
    /// </summary>
    /// <param name="fileSize">文件字节数</param>
    public static string FormatFriendlyFileSize(double fileSize)
    {
      if (fileSize > 0)
      {
        if (fileSize > 1024 * 1024 * 1024)
          return string.Format("{0:F2}G", (fileSize / (1024 * 1024 * 1024F)));
        else if (fileSize > 1024 * 1024)
          return string.Format("{0:F2}M", (fileSize / (1024 * 1024F)));
        else if (fileSize > 1024)
          return string.Format("{0:F2}K", (fileSize / (1024F)));
        else
          return string.Format("{0:F2}Bytes", fileSize);
      }
      else
        return string.Empty;
    }

    /// <summary>
    /// 格式化评论内容
    /// </summary>
    /// <param name="text">格式化的内容</param>
    /// <param name="enableNoFollow">Should we include the nofollow rel.</param>
    /// <param name="enableConversionToParagraphs">Should newlines be converted to P tags.</param>
    private static string FormatPlainTextComment(string text, bool enableNoFollow = true, bool enableConversionToParagraphs = true)
    {
      if (string.IsNullOrEmpty(text))
        return text;

      text = UrlUtility.Instance().HtmlEncode(text);

      if (enableNoFollow)
      {
        //Find any links
        StringCollection uniqueMatches = new StringCollection();

        string pattern = @"(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
        MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        foreach (Match m in matches)
        {
          if (!uniqueMatches.Contains(m.ToString()))
          {
            text = text.Replace(m.ToString(), "<a rel=\"nofollow\" target=\"_new\" href=\"" + m + "\">" + m.ToString().Trim() + "</a>");
            uniqueMatches.Add(m.ToString());
          }
        }
      }

      // Replace Line breaks with <br> and every other concurrent space with &nbsp; (to allow line breaking)
      if (enableConversionToParagraphs)
        text = ConvertPlainTextToParagraph(text);// text.Replace("\n", "<br />");

      text = text.Replace("  ", " &nbsp;");

      return text;
    }

    /// <summary>
    /// 把纯文字格式化成html段落
    /// </summary>
    /// <remarks>
    /// 使文本在Html中保留换行的格式
    /// </remarks>
    private static string ConvertPlainTextToParagraph(string text)
    {
      if (string.IsNullOrEmpty(text))
        return text;

      text = text.Replace("\r\n", "\n").Replace("\r", "\n");

      string[] lines = text.Split('\n');

      StringBuilder paragraphs = new StringBuilder();

      foreach (string line in lines)
      {
        if (line != null && line.Trim().Length > 0)
          paragraphs.AppendFormat("{0}<br />\n", line);
      }
      return paragraphs.ToString().Remove(paragraphs.ToString().LastIndexOf("<br />", StringComparison.Ordinal));
    }
    /// <summary>
    /// 移除html内的Elemtnts/Attributes及&amp;nbsp;，超过charLimit个字符进行截断
    /// </summary>
    /// <param name="rawHtml">待截字的html字符串</param>
    /// <param name="charLimit">最多允许返回的字符数</param>
    public static string TrimHtml(string rawHtml, int charLimit)
    {
      if (string.IsNullOrEmpty(rawHtml))
      {
        return string.Empty;
      }

      string nohtml = StripHtml(rawHtml, true, false);
      nohtml = StripBBTags(nohtml);

      if (charLimit <= 0 || charLimit >= nohtml.Length)
        return nohtml;
      else
        return StringUtility.Trim(nohtml, charLimit);
    }

    /// <summary>
    /// 移除Html标签
    /// </summary>
    /// <param name="rawString">待处理字符串</param>
    /// <param name="removeHtmlEntities">是否移除Html实体</param>
    /// <param name="enableMultiLine">是否保留换行符（<p/><br/>会转换成换行符）</param>
    /// <returns>返回处理后的字符串</returns>
    public static string StripHtml(string rawString, bool removeHtmlEntities, bool enableMultiLine)
    {
      if (string.IsNullOrEmpty(rawString))
      {
        return rawString;
      }

      string result = rawString;
      if (enableMultiLine)
      {
        result = Regex.Replace(result, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        result = Regex.Replace(result, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      }
      result = result.Replace("\"", "''");
      if (removeHtmlEntities)
      {
        //StripEntities removes the HTML Entities
        result = Regex.Replace(result, "&[^;]*;", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
      }
      return Regex.Replace(result, "<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    /// <summary>
    /// 移除Html用于内容预览
    /// </summary>
    /// <remarks>
    /// 将br、p替换为\n，“'”替换为对应Html实体，并过滤所有Html、Xml、UBB标签
    /// </remarks>
    /// <param name="rawString">用于预览的文本</param>
    /// <returns>返回移除换行及html、ubb标签的字符串</returns>
    public static string StripForPreview(string rawString)
    {
      if (string.IsNullOrEmpty(rawString))
      {
        return rawString;
      }

      string tempString;

      tempString = rawString.Replace("<br>", "\n");
      tempString = tempString.Replace("<br/>", "\n");
      tempString = tempString.Replace("<br />", "\n");
      tempString = tempString.Replace("<p>", "\n");
      tempString = tempString.Replace("'", "&#39;");

      tempString = StripHtml(tempString, false, false);
      tempString = StripBBTags(tempString);

      return tempString;
    }

    /// <summary>
    /// 清除UBB标签
    /// </summary>
    /// <param name="content">待处理的字符串</param>
    /// <remarks>处理后的字符串</remarks>
    public static string StripBBTags(string content)
    {
      if (string.IsNullOrEmpty(content))
      {
        return content;
      }

      return Regex.Replace(content, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// 移除script标签
    /// Helper function used to ensure we don't inject script into the db.
    /// </summary>
    /// <remarks>
    /// 移除&lt;script&gt;及javascript:
    /// </remarks>
    /// <param name="rawString">待处理的字符串</param>
    /// <remarks>处理后的字符串</remarks>
    public static string StripScriptTags(string rawString)
    {
      if (string.IsNullOrEmpty(rawString))
      {
        return rawString;
      }

      // Perform RegEx
      rawString = Regex.Replace(rawString, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      rawString = rawString.Replace("\"javascript:", "\"");

      return rawString;
    }

    /// <summary>
    /// 闭合未闭合的Html标签
    /// </summary>
    /// <returns></returns>
    public static string CloseHtmlTags(string html)
    {
      if (string.IsNullOrEmpty(html))
      {
        return html;
      }

      HtmlDocument doc = new HtmlDocument() { OptionAutoCloseOnEnd = true, OptionWriteEmptyNodes = true };
      doc.LoadHtml(html);

      return doc.DocumentNode.WriteTo();
    }

    #region Clean Html

    /// <summary>
    /// Html标签过滤/清除
    /// </summary>
    /// <remarks>需要在Starter中注册TrustedHtml类，也可以通过重写Basic与HtmlEditor方法来自定义过滤规则</remarks>
    /// <param name="rawHtml">需要处理的Html字符串</param>
    /// <param name="level">受信任Html标签严格程度</param>
    public static string CleanHtml(string rawHtml, TrustedHtmlLevel level)
    {
      if (string.IsNullOrEmpty(rawHtml))
      {
        return rawHtml;
      }

      HtmlDocument doc = new HtmlDocument() { OptionAutoCloseOnEnd = true, OptionWriteEmptyNodes = true };

      TrustedHtml trustedHtml = new TrustedHtml();
      switch (level)
      {
        case TrustedHtmlLevel.Basic:
          trustedHtml = trustedHtml.Basic();
          break;

        case TrustedHtmlLevel.HtmlEditor:
          trustedHtml = trustedHtml.HtmlEditor();
          break;
      }

      doc.LoadHtml(rawHtml);
      HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*");

      if (nodes != null)
      {
        string host = string.Empty;
        if (HttpContext.Current != null)
        {
          host = UrlUtility.Instance().GetHostPath(HttpContext.Current.Request.Url);
        }

        Dictionary<string, string> enforcedAttributes;
        nodes.ToList().ForEach(n =>
        {
          if (trustedHtml.IsSafeTag(n.Name))
          {
            //过滤属性
            n.Attributes.ToList().ForEach(attr =>
            {
              if (!trustedHtml.IsSafeAttribute(n.Name, attr.Name, attr.Value))
              {
                attr.Remove();
              }
              else if (attr.Value.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
              {
                attr.Value = "javascript:;";
              }
            });

            //为标签增加强制添加的属性
            enforcedAttributes = trustedHtml.GetEnforcedAttributes(n.Name);
            if (enforcedAttributes != null)
            {
              foreach (KeyValuePair<string, string> attr in enforcedAttributes)
              {
                if (!n.Attributes.Select(a => a.Name).Contains(attr.Key))
                {
                  n.Attributes.Add(attr.Key, attr.Value);
                }
                else
                {
                  n.Attributes[attr.Key].Value = attr.Value;
                }
              }
            }

            if (n.Name == "a")
            {
              if (n.Attributes.Contains("href"))
              {
                string href = n.Attributes["href"].Value;

                if (href.StartsWith("http://") && !href.ToLowerInvariant().StartsWith(host.ToLower()))
                {
                  if (!n.Attributes.Select(a => a.Name).Contains("rel"))
                  {
                    n.Attributes.Add("rel", "nofollow");
                  }
                  else if (n.Attributes["rel"].Value != "fancybox")
                  {
                    n.Attributes["rel"].Value = "nofollow";
                  }
                }
              }
            }
          }
          else
          {
            if (!trustedHtml.EncodeHtml)
            {
              n.RemoveAll();//移除不允许的Html标签
            }
          }
        });
      }

      return doc.DocumentNode.WriteTo();
    }

    #endregion Clean Html

    #region Get HtmlNode

    /// <summary>
    /// 选择单个Html节点
    /// </summary>
    /// <remarks>选择节点时会自动闭合未闭合的标签</remarks>
    /// <param name="html">要操作的html</param>
    /// <param name="xpath">要选择Html元素的XPath</param>
    public static string GetHtmlNode(string html, string xpath)
    {
      if (string.IsNullOrEmpty(html))
        return html;

      HtmlDocument doc = new HtmlDocument() { OptionAutoCloseOnEnd = true, OptionWriteEmptyNodes = true };
      doc.LoadHtml(html);

      HtmlNode node = doc.DocumentNode.SelectSingleNode(xpath);

      if (node == null)
        return string.Empty;

      return node.OuterHtml;
    }

    /// <summary>
    /// 选择多个Html节点
    /// </summary>
    /// <remarks>选择节点时会自动闭合未闭合的标签</remarks>
    /// <param name="html">要操作的Html</param>
    /// <param name="xpath">要选择Html元素的XPath</param>
    public static List<string> GetHtmlNodes(string html, string xpath)
    {
      if (string.IsNullOrEmpty(html))
        return null;

      HtmlDocument doc = new HtmlDocument() { OptionAutoCloseOnEnd = true, OptionWriteEmptyNodes = true };
      doc.LoadHtml(html);

      HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xpath);

      if (nodes == null)
        return null;

      return nodes.Select(n => n.OuterHtml).ToList();
    }

    #endregion Get HtmlNode

    #region BBCodeParser

    /// <summary>
    /// 将内容中的BBCode转换为对应的Html标签
    /// </summary>
    /// <param name="rawString">需要处理的字符串</param>
    /// <param name="bbTag">bbTag实体</param>
    /// <param name="htmlEncode">是否进行htmlEncode</param>
    public static string BBCodeToHtml(string rawString, BBTag bbTag, bool htmlEncode = false)
    {
      if (string.IsNullOrEmpty(rawString))
        return rawString;

      return BBCodeToHtml(rawString, new List<BBTag>() { bbTag }, htmlEncode);
    }

    /// <summary>
    /// 将内容中的BBCode转换为对应的Html标签
    /// </summary>
    /// <param name="rawString">需要处理的字符串</param>
    /// <param name="bbTags">bbTag实体集合</param>
    /// <param name="htmlEncode">是否进行htmlEncode</param>

    public static string BBCodeToHtml(string rawString, IList<BBTag> bbTags, bool htmlEncode = false)
    {
      if (string.IsNullOrEmpty(rawString) || bbTags == null)
        return rawString;

      var parser = new BBCodeParser(bbTags);
      return parser.ToHtml(rawString);
    }
    /// <summary>
    /// 
    /// </summary>
    static IList<BBTag> _AllowedTags = new List<BBTag> {
      new BBTag("b", "<b>", "</b>"),
      new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
      new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
      new BBTag("code", "<pre class=\"prettyprint\">", "</pre>"),
      new BBTag("img", "<img src=\"${content}\" />", "", false, true),
      new BBTag("quote", "<blockquote>", "</blockquote>"),
      new BBTag("list", "<ul>", "</ul>"),
      new BBTag("*", "<li>", "</li>", true, false),
      new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href"))
    };
    /// <summary>
    /// 
    /// </summary>
    /// <param name="allowedTags"></param>
    /// <param name="appendToDefaults"></param>
    public static void Init(IList<CodeKicker.BBCode.BBTag> allowedTags, bool appendToDefaults = true)
    {
      if (appendToDefaults)
        foreach (var bbTag in allowedTags)
          _AllowedTags.Add(bbTag);
      else
        _AllowedTags = allowedTags;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="encode"></param>
    /// <param name="errorMode"></param>
    /// <param name="allowedTags"></param>
    /// <returns></returns>
    public static object ToHtml(string input, bool encode = false, string errorMode = "ErrorFree", IList<BBTag> allowedTags = null)
    {
      IList<BBTag> tags = _AllowedTags;
      if (allowedTags != null)
        tags = allowedTags;
     
      ErrorMode enErrorMode;
      Enum.TryParse<ErrorMode>(errorMode, out enErrorMode);
      CodeKicker.BBCode.BBCodeParser parser = new CodeKicker.BBCode.BBCodeParser(tags);
      parser = new BBCodeParser(enErrorMode, parser.TextNodeHtmlTemplate, tags);
      if (encode)
        return parser.ToHtml(input);
      else
        return new HtmlString(parser.ToHtml(input));
    }
    #endregion BBCodeParser
  }
}