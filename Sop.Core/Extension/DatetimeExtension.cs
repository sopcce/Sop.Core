using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatetimeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertTime(this DateTime time)
        {
            DateTime dateTime = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return (long)(time - dateTime).TotalMilliseconds;
        }
        /// <summary>
        /// 将时间戳转换为日期类型 
        /// </summary>
        /// <param name="longTime"></param>
        /// <returns></returns>
        public static DateTime ConvertTime(this long longTime)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(longTime).ToLocalTime();
        }
    }
}
