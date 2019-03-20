
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace Sop.Framework.Utility
{
  public static class EnumDefine
  {
    #region 根据枚举类型返回类型中的所有值，文本及描述
    /// <summary>
    /// 根据枚举类型返回类型中的所有值，文本及描述
    /// </summary>
    /// <param name="type"></param>
    /// <returns>返回三列数组，第0列为Description,第1列为Value，第2列为Text</returns>
    public static List<string[]> GetEnumOpt(Type type)
    {
      List<string[]> list = new List<string[]>();
      FieldInfo[] fields = type.GetFields();
      for (int i = 0, count = fields.Length; i < count; i++)
      {
        string[] strEnum = new string[3];
        FieldInfo field = fields[i];

        if (field.Name == "value__")
        {
          continue;
        }
        //值列
        strEnum[1] = ((int)Enum.Parse(type, field.Name)).ToString();
        //文本列赋值
        strEnum[2] = field.Name;

        object[] objs = field.GetCustomAttributes(true);
        if (objs.Length == 0)
        {
          strEnum[0] = field.Name;
        }
        else
        {
          DescriptionAttribute da = (DescriptionAttribute)objs[0];
          strEnum[0] = da.Description;
        }

        list.Add(strEnum);
      }
      return list;
    }
    #endregion

    #region 获取枚举类子项描述信息
    /// <summary>
    /// 获取枚举类子项描述信息
    /// </summary>
    /// <example>
    /// 用法1：GetEnumDescription((Barfoo.FrameWork.Model.TableType)item.WFType)
    /// 用法2：GetEnumDescription(Barfoo.FrameWork.Model.TableType.DebriefInfo)
    /// </example>
    /// <param name="enumSubitem">枚举类子项</param>        
    public static string GetEnumDescription(this object enumSubitem)
    {
      enumSubitem = (Enum)enumSubitem;
      string strValue = enumSubitem.ToString();

      FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);

      if (fieldinfo != null)
      {

        Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (objs.Length == 0)
        {
          return strValue;
        }
        DescriptionAttribute da = (DescriptionAttribute)objs[0];
        return da.Description;
      }
      return null;
    }
    #endregion

    #region 字符串转换成枚举
    /// <summary>
    /// 字符串转换成枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumtext"></param>
    /// <returns></returns>
    public static T GetEnum<T>(string enumtext)
    {
      return (T)Enum.Parse(typeof(T), enumtext);
    }
    /// <summary>
    ///  根据值获取名称
    /// </summary>
    /// <param name="enumType">反射类型，如 typeof(EnumType.TitleList);</param>
    /// <param name="value">枚举值，如1，2，3</param>
    /// <returns>名称（字符串），如QuestionAnswer，TypicalApplications</returns>
    public static string GetNameByValue(Type enumType, string value)
    {
      return Enum.Parse(enumType, value).ToString();
    }
    #endregion

    public static string GetDescriptionByValue(Type enumType, string value)
    {
      object o = Enum.Parse(enumType, value);
      Enum b = (Enum)o;
      string strValue = b.ToString();
      FieldInfo fieldinfo = b.GetType().GetField(b.ToString());

      if (fieldinfo != null)
      {

        Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (objs.Length == 0)
        {
          return strValue;
        }
        DescriptionAttribute da = (DescriptionAttribute)objs[0];
        return da.Description;
      }
      return "不限";
    }
  }
}
