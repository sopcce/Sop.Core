using System;
using System.Web.Mvc;
using ItemDoc.Framework.Utilities;
using ItemDoc.Framework.Utility;


namespace ItemDoc.Core.Mvc.ModelBinder
{
    /// <summary>
    /// 自定义ModelBinder，支持用户输入内容的自动过滤
    /// </summary>
    public class CustomModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //输入内容是字符串或字符串数组
            if (bindingContext.ModelType.FullName.Contains("System.String"))
            {
                bool cleanHtml = false;

                //如果模型绑定的字段加了AllowHtmlAttribute，则bindingContext.ModelMetadata.RequestValidationEnabled=false，仍需要过滤掉不安全的Html标签
                if (!bindingContext.ModelMetadata.RequestValidationEnabled)
                {
                    cleanHtml = true;
                }

                //先禁止.net对请求内容的自动安全校验
                bindingContext.ModelMetadata.RequestValidationEnabled = false;
                var value = base.BindModel(controllerContext, bindingContext);

                if (value == null)
                {
                    return value;
                }

                if (value is Array)
                {
                    string[] tempArray = (string[])value;

                    if (tempArray != null && tempArray.Length > 0)
                    {
                        for (int i = 0; i < tempArray.Length; i++)
                        {
                            tempArray[i] = this.formatInputValue(tempArray[i], cleanHtml);
                        }
                    }

                    return tempArray;
                }
                else
                {
                    return this.formatInputValue(value as string, cleanHtml);
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }

        /// <summary>
        /// 对输入的字符串内容进行格式化处理
        /// </summary>
        /// <param name="inputValue">输入的字符串内容</param>
        /// <param name="cleanHtml">是否需要清理不安全的Html标签</param>
        /// <returns>格式化后的字符串内容</returns>
        private string formatInputValue(string inputValue, bool cleanHtml)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                return inputValue;
            }

            if (cleanHtml)
            {
                inputValue = HtmlUtility.CleanHtml(inputValue, TrustedHtmlLevel.HtmlEditor);
            }
            else
            {
                inputValue = HtmlUtility.StripHtml(inputValue, true, false);

                //处理多行纯文本
                inputValue = HtmlUtility.FormatMultiLinePlainTextForStorage(inputValue, false);
            }

            return inputValue;
        }

       
    }
}