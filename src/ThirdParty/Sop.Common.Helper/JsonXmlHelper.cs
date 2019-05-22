//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2018-6-6</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2019-04-6" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sop.Common.Helper.Json;

namespace Sop.Common.Helper
{
    public static class JsonXmlHelper 
    {
        #region ToJson
        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="dateTimeType">Type of the date time.</param>
        /// <param name="dateFormatString">The date format string.</param>
        /// <returns></returns>
        public static string ToJson(this object obj, DateTimeType dateTimeType, string dateFormatString = "yyyy-MM-dd HH:mm:ss fff")
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
                    settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    break;
                case DateTimeType.Default:
                    if (string.IsNullOrEmpty(dateFormatString))
                    {
                        dateFormatString = "yyyy-MM-dd HH:mm:ss fff";
                    }
                    settings.DateFormatString = dateFormatString;
                    break;
                default:

                    break;

            }
            var str = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, settings);
            return str;

        }


        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ToJson(this object obj, PropertyNameType type = PropertyNameType.ToLower)
        {
            var settings = new JsonSerializerSettings()
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-dd hh:mm:ss fff",
                DateTimeZoneHandling = DateTimeZoneHandling.Local

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

            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, settings);

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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json,
                new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss fff" });

        }
        #endregion

        #region ToXml

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

        /// <summary>
        /// xml转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string xml)
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
            catch (XmlException ex)
            {
                return false;
            }
        }
        #endregion
    }
}
