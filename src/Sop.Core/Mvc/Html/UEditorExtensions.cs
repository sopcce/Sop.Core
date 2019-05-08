//<http://www.sopcce.com>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2016-7-8</createdate>
//<author>郭家秋</author>
//<email>sopcce@qq.com</email>
//<log date="2016-7-8" version="0.5">新建</log>
//--------------------------------------------------------------
//<http://www.sopcce.com>
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Web.Mvc.Html;

namespace Sop.Core.Mvc.Html
{
    /// <summary>
    /// UEditor的HtmlHelper输出方法
    /// </summary>
    public static class UEditorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditor(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromStringExpression(name, htmlHelper.ViewContext.ViewData);
            if (value != null)
            {
                modelMetadata.Model = value;
            }

            return UEditorHelper(htmlHelper, modelMetadata, name, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditor(this HtmlHelper htmlHelper, string name, string value, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromStringExpression(name, htmlHelper.ViewContext.ViewData);
            if (value != null)
            {
                modelMetadata.Model = value;
            }
            return UEditorHelper(htmlHelper, modelMetadata, name, htmlAttributes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return UEditorHelper(htmlHelper, ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression), htmlAttributes);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="modelMetadata"></param>
        /// <param name="name"></param>
        /// <param name="rowsAndColumns"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        internal static MvcHtmlString UEditorHelper(HtmlHelper htmlHelper, ModelMetadata modelMetadata, string name, IDictionary<string, object> htmlAttributes)
        {
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullHtmlFieldName))
            {
                throw new ArgumentException("名称不存在");
            }
            TagBuilder tagBuilder = new TagBuilder("textarea");
            tagBuilder.GenerateId(fullHtmlFieldName);
            tagBuilder.MergeAttributes<string, object>(htmlAttributes, true);
            tagBuilder.MergeAttribute("name", fullHtmlFieldName, true);
            tagBuilder.MergeAttribute("id", fullHtmlFieldName, true);
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out modelState) && modelState.Errors.Count > 0)
            {
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }
            tagBuilder.MergeAttributes<string, object>(htmlHelper.GetUnobtrusiveValidationAttributes(name, modelMetadata));
            string s;
            if (modelState != null && modelState.Value != null)
            {
                s = modelState.Value.AttemptedValue;
            }
            else if (modelMetadata.Model != null)
            {
                s = modelMetadata.Model.ToString();
            }
            else
            {
                s = string.Empty;
            }
            tagBuilder.SetInnerText(Environment.NewLine + System.Web.HttpUtility.HtmlEncode(s) );

            return new MvcHtmlString(tagBuilder.ToString());
        }

        // var name = String.Join(".", GetMembersOnPath(expression.Body as MemberExpression).Select(m => m.Member.Name).Reverse());
        /// <summary>
        /// MemberExpression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns> 
        private static IEnumerable<MemberExpression> GetMembersOnPath(MemberExpression expression)
        {
            while (expression != null)
            {
                yield return expression;
                expression = expression.Expression as MemberExpression;
            }
        }


    }
}
