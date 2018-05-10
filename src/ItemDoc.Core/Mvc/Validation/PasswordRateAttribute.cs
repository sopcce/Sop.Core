using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ItemDoc.Core.Mvc.Validation
{
    /// <summary>
    /// 验证密码强度
    /// </summary>
    public class PasswordRateAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 密码强度
        /// </summary>
        private readonly PasswordRateEnum _passwordRate;

        private readonly string _lower = @"/[a-z]/";

        private readonly string _upper = @"/[A-Z]/";

        private readonly string _digit = @"/[0-9]/";

        private readonly string _digits = @"/[0-9].*[0-9]/";

        private readonly string _special = @"/[^a-zA-Z0-9]/";

        //private string _same = @"/^(.)\1+$/";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="passwordRate">密码强度:枚举</param>
        public PasswordRateAttribute(PasswordRateEnum passwordRate)
            : base()
        {
            switch (passwordRate)
            {
                case PasswordRateEnum.Weak:
                    _passwordRate = passwordRate;
                    break;

                case PasswordRateEnum.Good:
                    _passwordRate = passwordRate;
                    break;

                case PasswordRateEnum.Strong:
                    _passwordRate = passwordRate;
                    break;

                default:
                    _passwordRate = PasswordRateEnum.Weak;
                    break;
            }
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Uncapitalize(string password)
        {
            return password.Substring(0, 1).ToLower() + password.Substring(1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            string password = Convert.ToString(value);
            bool lower = Regex.IsMatch(password, _lower),
            upper = Regex.IsMatch(Uncapitalize(password), _upper),
            digit = Regex.IsMatch(password, _digit),
            digits = Regex.IsMatch(password, _digits),
            special = Regex.IsMatch(password, _special);
            if (_passwordRate == PasswordRateEnum.Weak)
            {
                if (!string.IsNullOrEmpty(password))
                {
                    return true;
                }
            }
            else if (_passwordRate == PasswordRateEnum.Good)
            {
                return lower && upper || lower && digit || upper && digit;
            }
            else
            {
                return lower && upper && digit || lower && digits || upper && digits || special;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format("{0}强度不够，请输入更强的密码", name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]
                       {
                           new ModelClientValidationRule{
                             ErrorMessage = this.ErrorMessage,
                             ValidationType = "passwordrate"
                            }
                       };
        }
    }

    /// <summary>
    /// 密码强度
    /// </summary>
    public enum PasswordRateEnum
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "弱")]
        Weak = 0,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "中")]
        Good = 1,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "强")]
        Strong = 2
    }
}