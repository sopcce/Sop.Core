using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Sop.Common.Serialization
{
  public static class XmlToModel
  {
    #region Info
    //这个标签表示整个类都需要序列化
    //[Serializable()]
    //[XmlRoot("Orders")]
    //public class Orders
    //{
    //  public int Id { get; set; }

    //  public Order Order { get; set; }

    //  [XmlAttribute(AttributeName = "OrdersName")]
    //  public string Name { get; set; }

    //  [XmlElement(ElementName = "Ordersdescribe")]
    //  public string describe { get; set; }

    //}
    //[Serializable()]
    //public class Order
    //{
    //  public int Id { get; set; }
    //  [XmlAttribute(AttributeName = "Name")]
    //  public string Name { get; set; }

    //  [XmlElement(ElementName = "describe")]
    //  public string describe { get; set; }

    //  public UsersLoginInfo UsersLoginInfo { get; set; }
    //}
    //[Serializable()]
    //public class UsersLoginInfo
    //{
    //  public int Id { get; set; }
    //  public string name { get; set; }


    //  [XmlAttribute(AttributeName = "uphone")]
    //  public string phone { get; set; }

    //  [XmlElement(ElementName = "ucity")]
    //  public string city { get; set; }



    //  public string address { get; set; }

    //}
    #endregion
    //var orders = new Orders()
    //{
    //  Id = 110,
    //  Name = "Orders",
    //  describe = DateTime.Now.ToShortDateString(),
    //  Order = new Order()
    //  {
    //    Id = 110111,
    //    Name = "Orders",
    //    describe = DateTime.Now.ToShortDateString(),
    //    UsersLoginInfo = new UsersLoginInfo()
    //    {
    //      Id = 110112000,
    //      name = "guojiaqiu",
    //      phone = "11222222",
    //      city = DateTime.Now.ToShortDateString(),
    //    }
    //  }

    //}; 
    //var xml = orders.ToXml();



    /// <summary>
    /// 
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




    /// <summary>
    /// 
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





    public static bool IsValidXml(this string xml)
    {
      try
      {
        var result = new XmlDocument();
        result.LoadXml(xml);
        return true;
      }
      catch
      {
        // ignored
      }

      return false;
    }
  }
}
