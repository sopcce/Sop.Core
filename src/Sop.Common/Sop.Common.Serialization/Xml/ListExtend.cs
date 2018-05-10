//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.1</verion>
//<createdate>2018-1-23</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-2-23" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Sop.Common.Serialization.XML
{
  public static class ListExtend
  {
    /// <summary>
    /// 加载XML文档返回List集合
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<TSource> Load<TSource>(this List<TSource> source)
    {
      string fileName = PathRoute.GetXmlPath<TSource>();
      if (File.Exists(fileName))
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TSource>));
        using (Stream reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
          return xmlSerializer.Deserialize(reader) as List<TSource>;
        }
      }
      else
      {
        return new List<TSource>();
      }
    }
    /// <summary>
    /// 将list集合保存为XML文档
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    public static void Save<TSource>(this List<TSource> source)
    {
      string fileName = PathRoute.GetXmlPath<TSource>();
      FileInfo fileInfo = new FileInfo(fileName);
      DirectoryInfo directoryInfo = fileInfo.Directory;
      if (!directoryInfo.Exists)
      {
        directoryInfo.Create();
      }
      XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
      using (Stream writer = new FileStream(fileName, FileMode.Create, FileAccess.Write))
      {
        xmlSerializer.Serialize(writer, source);
      }
    }
  }
}
