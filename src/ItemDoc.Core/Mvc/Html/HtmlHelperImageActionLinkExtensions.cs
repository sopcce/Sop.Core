using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ItemDoc.Core.Mvc.Html
{
    /// <summary>
    /// This represents the extension methods entity for the <c>HtmlHelper</c> class.
    /// </summary>
    public static class HtmlHelperImageActionLinkExtensions
    {
        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this HtmlHelper htmlHelper, string src, string actionName)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, null, null, null, null);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, object htmlAttributes, object imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, null, null, htmlAttributes, imageAttributes);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, object routeValues)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, null, routeValues, null, null);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, object routeValues, object htmlAttributes, object imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, null, routeValues, htmlAttributes, imageAttributes);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, IDictionary<string, object> htmlAttributes, IDictionary<string, object> imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, null, null, htmlAttributes, imageAttributes);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, IDictionary<string, object> routeValues)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, null, routeValues, null, null);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, IDictionary<string, object> routeValues, IDictionary<string, object> htmlAttributes, IDictionary<string, object> imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, null, routeValues, htmlAttributes, imageAttributes);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, controllerName, null, null, null);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, object htmlAttributes, object imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, controllerName, null, htmlAttributes, imageAttributes);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, object routeValues)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, controllerName, routeValues, null, null);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, object routeValues, object htmlAttributes, object imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            var actionLink = htmlHelper.ActionLink(".", actionName, controllerName, routeValues, htmlAttributes);
            var image = htmlHelper.Image(src, imageAttributes);
            return new MvcHtmlString(actionLink.ToHtmlString().Replace(">.</a>", ">" + image.ToHtmlString() + "</a>"));
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, IDictionary<string, object> htmlAttributes, IDictionary<string, object> imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, controllerName, null, htmlAttributes, imageAttributes);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, IDictionary<string, object> routeValues)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            return ImageActionLink(htmlHelper, src, actionName, controllerName, routeValues, null, null);
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, IDictionary<string, object> routeValues, IDictionary<string, object> htmlAttributes, IDictionary<string, object> imageAttributes)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            var actionLink = htmlHelper.ActionLink(".", actionName, controllerName, routeValues, htmlAttributes);
            var image = htmlHelper.Image(src, imageAttributes);
            return new MvcHtmlString(actionLink.ToHtmlString().Replace(">.</a>", ">" + image.ToHtmlString() + "</a>"));
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="protocol">HTTP protocal.</param>
        /// <param name="hostName">Hostname.</param>
        /// <param name="fragment">URL fragment - anchor name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>
        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes, object imageAttributes = null)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            if (String.IsNullOrWhiteSpace(protocol))
            {
                throw new ArgumentNullException("protocol");
            }

            if (String.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (String.IsNullOrWhiteSpace(fragment))
            {
                throw new ArgumentNullException("fragment");
            }

            var actionLink = htmlHelper.ActionLink(".", actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
            var image = htmlHelper.Image(src, imageAttributes);
            return new MvcHtmlString(actionLink.ToHtmlString().Replace(">.</a>", ">" + image.ToHtmlString() + "</a>"));
        }

        /// <summary>
        /// Renders the action link containing image.
        /// </summary>
        /// <param name="htmlHelper"><c>HtmlHelper</c> instance.</param>
        /// <param name="src">Image file location.</param>
        /// <param name="actionName">MVC action name.</param>
        /// <param name="controllerName">MVC controller name.</param>
        /// <param name="protocol">HTTP protocal.</param>
        /// <param name="hostName">Hostname.</param>
        /// <param name="fragment">URL fragment - anchor name.</param>
        /// <param name="routeValues">List of the route values.</param>
        /// <param name="htmlAttributes">List of attributes used for the {a} tag.</param>
        /// <param name="imageAttributes">List of attributes used for the {img} tag.</param>
        /// <returns>Returns the action link containing image.</returns>

        public static MvcHtmlString ImageActionLink(this System.Web.Mvc.HtmlHelper htmlHelper, string src, string actionName, string controllerName, string protocol, string hostName, string fragment, IDictionary<string, object> routeValues, IDictionary<string, object> htmlAttributes, IDictionary<string, object> imageAttributes = null)
        {
            if (String.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentNullException("src");
            }

            if (String.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            if (String.IsNullOrWhiteSpace(protocol))
            {
                throw new ArgumentNullException("protocol");
            }

            if (String.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (String.IsNullOrWhiteSpace(fragment))
            {
                throw new ArgumentNullException("fragment");
            }

            var actionLink = htmlHelper.ActionLink(".", actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
            var image = htmlHelper.Image(src, imageAttributes);
            return new MvcHtmlString(actionLink.ToHtmlString().Replace(">.</a>", ">" + image.ToHtmlString() + "</a>"));
        }
    }
}