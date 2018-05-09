using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ItemDoc.Core.Mvc.Html
{
    /// <summary>
    /// This represents the extension methods entity for the <c>System.Web.Mvc.HtmlHelper</c> class.
    /// </summary>
    public static class HtmlHelperImageExtensions
    {
        /// <summary>
        /// Renders the &lt;img&gt; tag.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <returns>Returns the &lt;img&gt; tag in HTML format.</returns>
        public static MvcHtmlString Image(this System.Web.Mvc.HtmlHelper htmlHelper, string src)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            return Image(htmlHelper, src, null);
        }

        /// <summary>
        /// Renders the &lt;img&gt; tag.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="imageAttributes">List of attributes for the {img} tag.</param>
        /// <returns>Returns the &lt;img&gt; tag in HTML format.</returns>
        public static MvcHtmlString Image(this System.Web.Mvc.HtmlHelper htmlHelper, string src, object imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            return Image(htmlHelper, src, System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(imageAttributes));
        }

        /// <summary>
        /// Renders the &lt;img&gt; tag.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="imageAttributes">List of attributes for the {img} tag.</param>
        /// <returns>Returns the &lt;img&gt; tag in HTML format.</returns>
        public static MvcHtmlString Image(this System.Web.Mvc.HtmlHelper htmlHelper, string src, IDictionary<string, object> imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            var tagBuilder = new TagBuilder("img");
            tagBuilder.MergeAttribute("src", src);
            tagBuilder.MergeAttributes(imageAttributes);
            
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}