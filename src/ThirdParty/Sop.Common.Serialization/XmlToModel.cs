//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2018-6-6</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-6-6" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Sop.Common.Serialization
{
  /// <summary>
  /// 
  /// </summary>
  public static class XmlToModel
  {

    /// <summary>
    /// 实体转xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string ToXml<T>(this T model)
    {
      if (model == null) return string.Empty;
      var xmlSerializer = new XmlSerializer(typeof(T));

      using (var stringWriter = new StringWriter())
      {
        using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
        {
          xmlSerializer.Serialize(xmlWriter, model);
          return stringWriter.ToString();
        }
      }
    }

    //public static string ToXml<IEnumerable<T>() >(this IEnumerable<T> list)
    //{
    //  foreach (var model in list)
    //  {
    //    model.ToXml();
    //  }

    //}

    /// <summary>
    /// xml转实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static T ToModel<T>(this string xml)
    {
      if (string.IsNullOrWhiteSpace(xml) || !xml.IsValidXml()) return default(T);

      var xmlSerializer = new XmlSerializer(typeof(T));

      using (var stringReader = new StringReader(xml))
      {
        using (var xmlTextReader = new XmlTextReader(stringReader))
        {
          var result = xmlSerializer.Deserialize(xmlTextReader);
          return result != null ? (T)Convert.ChangeType(result, typeof(T)) : default(T);
        }
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static bool IsValidXml(this string xml)
    {
      try
      {
        var result = new XmlDocument();
        result.LoadXml(xml);
        return true;
      }
      catch (XmlException xex)
      {

      }
      catch (Exception ex)
      {



      }

      return false;
    }
  }

  internal class T
  {
  }
}
