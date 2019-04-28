using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Sop.Framework.WebUtility
{
    /// <summary>
    /// Cookie操作
    /// </summary>
    public class CookieUtility
    {
        #region Instance

        private static volatile CookieUtility _instance = null;
        private static readonly object Lock = new object();
        /// <summary>
        /// CookieUtility
        /// </summary>
        /// <returns></returns>
        public static CookieUtility Instance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CookieUtility();
                    }
                }
            }
            return _instance;
        }

        #endregion Instance

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public string GetCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }

        /// <summary>
        /// 获取集合类型Cookie，一个Cookie里可以存储多项值。
        /// </summary>
        /// <param name="sKey">Key键名称</param>
        /// <returns>null：Cookie不存在，其它：Cookie值的集合</returns>
        public NameValueCollection GetCookies(string sKey)
        {
            NameValueCollection nameValueCollection = null;
            if (string.IsNullOrEmpty(sKey))
                return null;
            var request = HttpContext.Current.Request;
            var httpCookie = request.Cookies[sKey];
            if (httpCookie != null)
            {
                nameValueCollection = new NameValueCollection();
                string[] allKeys = httpCookie.Values.AllKeys;
                for (int i = 0; i < allKeys.Length; i++)
                {
                    string name = allKeys[i];
                    nameValueCollection.Add(name, UrlUtility.Instance().UrlDecode(httpCookie.Values[name]));
                }
            }
            return nameValueCollection;
        }
        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        public void SetCookie(string cookiename, string cookievalue)
        {
            SetCookie(cookiename, cookievalue, DateTime.Now.AddDays(1.0));
        }
        /// <summary>
        /// 保存普通Cookie的值。
        /// </summary>
        /// <param name="sKey">Key键名称</param>
        /// <param name="sValue">要保存的值</param>
        /// <param name="expiresTime">到期时间，小于或等于当前时间的为临时Cookie。</param>
        public void SetCookie(string sKey, string sValue, DateTime expiresTime)
        {
            SetCookie(HttpContext.Current.Response, sKey, sValue, expiresTime);
        }
        /// <summary>
        /// 保存普通Cookie的值。
        /// </summary>
        /// <param name="response">HttpResponse对象</param>
        /// <param name="sKey">Key键名称</param>
        /// <param name="sValue">要保存的值</param>
        /// <param name="expiresTime">到期时间，小于或等于当前时间的为临时Cookie。</param>
        public void SetCookie(HttpResponse response, string sKey, string sValue, DateTime expiresTime)
        {
            if (string.IsNullOrEmpty(sKey))
            {
                return;
            }
            HttpCookie httpCookie = new HttpCookie(sKey);
            httpCookie.Value = UrlUtility.Instance().UrlEncode(sValue);
            httpCookie.HttpOnly = true;
            if (expiresTime > DateTime.Now)
            {
                httpCookie.Expires = expiresTime;
            }
            response.Cookies.Add(httpCookie);
        }
        /// <summary>
        /// 保存集合类型Cookie，一个Cookie里可以存储多项值。
        /// </summary>
        /// <param name="sKey">Key键名称</param>
        /// <param name="sValues">要存储的键/值对集合</param>
        /// <param name="expiresTime">到期时间，小于或等于当前时间的为临时Cookie。</param>
        public void SetCookies(string sKey, NameValueCollection sValues, DateTime expiresTime)
        {
            SetCookies(HttpContext.Current.Response, sKey, sValues, expiresTime);
        }
        /// <summary>
        /// 保存集合类型Cookie，一个Cookie里可以存储多项值。
        /// </summary>
        /// <param name="response">HttpResponse对象</param>
        /// <param name="sKey">Key键名称</param>
        /// <param name="sValues">要存储的键/值对集合</param>
        /// <param name="expiresTime">到期时间，小于或等于当前时间的为临时Cookie。</param>
        public void SetCookies(HttpResponse response, string sKey, NameValueCollection sValues, DateTime expiresTime)
        {
            if (string.IsNullOrEmpty(sKey) || sValues == null)
            {
                return;
            }
            if (sValues.Count <= 0)
            {
                return;
            }
            HttpCookie httpCookie = new HttpCookie(sKey);
            string[] allKeys = sValues.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string name = allKeys[i];
                httpCookie.Values[name] = UrlUtility.Instance().UrlEncode(sValues[name]);
            }
            httpCookie.HttpOnly = true;
            if (expiresTime > DateTime.Now)
            {
                httpCookie.Expires = expiresTime;
            }
            response.Cookies.Add(httpCookie);
        }

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public void Clear(string cookiename)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        /// <summary>
        /// 清除Cookie
        /// </summary>
        public void Clear()
        {
            HttpContext.Current.Request.Cookies.Clear();
        }


    }
}