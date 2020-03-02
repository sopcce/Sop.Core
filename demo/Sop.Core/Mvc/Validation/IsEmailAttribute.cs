using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sop.Core.Mvc.Validation
{
    /// <summary>
    /// 是否是Email地址
    /// </summary>
    public class IsEmailAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public static string regularExpression = @"/^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$/";

        public IsEmailAttribute()
            : base(regularExpression)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format("{0}格式不正确，请输入正确的邮箱地址", name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]
                       {
                           new ModelClientValidationRule{
                             ErrorMessage = this.ErrorMessage,
                             ValidationType = "isemail"
                            }
                       };
        }
    }
}