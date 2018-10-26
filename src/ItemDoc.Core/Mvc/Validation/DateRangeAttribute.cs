using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;


namespace ItemDoc.Core.Mvc
{
    /// <summary>
    /// 验证日期确保在某一范围内
    /// </summary>
    /// <remarks>
    /// 至少指定最大值和最小值中的一个
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DateRangeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DateFormat = "yyyy-MM-dd";

        private static class DefaultErrorMessages
        {
            public const string Range = "'{0}' 必须是在 {1:d} 和 {2:d} 之间的日期";
            public const string Min = "'{0}' 必须大于或等于 {1:d}";
            public const string Max = "'{0}' 必须小于或等于 {2:d}";
        }

        private DateTime? _minDate = null;
        private DateTime? _maxDate = null;

        /// <summary>
        /// 日期最小值
        /// </summary>
        public string MinDate
        {
            get
            {
                return _minDate == null ? string.Empty : _minDate.Value.ToString(DateFormat);
            }
            set
            {
                _minDate = ParseDate(value);
            }
        }

        /// <summary>
        /// 日期最大值
        /// </summary>
        public string MaxDate
        {
            get
            {
                return _maxDate == null ? string.Empty : _maxDate.Value.ToString(DateFormat);
            }
            set
            {
                _maxDate = ParseDate(value);
            }
        }

        /// <summary>
        /// 服务器端验证
        /// </summary>
        public override bool IsValid(object value)
        {
            if (value == null || !(value is DateTime))
            {
                return true;
            }
            DateTime dateValue = (DateTime)value;
            return (_minDate ?? DateTime.MinValue) <= dateValue && dateValue <= (_maxDate ?? DateTime.MaxValue);
        }

        /// <summary>
        /// 格式化ErrorMessage
        /// </summary>
        /// <param name="name">表单项名</param>
        /// <returns>格式式处理过的ErrorMessage</returns>
        public override string FormatErrorMessage(string name)
        {
            EnsureErrorMessage();
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                name, _minDate, _maxDate);
        }

        /// <summary>
        /// 获取客户端验证规则
        /// </summary>
        /// <param name="metadata">Model元数据</param>
        /// <param name="context">控制器上下文</param>
        /// <returns>客户端验证规则集合</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]
                       {
                           new DateRangeValidationRule(FormatErrorMessage(metadata.GetDisplayName()), _minDate,
                                                              _maxDate)
                       };
        }

        /// <summary>
        /// 转换为日期格式
        /// </summary>
        /// <param name="dateString">字符串类型的日期，或日期表达式</param>
        private DateTime? ParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return null;
            try
            {
                DateTime res;
                if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out res))
                    return res;
            }
            catch
            {
                //忽略
            }

            DateTime date = DateTime.Now;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var matches = Regex.Matches(dateString, "(?<number>[+-]?[0-9]+)\\s*(?<unit>d|D|w|W|m|M|y|Y)?");
            foreach (Match match in matches)
            {
                string matchValue = match.Value;
                int number = 0;
                int.TryParse(match.Groups["number"].ToString(), out number);
                switch (match.Groups["unit"].Value)
                {
                    case "w":
                    case "W":
                        day += number * 7; break;
                    case "m":
                    case "M":
                        month += number;
                        day = Math.Min(day, GetDaysInMonth(year, month));
                        break;

                    case "y":
                    case "Y":
                        year += number;
                        day = Math.Min(day, GetDaysInMonth(year, month));
                        break;

                    case "d":
                    case "D":
                    default:
                        day += number; break;
                }
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 获取某个月的天数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>指定年份、月份里的天数</returns>
        private int GetDaysInMonth(int year, int month)
        {
            return 32 - new DateTime(year, month, 32).Day;
        }

        /// <summary>
        /// 确保ErrorMessage已赋值.
        /// 如果ErrorMessage没有赋值,则根据当前的设置,给ErrorMessage赋相应值
        /// </summary>
        private void EnsureErrorMessage()
        {
            if (string.IsNullOrEmpty(ErrorMessage)
                && string.IsNullOrEmpty(ErrorMessageResourceName)
                && ErrorMessageResourceType == null)
            {
                string message;
                if (_minDate == DateTime.MinValue)
                {
                    if (_maxDate == DateTime.MaxValue)
                    {
                        throw new ArgumentException("最大值和最小值至少设置一个");
                    }
                    message = DefaultErrorMessages.Max;
                }
                else
                {
                    if (_maxDate == DateTime.MaxValue)
                    {
                        message = DefaultErrorMessages.Min;
                    }
                    else
                    {
                        message = DefaultErrorMessages.Range;
                    }
                }
                ErrorMessage = message;
            }
        }
    }
}