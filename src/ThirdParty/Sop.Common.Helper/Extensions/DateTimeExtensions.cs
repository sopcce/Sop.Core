using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sop.Common.Helper.Extensions
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// 得到一年中的某周的起始日和截止日
        /// </summary>
        /// <param name="nYear">年</param>
        /// <param name="nNumWeek">周数</param>
        /// <param name="dtWeekStart">周始</param>
        /// <param name="dtWeekeEnd">周终</param>
        public static void GetWeek(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            var dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Sunday);
            if (dtWeekStart < new DateTime(nYear, 1, 1))
                dtWeekStart = new DateTime(nYear, 1, 1);

            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
            if (dtWeekeEnd > new DateTime(nYear, 1, 1).AddYears(1))
                dtWeekeEnd = new DateTime(nYear, 1, 1).AddYears(1);
        }


        /// <summary>
        /// 求某年有多少周 
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns>返回 int</returns>
        public static int GetYearWeekCount(int strYear)
        {
            var fDt = DateTime.Parse(strYear + "-01-01");
            var k = Convert.ToInt32(fDt.DayOfWeek); //得到该年的第一天是周几 
            if (k == 1)
            {
                var countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                var countWeek = countDay / 7 + 1;
                return countWeek;
            }
            else
            {
                var countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                var countWeek = countDay / 7 + 2;
                return countWeek;
            }
        }

        /**/

        /// <summary>
        /// 求当前日期是一年的中第几周
        /// </summary>
        /// <param name="curDay"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime curDay)
        {
            var firstdayofweek = Convert.ToInt32(Convert.ToDateTime(curDay.Year + "-01-01 ").DayOfWeek);

            var days = curDay.DayOfYear;
            var daysOutOneWeek = days - (7 - firstdayofweek);

            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            var weeks = daysOutOneWeek / 7;
            if (daysOutOneWeek % 7 != 0)
                weeks++;

            return weeks + 1;
        }
    }
}
