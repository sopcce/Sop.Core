using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sop.Core.Mvc.Html
{
    /// <summary>
    /// This represents the extension methods entity for the <c>HtmlHelper</c> class.
    /// </summary>
    public static class HtmlHelperImageLinkExtensions
    {
        /// <summary>
        /// Renders the &lt;a&gt; tag with image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="href">Link URL.</param>
        /// <returns>Returns the &lt;a&gt; tag with image.</returns>
        public static MvcHtmlString ImageLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string href)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentNullException("href");
            }

            return ImageLink(htmlHelper, src, href, null, null);
        }

        /// <summary>
        /// Renders the &lt;a&gt; tag with image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="href">Link URL.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes for the {img} tag.</param>
        /// <returns>Returns the &lt;a&gt; tag with image.</returns>
        public static MvcHtmlString ImageLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string href, object htmlAttributes, object imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentNullException("href");
            }

            return ImageLink(htmlHelper, src, href, System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(imageAttributes));
        }

        /// <summary>
        /// Renders the &lt;a&gt; tag with image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="href">Link URL.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes for the {img} tag.</param>
        /// <returns>Returns the &lt;a&gt; tag with image.</returns>
        public static MvcHtmlString ImageLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string href, IDictionary<string, object> htmlAttributes, IDictionary<string, object> imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentNullException("href");
            }

            var link = htmlHelper.Link(".", href, htmlAttributes);
            var image = htmlHelper.Image(src, imageAttributes);
            return new MvcHtmlString(link.ToHtmlString().Replace(">.</a>", ">" + image.ToHtmlString() + "</a>"));
        }
    }
}