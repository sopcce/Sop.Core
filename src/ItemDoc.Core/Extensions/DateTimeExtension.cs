using System.Globalization;

namespace System
{
    /// <summary>
    /// DateTime扩展方法
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="dateTime">DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ToTimestamp(this DateTime dateTime)
        {
            TimeSpan span = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (long)span.TotalSeconds;
        }

        /// <summary>
        /// Unix时间戳格式转换为DateTime时间格式
        /// </summary>
        /// <param name="dateTime">DateTime时间格式</param>
        /// <param name="timestamp">Unix时间戳格式</param>
        /// <returns>DateTime时间格式</returns>
        public static DateTime FromTimestamp(this DateTime dateTime, long timestamp)
        {
            DateTime newDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
            return newDateTime.ToLocalTime();
        }

        /// <summary>
        /// 转换成用户所在时区的时间，并按用户设置返回对应的格式化字符串
        /// </summary>
        /// <param name="dateTime">原日期(UTC时间)</param>
        /// <param name="displayTime">是否显示时间</param>
        /// <returns>返回用户所在时区时间,并按用户设置返回对应的格式化字符串</returns>
        public static string ToUserDateString(this DateTime dateTime, bool displayTime = false)
        {
            if (dateTime == DateTime.MinValue)
            {
                return "-";
            }

            if (dateTime.Kind != DateTimeKind.Local)
            {
                dateTime = dateTime.ToLocalTime();
            }

            if (displayTime)
            {
                return dateTime.ToString("yyyy/MM/dd HH:mm");
            }
            else
            {
                return dateTime.ToString("yyyy/MM/dd");
            }
        }

        /// <summary>
        /// 获取指定日所在周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(this DateTime dateTime)
        {
            GregorianCalendar calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);

            return calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 获取指定日所在周的周一(00:00.000)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetClosestMonday(this DateTime dateTime)
        {
            GregorianCalendar calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);

            DayOfWeek dayOfWeek = calendar.GetDayOfWeek(dateTime);
            if (dayOfWeek.Equals(DayOfWeek.Sunday))
            {
                return dateTime.Date.AddDays(-6);
            }
            else
            {
                return dateTime.Date.AddDays(-((int)dayOfWeek - 1));
            }
        }

        /// <summary>
        /// 获取指定日所在周的周末(23:59.999)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetClosestSunday(this DateTime dateTime)
        {
            GregorianCalendar calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);

            DayOfWeek dayOfWeek = calendar.GetDayOfWeek(dateTime);
            if (dayOfWeek.Equals(DayOfWeek.Sunday))
            {
                return dateTime.Date.AddDays(1).AddMilliseconds(-1);
            }
            else
            {
                return dateTime.Date.AddDays(7 - (int)dayOfWeek + 1).AddMilliseconds(-1);
            }
        }

        /// <summary>
        /// 友好时间格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToFriendlyDate(this DateTime dateTime)
        {
            string friendlyDate = string.Empty;

            //校准时间
            dateTime = dateTime.ToLocalTime();
            DateTime now = DateTime.Now;

            TimeSpan span = dateTime - now;
            if (Math.Abs(span.Days) <= 30)
            {
                int week_span = dateTime.GetWeekOfYear() - now.GetWeekOfYear();
                int day_span = (dateTime.Date - now.Date).Days;

                //上下三十天内
                if (span.Days == 0)
                {
                    //当天
                    if (span.Minutes == 0)
                    {
                        friendlyDate = "现在";
                    }
                    else if (Math.Abs(span.Hours) >= 1)
                    {
                        //当天的前后一小时之外
                        if (span.Hours > 0)
                        {
                            friendlyDate = string.Format("{0}小时后", Math.Abs(span.Hours));
                        }
                        else
                        {
                            friendlyDate = string.Format("{0}小时前", Math.Abs(span.Hours));
                        }
                    }
                    else
                    {
                        //一小时之内
                        if (span.Minutes > 0)
                        {
                            friendlyDate = string.Format("{0}分钟后", Math.Abs(span.Minutes));
                        }
                        else
                        {
                            friendlyDate = string.Format("{0}分钟前", Math.Abs(span.Minutes));
                        }
                    }
                }
                else if (span.Days > 0)
                {
                    //后三十天内
                    if (dateTime.Year == now.Year)
                    {
                        //同一年
                        if (week_span == 1)
                        {
                            friendlyDate = string.Format("下周{0}", DateTimeExtension.GetWeekString(dateTime));
                        }
                        else
                        {
                            //n天后
                            friendlyDate = string.Format("{0}天后", Math.Abs(day_span));
                        }
                    }
                    else
                    {
                        //n天后
                        friendlyDate = string.Format("{0}天后", Math.Abs(day_span));
                    }
                }
                else
                {
                    //前三十天内
                    if (dateTime.Year == now.Year)
                    {
                        //同一年
                        if (week_span == -1)
                        {
                            friendlyDate = string.Format("上周{0}", DateTimeExtension.GetWeekString(dateTime));
                        }
                        else
                        {
                            friendlyDate = string.Format("{0}天前", Math.Abs(day_span));
                        }
                    }
                    else
                    {
                        friendlyDate = string.Format("{0}天前", Math.Abs(day_span));
                    }
                }
            }
            else
            {
                if (dateTime.Year != now.Year)
                {
                    friendlyDate = dateTime.ToString("yyyy年MM月dd日");
                }
                else
                {
                    friendlyDate = dateTime.ToString("MM月dd日");
                }
            }

            return friendlyDate;
        }

        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetDayofWeek(this DateTime dateTime)
        {
            string dayofweek = string.Empty;

            dayofweek = string.Format("星期{0}", DateTimeExtension.GetWeekString(dateTime));

            return dayofweek;
        }

        /// <summary>
        /// 获得星期描述
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static string GetWeekString(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "一";

                case DayOfWeek.Tuesday:
                    return "二";

                case DayOfWeek.Wednesday:
                    return "三";

                case DayOfWeek.Thursday:
                    return "四";

                case DayOfWeek.Friday:
                    return "五";

                case DayOfWeek.Saturday:
                    return "六";

                case DayOfWeek.Sunday:
                    return "日";

                default:
                    return string.Empty;
            }
        }
    }
}