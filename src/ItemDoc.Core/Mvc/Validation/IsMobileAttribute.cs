using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ItemDoc.Core.Mvc.Validation
{
  /// <summary>
  /// 是否是手机号码
  /// </summary>
  public class IsMobileAttribute : RegularExpressionAttribute, IClientValidatable
  {
    private static string regularExpression = @"^0{0,1}(13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9])[0-9]{8}$";

    public IsMobileAttribute()
        : base(regularExpression)
    {
    }

    public override string FormatErrorMessage(string name)
    {
      return $"{name}格式不正确，请输入正确的手机号";
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
      return new[]
                 {
                           new ModelClientValidationRule{
                             ErrorMessage = this.ErrorMessage,
                             ValidationType = "ismobile"
                            }
                       };
    }
  }
}