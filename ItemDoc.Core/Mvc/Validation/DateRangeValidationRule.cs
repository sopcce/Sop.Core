using System;
using System.Web.Mvc;

namespace ItemDoc.Core.Mvc
{
    /// <summary>
    /// 日期范围客户端规则
    /// </summary>
    public class DateRangeValidationRule : ModelClientValidationRule
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="errorMessage">出错信息</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        public DateRangeValidationRule(string errorMessage, DateTime? minValue, DateTime? maxValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = "rangedate";
            if (minValue.HasValue)
                ValidationParameters["min"] = minValue.Value.ToString("yyyy-MM-dd");
            if (maxValue.HasValue)
                ValidationParameters["max"] = maxValue.Value.ToString("yyyy-MM-dd");
        }
    }
}