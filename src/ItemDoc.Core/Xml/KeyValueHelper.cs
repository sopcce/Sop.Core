using System.Collections.Generic;
using System.Web;
using System.Xml;
using ItemDoc.Framework.Utility;

namespace ItemDoc.Core.Xml
{
  public class KeyValueHelper
  {
    /// <summary>
    /// 
    /// </summary>
    private static readonly string path = FileUtility.GetDiskFilePath("~/App_Data/keyvalue.xml");
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static KeyValueInfo GetInfo(string key = null, string value = null)
    {
      KeyValueInfo info = null;
      XmlDocument xmlNode = GetXmlNode();
      string xpath = "";
      if (!string.IsNullOrEmpty(key))
      {
        xpath = string.Format("//keyvalue[@key='{0}']", key);
      }
      else
      {
        xpath = string.Format("//keyvalue[@value='{0}']", value);
      }
      XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
      if (xmlNode2 != null)
      {
        info = new KeyValueInfo();
        info.Key = key;
        info.Value = xmlNode2.Attributes["value"].Value;

      } 
      return info;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IList<KeyValueInfo> GetAll()
    {
      IList<KeyValueInfo> list = new List<KeyValueInfo>();
      XmlDocument xmlNode = GetXmlNode();
      XmlNode xmlNode2 = xmlNode.SelectSingleNode("keyvalues");
      int num = 1;
      foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
      {
        KeyValueInfo info = new KeyValueInfo();
        info.Id = num;
        info.Key = xmlNode3.Attributes["key"] != null ? xmlNode3.Attributes["key"].Value : "";
        info.Value = xmlNode3.Attributes["value"] != null ? xmlNode3.Attributes["value"].Value : "";

        bool isnew = false;
        bool.TryParse((xmlNode3.Attributes["new"] != null ? xmlNode3.Attributes["value"].Value : "false"), out isnew);
        info.IsNew = isnew;
        list.Add(info);
        num++;

      }
      return list;
    }

    public static void DeleteInfo(string key)
    {
      XmlDocument xmlNode = GetXmlNode();
      XmlNode xmlNode2 = xmlNode.SelectSingleNode("keyvalues");
      foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
      {
        if (xmlNode3.Attributes["key"].Value == key)
        {
          xmlNode2.RemoveChild(xmlNode3);
          break;
        }
      }
      xmlNode.Save(path);
    }

    public static void AddInfo(string key, string value)
    {
      XmlDocument xmlNode = GetXmlNode();
      XmlNode xmlNode2 = xmlNode.SelectSingleNode("keyvalues");
      XmlElement xmlElement = xmlNode.CreateElement("keyvalue");
      xmlElement.SetAttribute("key", key);
      xmlElement.SetAttribute("value", value);
      xmlElement.SetAttribute("New", "Y");
      xmlNode2.AppendChild(xmlElement);
      xmlNode.Save(path);
    }

    public static bool IsExitKey(string name)
    {
      XmlDocument xmlNode = GetXmlNode();
      XmlNode xmlNode2 = xmlNode.SelectSingleNode("keyvalues");
      bool result;
      foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
      {
        if (xmlNode3.Attributes["key"].Value == name)
        {
          result = true;
          return result;
        }
      }
      result = false;
      return result;
    }

    public static void UpdateInfo(string oldkey, string newkey, string value)
    {
      XmlDocument xmlNode = GetXmlNode();
      XmlNode xmlNode2 = xmlNode.SelectSingleNode("keyvalues");
      foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
      {
        if (xmlNode3.Attributes["key"].Value == oldkey)
        {
          xmlNode3.Attributes["key"].Value = newkey;
          xmlNode3.Attributes["value"].Value = value;
          break;
        }
      }
      xmlNode.Save(path);
    }




    private static XmlDocument GetXmlNode()
    {
      XmlDocument xmlDocument = new XmlDocument();
      if (!string.IsNullOrEmpty(KeyValueHelper.path))
      {
        xmlDocument.Load(KeyValueHelper.path);
      }
      return xmlDocument;
    }
  }



  public class KeyValueInfo
  {
    public int Id { get; set; }
    public string Key { get; set; }

    public string Value { get; set; }

    public bool IsNew { get; set; }




  }
}