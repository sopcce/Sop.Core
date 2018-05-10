//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.1</verion>
//<createdate>2018-1-23</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-2-23" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sop.Common.Serialization.Json;

namespace Sop.Common.Serialization
{
  public static class JsonUtility
  {
    /// <summary>
    /// To the json.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static string ToJson(this object obj)
    {
      var settings = new JsonSerializerSettings()
      {

        DateFormatString = "yyyy-MM-dd HH:mm:ss",
        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
      };
      return JsonConvert.SerializeObject(obj, Formatting.None, settings);



    }
    /// <summary>
    /// To the json.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="dateTimeType">Type of the date time.</param>
    /// <param name="dateFormatString">The date format string.</param>
    /// <returns></returns>
    public static string ToJson(this object obj, DateTimeType dateTimeType = DateTimeType.Default, string dateFormatString = "yyyy-MM-dd HH:mm:ss")
    {

      var settings = new JsonSerializerSettings()
      {
        ContractResolver = new SopJsonResolver(),
        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
        NullValueHandling = NullValueHandling.Ignore
      };
      switch (dateTimeType)
      {
        case DateTimeType.MicrosoftDateFormatUtc:
          settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
          settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
          break;

        case DateTimeType.Default:
          if (string.IsNullOrEmpty(dateFormatString))
          {
            dateFormatString = "yyyy-MM-dd HH:mm:ss";
          }

          settings.DateFormatString = dateFormatString;
          break;
        default:
          break;

      }
      var str = JsonConvert.SerializeObject(obj, Formatting.None, settings);
      return str;

    }


    /// <summary>
    /// To the json.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static string ToJson(this object obj, PropertyNameType type = PropertyNameType.Default)
    {
      var ssd = new IsoDateTimeConverter()
      {
        DateTimeFormat = "yyyy-MM-dd hh:mm:ss"
      };

      var settings = new JsonSerializerSettings()
      {
        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
        NullValueHandling = NullValueHandling.Ignore
      };
      switch (type)
      {
        case PropertyNameType.CamelCase:
          settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          break;
        case PropertyNameType.ToLower:
        case PropertyNameType.ToUpper:
        case PropertyNameType.Default:
        default:
          settings.ContractResolver = new SopJsonResolver(type);
          break;
      }
       
      return JsonConvert.SerializeObject(obj, Formatting.None, settings);

    }
    /// <summary>
    /// To the json.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="formatting">The formatting.</param>
    /// <returns></returns>
    public static string ToJson(this object obj, Formatting formatting = Formatting.None)
    {

      var settings = new JsonSerializerSettings()
      {
        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
        NullValueHandling = NullValueHandling.Ignore
      };
      return JsonConvert.SerializeObject(obj, Formatting.None, settings);

    }


    /// <summary>
    /// To the json.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="settings">The settings.</param>
    /// <returns></returns>
    public static string ToJson(this object obj, object settings)
    {
      var settings2 = settings as JsonSerializerSettings;
      return JsonConvert.SerializeObject(obj, settings2);
    }





    public static T FromJson<T>(this string json)
    {
      return JsonConvert.DeserializeObject<T>(json,
        new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });

    }





  }


}
