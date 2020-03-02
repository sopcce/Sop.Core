using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace Sop.Core.Mvc.Html
{
  /// <summary>
  /// 联动下拉列表
  /// </summary>
  public static class DropDownListExtensions
    {
        

        
        /// <summary>
        /// 联动下拉列表
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="expression">获取数据集合</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="rootItems">获取根级列表数据</param>
        /// <param name="getParentID">获取列表项的ParentID方法</param>
        /// <param name="getChildItems">获取子级列表数据集合方法</param>
        /// <param name="optionLabel"></param>
        /// <returns>html代码</returns>
        public static MvcHtmlString DropDownListLinkageFor<TModel, TProperty>(
                this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression,
                TProperty defaultValue,
                Dictionary<TProperty, string> rootItems,
                Func<TProperty, TProperty> getParentID,
                Func<TProperty, Dictionary<TProperty, string>> getChildItems,
                string optionLabel = "请选择")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return DropDownListLinkage(htmlHelper,
                                 ExpressionHelper.GetExpressionText(expression),
                                (TProperty)metadata.Model,
                                defaultValue,
                                rootItems,
                                getParentID,
                                getChildItems,
                                optionLabel);
        }

        /// <summary>
        /// 联动下拉列表
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="name">下拉列表表单项名</param>
        /// <param name="selectedValue">当前选中值</param>
        /// <param name="defaultValue">选中的默认值</param>
        /// <param name="rootItems">获取根级列表数据</param>
        /// <param name="getParentId">获取列表项的ParentID方法</param>
        /// <param name="getChildItems">获取子级列表数据集合方法</param>
        /// <param name="optionLabel"></param>
        /// <returns>html代码</returns>
        public static MvcHtmlString DropDownListLinkage<TProperty>(
            this HtmlHelper htmlHelper,
            string name,
            TProperty selectedValue,
            TProperty defaultValue,
            Dictionary<TProperty, string> rootItems,
            Func<TProperty, TProperty> getParentId,
            Func<TProperty, Dictionary<TProperty, string>> getChildItems,
            string optionLabel = "请选择")
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            //select data init
            Stack<Dictionary<TProperty, string>> stack = new Stack<Dictionary<TProperty, string>>();

            //如果有选中的值，则查找其所在列表前面的所有列表
            IList<TProperty> selectedValues = new List<TProperty>();
            if (selectedValue != null && !selectedValue.Equals(defaultValue))
            {
                TProperty itemId = selectedValue;

                TProperty parentItemId = getParentId(itemId);
                while (!itemId.Equals(defaultValue) && parentItemId.Equals(defaultValue))
                {
                    stack.Push(getChildItems(parentItemId));
                    selectedValues.Add(itemId);
                    itemId = parentItemId;
                    parentItemId = getParentId(itemId);
                }
                if (rootItems.Count() > 0)
                {
                    TProperty rootId = getParentId(rootItems.First().Key);
                    if (!itemId.Equals(rootId))
                    {
                        stack.Push(rootItems);
                        selectedValues.Add(itemId);
                    }
                }
            }
            else
            {
                TProperty rootItemID = rootItems.Select(n => n.Key).FirstOrDefault();
                stack.Push(rootItems);
            }
            //生成标签
            TagBuilder containerBuilder = new TagBuilder("span");
            containerBuilder.MergeAttribute("plugin", "linkageDropDownList");
            containerBuilder.MergeAttribute("class", "form-inline");
            var data = new Dictionary<string, object>();
            data.TryAdd("ControlName", name);
            data.TryAdd("DefaultValue", defaultValue.ToString());
            containerBuilder.MergeAttribute("data", JsonConvert.SerializeObject(data));

            int currentIndex = 0;
            while (stack.Count > 0)
            {
                Dictionary<TProperty, string> dictionary = stack.Pop();
                IEnumerable<SelectListItem> selectList = dictionary.Select(n => new SelectListItem()
                {
                    Selected = selectedValues.Contains(n.Key),
                    Text = n.Value,
                    Value = n.Key.ToString()
                });
                containerBuilder.InnerHtml += "\r\n" + htmlHelper.DropDownList(string.Format("{0}_{1}", name, currentIndex), selectList,
                                optionLabel, new { @class = "tn-dropdownlist form-control cms-floder-list" });
                currentIndex++;
            }
            containerBuilder.InnerHtml += "\r\n" + htmlHelper.Hidden(name);
            return MvcHtmlString.Create(containerBuilder.ToString());
        }


    }
}






