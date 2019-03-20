using System.Web;

namespace Sop.Core.Helper
{
    /// <summary>
    /// Session 操作类 
    /// </summary>
    public class SessionHelper
    {

        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static object Get(string key)
        {
            if (HttpContext.Current.Session[key] == null)
                return null;
            else
                return HttpContext.Current.Session[key];
        }
        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static string[] Gets(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])HttpContext.Current.Session[strSessionName];
            }
        }

     




        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="key">session 名</param>
        /// <param name="value">session 值</param>
        public static void Set(string key, object value)
        {
            HttpContext.Current.Session.Remove(key);
            HttpContext.Current.Session.Add(key, value);
        }
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <param name="value">Session值</param>
        /// <param name="timeOut">调动有效期（分钟）</param>
        public static void Set(string key, object value, int timeOut)
        {
            HttpContext.Current.Session[key] = value;
            HttpContext.Current.Session.Timeout = timeOut;
        }
     
       
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSession(string name, object val)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, val);
        }
        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 20;
        }

        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        public static void Adds(string strSessionName, string[] strValues)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = 20;
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Add(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Adds(string strSessionName, string[] strValues, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = iExpires;
        }

      
      

        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }



    }
}
