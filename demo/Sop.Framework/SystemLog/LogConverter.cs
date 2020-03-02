using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net.Layout.Pattern;

namespace Sop.Framework.SystemLog
{
  public class LogConverter : PatternLayoutConverter
  {
    protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
    {
      if (Option != null)
      {
        WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
      }
      else
      {
        WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
      }
    }

    /// <summary>
    /// 通过反射获取传入的日志对象的某个属性的值
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
    {
      object propertyValue = string.Empty;

      PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);

      if (propertyInfo != null)
        propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);

      return propertyValue;
    }
  }
}
