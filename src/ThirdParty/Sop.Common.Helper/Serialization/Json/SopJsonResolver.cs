//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.1</verion>
//<createdate>2018-1-23</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-2-23" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sop.Common.Helper.Json
{
    public class SopJsonResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
  {
    readonly PropertyNameType _type;
    public SopJsonResolver(PropertyNameType type = PropertyNameType.Default)
    {
      this._type = type;
    }

    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
      return type.GetProperties()
        .Select(p =>
        {
          var jp = base.CreateProperty(p, memberSerialization);
          jp.ValueProvider = new SopValueProvider(p);
          switch (_type)
          {
            case PropertyNameType.ToUpper:
              jp.PropertyName = jp.PropertyName.ToUpper();
              break;
            case PropertyNameType.ToLower:
              jp.PropertyName = jp.PropertyName.ToLower();
              break;
            case PropertyNameType.Default:
            default:
              jp.PropertyName = jp.PropertyName;
              break;
          }

          return jp;
        }).ToList();
    }


  }

  
}
