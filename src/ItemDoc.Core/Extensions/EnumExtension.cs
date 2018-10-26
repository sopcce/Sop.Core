using System;
using System.ComponentModel;
using System.Linq;

namespace System
{
    // <summary>
    /// 枚举类扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举项上设置的显示Display文字
        /// </summary>
        /// <param name="value">被扩展对象</param>
        public static string GetEnumDisplay(this Enum value)
        {
            var attribute = value.GetType().GetField(Enum.GetName(value.GetType(), value)).GetCustomAttributes(
                 typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                 .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                 .FirstOrDefault();
            if (attribute != null)
            {
                return attribute.Name;
            }

            return Enum.GetName(value.GetType(), value);
        }


        /// <summary>
        /// 获取枚举项上设置的显示Description文字
        /// </summary>
        /// <param name="enumSubitem">枚举</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetEnumDescription(System.Enum enumSubitem, int index)
        {
            string text = enumSubitem.ToString();
            System.Reflection.FieldInfo field = enumSubitem.GetType().GetField(text);
            object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string result;
            if (customAttributes == null || customAttributes.Length == 0)
            {
                result = text;
            }
            else
            {
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)customAttributes[0];
                result = descriptionAttribute.Description.Split(new char[]
                {
                    '|'
                })[index];
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumSubitem"></param>
        /// <returns></returns>
        public static string GetEnumDescription(System.Enum enumSubitem)
        {
            string text = enumSubitem.ToString();
            System.Reflection.FieldInfo field = enumSubitem.GetType().GetField(text);
            object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string result;
            if (customAttributes == null || customAttributes.Length == 0)
            {
                result = text;
            }
            else
            {
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)customAttributes[0];
                result = descriptionAttribute.Description;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumDescription"></param>
        /// <param name="currentfiled"></param>
        /// <returns></returns>
        public static bool GetEnumValue<TEnum>(string enumDescription, ref TEnum currentfiled)
        {
            bool result = false;
            System.Type typeFromHandle = typeof(TEnum);
            System.Reflection.FieldInfo[] fields = typeFromHandle.GetFields();
            for (int i = 1; i < fields.Length - 1; i++)
            {
                DescriptionAttribute descriptionAttribute = fields[i].GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute;
                if (descriptionAttribute.Description.Contains(enumDescription))
                {
                    currentfiled = (TEnum)((object)fields[i].GetValue(typeof(TEnum)));
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}