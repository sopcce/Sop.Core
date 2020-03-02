using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sop.Core.Mvc.Html
{
    /// <summary>
    /// This represents the extension methods entity for the <c>HtmlHelper</c> class.
    /// </summary>
    public static class HtmlHelperLinkExtensions
    {
        /// <summary>
        /// Renders the &lt;a&gt; tag.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="linkText">Link text.</param>
        /// <param name="href">Link URL.</param>
        /// <returns>Returns the &lt;a&gt; tag.</returns>
        public static MvcHtmlString Link(this System.Web.Mvc.HtmlHelper htmlHelper, string linkText, string href)
        {
            if (String.IsNullOrWhiteSpace(linkText))
            {
                throw new ArgumentNullException("linkText");
            }

            if (String.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentNullException("href");
            }

            return Link(htmlHelper, linkText, href, null);
        }

        /// <summary>
        /// Renders the &lt;a&gt; tag.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="linkText">Link text.</param>
        /// <param name="href">Link URL.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <returns>Returns the &lt;a&gt; tag.</returns>
        public static MvcHtmlString Link(this System.Web.Mvc.HtmlHelper htmlHelper, string linkText, string href, object htmlAttributes)
        {
            if (String.IsNullOrWhiteSpace(linkText))
            {
                throw new ArgumentNullException("linkText");
            }

            if (String.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentNullException("href");
            }

            return Link(htmlHelper, linkText, href, System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Renders the &lt;a&gt; tag.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="linkText">Link text.</param>
        /// <param name="href">Link URL.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <returns>Returns the &lt;a&gt; tag.</returns>
        public static MvcHtmlString Link(this System.Web.Mvc.HtmlHelper htmlHelper, string linkText, string href, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrWhiteSpace(linkText))
            {
                throw new ArgumentNullException("linkText");
            }

            if (String.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentNullException("href");
            }

            var tagBuilder = new TagBuilder("a")
                                 {
                                     InnerHtml = linkText
                                 };
            tagBuilder.MergeAttribute("href", href);
            tagBuilder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.EndTag));
           
        }
    }
}