using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sop.Common.Serialization.Json
{

  /// <summary>  
  /// Newtonsoft.Json序列化扩展特性  
  /// <para>DateTime序列化（输出为时间戳）</para>  
  /// </summary>  
  /// <summary>  
  /// Newtonsoft.Json序列化扩展特性  
  /// <para>DateTime序列化（输出为时间戳）</para>  
  /// </summary>  
  public class JsonTimeStampConverter : JsonConverter
  {
    static readonly DateTime DefaluTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(DateTime);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      long value = long.MinValue;
      long.TryParse(reader.Value.ToString(), out value);
      return ConvertIntDateTime(value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(ConvertDateTimeInt((DateTime)value));
    }

    private static DateTime ConvertIntDateTime(long aSeconds)
    {
      return DefaluTime.AddSeconds(aSeconds);
    }

    private static long ConvertDateTimeInt(DateTime date)
    {
      return (long)(date - DefaluTime).TotalSeconds;
    }
  }





}
