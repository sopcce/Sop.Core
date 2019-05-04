using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;

namespace Sop.Framework.Environment
{
    /// <summary>
    ///  网站config中配置
    /// </summary>
    public static class Config
    {
        static string _docName = "web.config";


        /// <summary>
        /// 允许上传及删除的文件类型(小写，可在*.config中AppSettings区域AllowUploadFile项自定义配置)。
        /// 如果没有配置此项值将返回string.Empty，即不允许上传及删除任何文件！
        /// </summary>
        public static readonly string AllowUploadOrDeleteFileTypes = Config.AppSettings<string>("AllowUploadFile", string.Empty);
        /// <summary>
        /// DES解密字符串
        /// </summary>
        public static readonly string EncryptKey = Config.AppSettings<string>("EncryptKey", "sop@cce#.￥com");

        /// <summary>
        /// 检测XXX.config的读写权限
        /// </summary>
        /// <param name="fileName">存放Config物理路径 如：Server.MapPath("~/Web.config")</param>
        /// <returns>如果指定的XXX.config.config的权限能读写，则为 true；否则为 false。</returns>
        public static bool CheckConfigReadWrite(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
                return false;
            var xmldocument = new System.Xml.XmlDocument();
            xmldocument.Load(fileInfo.FullName);
            try
            {
                var moduleNode = xmldocument.SelectSingleNode("//appSettings");
                if (moduleNode != null && moduleNode.HasChildNodes)
                {
                    for (int i = 0; i < moduleNode.ChildNodes.Count; i++)
                    {
                        var node = moduleNode.ChildNodes[i];

                        if (node.Name == "add")
                        {
                            if (node.Attributes != null)
                            {
                                var sop = node.Attributes.GetNamedItem("key").Value;
                                var sopValue = node.Attributes.GetNamedItem("value").Value;

                                if (sop != "SopInitialization") continue;
                                moduleNode.RemoveChild(node);
                                break;
                            }
                        }
                    }
                }
                xmldocument.Save(fileInfo.FullName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取配置文件中当前键值对应的值，并转换为相应的类型
        /// </summary>
        /// <typeparam name="T">想要转换的类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>配置项值</returns>
        public static T AppSettings<T>(string key)
        {
            return AppSettings<T>(key, default(T));
        }

        /// <summary>
        /// 获取配置文件中当前键值对应的值，并转换为相应的类型
        /// </summary>
        /// <typeparam name="T">想要转换的类型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置项值</returns>
        public static T AppSettings<T>(string key, T defaultValue)
        {
            var v = ConfigurationManager.AppSettings[key];
            return String.IsNullOrEmpty(v) ? defaultValue : (T)Convert.ChangeType(v, typeof(T));
        }



        /// <summary>
        /// 获取配置文件中当前键值对应的数据库连接
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>数据库连接字符串</returns>
        public static string ConnectionStrings(string key)
        {
            var setting = ConfigurationManager.ConnectionStrings[key];
            if (setting == null) return string.Empty;
            return setting.ConnectionString;
        }

        /// <summary>
        /// 获取配置文件中当前键值对应的数据库连接
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>数据库连接字符串</returns>
        public static string ConnectionStrings(string key, string defaultValue)
        {
            var setting = ConfigurationManager.ConnectionStrings[key];
            if (setting == null || string.IsNullOrEmpty(setting.ConnectionString)) return defaultValue;

            return setting.ConnectionString;
        }











        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetAppSettingsValue(string key, string value)
        {
            XmlDocument cfgDoc = new XmlDocument();

            object _lock = new object();
            lock (_lock)
            {
                LoadConfigDoc();
                // retrieve the appSettings node 
                var node = cfgDoc.SelectSingleNode("//appSettings");

                if (node == null)
                {
                    throw new InvalidOperationException("appSettings section not found");
                }
                try
                {
                    // XPath select setting "add" element that contains this key    
                    XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");
                    if (addElem != null)
                    {
                        addElem.SetAttribute("value", value);
                    }
                    // not found, so we need to add the element, key and value
                    else
                    {
                        XmlElement entry = cfgDoc.CreateElement("add");
                        entry.SetAttribute("key", key);
                        entry.SetAttribute("value", value);
                        node.AppendChild(entry);
                    }
                    //save it
                    SaveConfigDoc(_docName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfgDocPath"></param>
        public static void SaveConfigDoc(string cfgDocPath)
        {
            XmlDocument cfgDoc = new XmlDocument();
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(cfgDocPath, null))
                {
                    writer.Formatting = Formatting.Indented;
                    cfgDoc.WriteTo(writer);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                throw new FileLoadException("Unable to load the web.config file for modification", e.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementKey"></param>
        /// <returns></returns>
        public static bool RemoveElement(string elementKey)
        {
            XmlDocument cfgDoc = new XmlDocument();

            try
            {
                LoadConfigDoc();
                // retrieve the appSettings node 
                var node = cfgDoc.SelectSingleNode("//appSettings");
                if (node == null)
                {
                    throw new InvalidOperationException("appSettings section not found");
                }
                // XPath select setting "add" element that contains this key to remove   
                var oldNode = node.SelectSingleNode("//add[@key='" + elementKey + "']");
                if (oldNode != null)
                    node.RemoveChild(oldNode);

                SaveConfigDoc(_docName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static void LoadConfigDoc()
        {
            XmlDocument cfgDoc = new XmlDocument();
            // load the config file 
            _docName = HttpContext.Current.Server.MapPath(_docName);
            cfgDoc.Load(_docName);
            return;
        }









    }
}