using System.Web.Mvc;
using System.Web.Routing;

namespace Sop.Core.Mvc.Html
{
    /// <summary>
    /// 封装对HtmlHelper的扩展方法
    /// </summary>
    public static class CheckboxExtensions
    {
        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="htmlhelper">被扩展对象</param>
        /// <param name="name">名称</param>
        /// <param name="value">复选框对应的值</param>
        /// <param name="isChecked">是否选中</param>
        /// <param name="htmlAttributes">html属性</param>
        /// <returns></returns>
        public static MvcHtmlString SimpleCheckBox(this System.Web.Mvc.HtmlHelper htmlhelper, string name, object value, bool isChecked = false, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", name);

            if (value != null)
                tagBuilder.MergeAttribute("value", value.ToString());

            if (isChecked)
                tagBuilder.MergeAttribute("checked", "checked");

            if (htmlAttributes != null)
            {
                RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                tagBuilder.MergeAttributes(attributes, false);
            }

            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}