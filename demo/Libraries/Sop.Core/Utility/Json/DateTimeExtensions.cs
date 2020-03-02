using System;

namespace Sop.Data.Utility
{
  public static class DateTimeExtensions
  {
    /// <summary>
    /// Dates the time to unix timestamp.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static long DateTimeToUnixTimestamp(DateTime dateTime)
    {
      var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
      return Convert.ToInt64((dateTime - start).TotalSeconds);
    }

    /// <summary>
    /// Unixes the timestamp to date time.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="timestamp">The timestamp.</param>
    /// <returns></returns>
    public static DateTime UnixTimestampToDateTime(this DateTime target, long timestamp)
    {
      var start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
      return start.AddSeconds(timestamp);
    }

   
  }
}
