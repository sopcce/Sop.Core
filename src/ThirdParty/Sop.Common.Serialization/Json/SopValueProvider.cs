//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.1</verion>
//<createdate>2018-1-23</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-2-23" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sop.Common.Serialization.Json
{
  public class SopValueProvider : IValueProvider
  {
    readonly PropertyInfo _memberInfo;
    public SopValueProvider(PropertyInfo memberInfo)
    {
      _memberInfo = memberInfo;
    }

    public object GetValue(object target)
    {
      object result = _memberInfo.GetValue(target);
      //if (_memberInfo.PropertyType == typeof(string) && result == null)
      //  result = "";
      //if (_memberInfo.PropertyType == typeof(DateTime))
      //{
      //  //DateTime date = DateTime.MinValue;

      //  //if (DateTime.TryParse(result.ToString(), out date))
      //  //{
      //  //  result = (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
      //  //}

      //}

      return result;

    }

    public void SetValue(object target, object value)
    {
      _memberInfo.SetValue(target, value);
    }
  }
}
