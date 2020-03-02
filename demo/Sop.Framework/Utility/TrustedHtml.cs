using System;
using System.Collections.Generic;

namespace Sop.Framework.Utility
{
    /// <summary>
    /// Html标签过滤配置类
    /// </summary>
    public sealed class TrustedHtml
    {
        private HashSet<string> _tagNames; // 受信任的标签名
        private HashSet<string> _globalAttributes; //全局受信任的属性
        private Dictionary<string, HashSet<string>> _attributes; //受信任的属性名
        private Dictionary<string, Dictionary<string, string>> _enforcedAttributes; //必需的属性名
        private Dictionary<string, Dictionary<string, HashSet<string>>> _protocols; //属性中被允许的url协议

        /// <summary>
        /// 已经添加的规则
        /// </summary>
        private static Dictionary<TrustedHtmlLevel, TrustedHtml> addedRules = new Dictionary<TrustedHtmlLevel, TrustedHtml>();

        private bool _encodeHtml = false;

        /// <summary>
        /// 是否需要Html编码
        /// </summary>
        public bool EncodeHtml
        {
            get { return _encodeHtml; }
        }

        #region 构造器

        /// <summary>
        /// 构造函数
        /// </summary>
        public TrustedHtml()
            : this(false)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encodeHtml">是否需要htmlencode</param>
        public TrustedHtml(bool encodeHtml)
        {
            _encodeHtml = encodeHtml;

            _tagNames = new HashSet<string>();
            _globalAttributes = new HashSet<string>();
            _attributes = new Dictionary<string, HashSet<string>>();
            _enforcedAttributes = new Dictionary<string, Dictionary<string, string>>();
            _protocols = new Dictionary<string, Dictionary<string, HashSet<string>>>();
        }

        #endregion 构造器

        #region 验证规则

        /// <summary>
        /// 普通受信任标签
        /// </summary>
        public TrustedHtml Basic()
        {
            if (!addedRules.ContainsKey(TrustedHtmlLevel.Basic))
            {
                addedRules[TrustedHtmlLevel.Basic] = new TrustedHtml(_encodeHtml)
                           .AddTags("strong", "em", "u", "b", "i", "font", "ul", "ol", "li",
                                    "p", "address", "div", "hr", "br", "a", "span", "img")

                           .AddGlobalAttributes("align", "style")

                           .AddAttributes("font", "size", "color", "face")

                           .AddAttributes("p", "dir")
                           .AddAttributes("a", "href", "title", "name", "target", "rel")
                           .AddAttributes("img", "src", "alt", "title", "border", "width", "height")

                           .AddProtocols("a", "href", "ftp", "http", "https", "mailto");
            }

            return addedRules[TrustedHtmlLevel.Basic];
        }

        /// <summary>
        /// 编辑器中受信任的标签
        /// </summary>
        public TrustedHtml HtmlEditor()
        {
            if (!addedRules.ContainsKey(TrustedHtmlLevel.HtmlEditor))
            {
                addedRules[TrustedHtmlLevel.HtmlEditor] = new TrustedHtml(_encodeHtml)
                           .AddTags("h1", "h2", "h3", "h4", "h5", "h6", "h7", "strong", "em", "u", "b", "i",
                                   "strike", "sub", "sup", "font", "blockquote", "ul", "ol", "li", "p",
                                   "address", "div", "hr", "br", "a", "span", "img", "table", "tbody", "th",
                                   "td", "tr", "pre", "code", "xmp", "object", "param", "embed", "iframe")

                           .AddGlobalAttributes("align", "id", "style")

                           .AddAttributes("font", "size", "color", "face")
                           .AddAttributes("blockquote", "dir")
                           .AddAttributes("p", "dir")
                           .AddAttributes("a", "href", "title", "name", "target", "rel")
                           .AddAttributes("img", "src", "alt", "title", "border", "width", "height")
                           .AddAttributes("table", "border", "cellpadding", "cellspacing", "bgcorlor", "width")
                           .AddAttributes("th", "bgcolor", "width")
                           .AddAttributes("td", "rowspan", "colspan", "bgcolor", "width")
                           .AddAttributes("pre", "name", "class")
                           .AddAttributes("object", "classid", "codebase", "width", "height", "data", "type")
                           .AddAttributes("param", "name", "value")
                           .AddAttributes("embed", "type", "src", "width", "height", "quality", "scale",
                                          "bgcolor", "vspace", "hspace", "base", "flashvars", "swliveconnect")
                           .AddAttributes("iframe", "src", "frameborder", "width", "height")

                           .AddProtocols("a", "href", "ftp", "http", "https", "mailto")
                           .AddProtocols("blockquote", "cite", "http", "https")
                           .AddProtocols("cite", "cite", "http", "https");
            }

            return addedRules[TrustedHtmlLevel.HtmlEditor];
        }

        #endregion 验证规则

        #region add rules methods

        /// <summary>
        /// 添加受信任的标签验证规则
        /// </summary>
        /// <param name="tags">受信任的标签</param>
        public TrustedHtml AddTags(params string[] tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException("tags");
            }

            foreach (string tagName in tags)
            {
                if (string.IsNullOrEmpty(tagName))
                {
                    throw new Exception("An empty tag was found.");
                }
                _tagNames.Add(tagName);
            }
            return this;
        }

        /// <summary>
        /// 添加受信任的标签属性规则
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <param name="keys">标签的受信任属性</param>
        public TrustedHtml AddAttributes(string tag, params string[] keys)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }

            HashSet<string> attributeSet = new HashSet<string>();
            foreach (string key in keys)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new Exception("key");
                }

                attributeSet.Add(key);
            }
            if (_attributes.ContainsKey(tag))
            {
                foreach (string item in attributeSet)
                {
                    _attributes[tag].Add(item);
                }
            }
            else
            {
                _attributes.Add(tag, attributeSet);
            }
            return this;
        }

        /// <summary>
        /// 添加全局受信任的属性
        /// </summary>
        /// <param name="attrs">属性名</param>
        public TrustedHtml AddGlobalAttributes(params string[] attrs)
        {
            if (attrs == null)
            {
                throw new ArgumentNullException("attributes");
            }

            foreach (string attrName in attrs)
            {
                if (string.IsNullOrEmpty(attrName))
                {
                    throw new Exception("An empty attribute was found.");
                }
                _globalAttributes.Add(attrName);
            }

            return this;
        }

        /// <summary>
        /// 添加必须存在的标签属性规则
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <param name="key">属性名</param>
        /// <param name="value">属性值</param>
        public TrustedHtml AddEnforcedAttribute(string tag, string key, string value)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            if (_enforcedAttributes.ContainsKey(tag))
            {
                _enforcedAttributes[tag].Add(key, value);
            }
            else
            {
                Dictionary<string, string> attrMap = new Dictionary<string, string>();
                attrMap.Add(key, value);
                _enforcedAttributes.Add(tag, attrMap);
            }
            return this;
        }

        /// <summary>
        /// 添加标签中被允许协议的Url规则
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <param name="key">属性名</param>
        /// <param name="protocols">被允许的Url协议</param>
        public TrustedHtml AddProtocols(string tag, string key, params string[] protocols)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            if (protocols == null)
            {
                throw new ArgumentNullException("protocols");
            }

            Dictionary<string, HashSet<string>> attrMap;
            HashSet<string> protSet;

            if (this._protocols.ContainsKey(tag))
            {
                attrMap = this._protocols[tag];
            }
            else
            {
                attrMap = new Dictionary<string, HashSet<string>>();
                this._protocols.Add(tag, attrMap);
            }
            if (attrMap.ContainsKey(key))
            {
                protSet = attrMap[key];
            }
            else
            {
                protSet = new HashSet<string>();
                attrMap.Add(key, protSet);
            }
            foreach (string protocol in protocols)
            {
                if (string.IsNullOrEmpty(protocol))
                {
                    throw new Exception("protocol is empty.");
                }

                protSet.Add(protocol);
            }
            return this;
        }

        #endregion add rules methods

        /// <summary>
        ///  获取强制添加的标签属性
        /// </summary>
        /// <param name="tag">当前标签名</param>
        public Dictionary<string, string> GetEnforcedAttributes(string tag)
        {
            if (_enforcedAttributes.ContainsKey(tag))
            {
                return _enforcedAttributes[tag];
            }
            return null;
        }

        /// <summary>
        /// 判断标签是否被信任
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <returns>true被信任，false反之</returns>
        public bool IsSafeTag(string tag)
        {
            return _tagNames.Contains(tag);
        }

        /// <summary>
        /// 验证标签属性是否被信任
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <param name="attr">属性名</param>
        /// <param name="attrVal">属性值</param>
        /// <returns>true为被信任，false反之</returns>
        public bool IsSafeAttribute(string tag, string attr, string attrVal)
        {
            if (_globalAttributes.Contains(attr) || (_attributes.ContainsKey(tag) && _attributes[tag].Contains(attr) || attr.StartsWith("data-")))
            {
                if (_protocols.ContainsKey(tag))
                    return !_protocols[tag].ContainsKey(attr) || ValidProtocol(tag, attr, attrVal);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 验证标签实行中的Url是否符合限制条件
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <param name="attr">属性名</param>
        /// <param name="attVal">属性值</param>
        /// <returns>true为符合限制的，false反之</returns>
        private bool ValidProtocol(string tag, string attr, string attVal)
        {
            if (!attVal.Contains("://"))
                return true;
            foreach (string protocol in _protocols[tag][attr])
            {
                string prot = protocol + ":";
                if (attVal.ToLowerInvariant().StartsWith(prot))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// 受信任Html标签的严格程度
    /// </summary>
    public enum TrustedHtmlLevel
    {
        /// <summary>
        /// 普通受信任标签
        /// </summary>
        /// <remarks>
        /// <para>允许使用个别html标签，例如：</para>
        /// <list type="bullet">
        /// <item>无Html编辑器的评论表单</item>
        /// </list>
        /// </remarks>
        Basic = 0,

        /// <summary>
        /// 针对于Html编辑器的受信任标签
        /// </summary>
        HtmlEditor = 1
    }
}