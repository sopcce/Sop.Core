using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Caches.Redis
{
  /// <summary>
  /// 
  /// </summary>
  public class NetDataContractCacheSerializer : XmlRedisCacheSerializerBase
  {
    protected override XmlObjectSerializer CreateSerializer()
    {
      var serializer = new NetDataContractSerializer();
      return serializer;
    }

  }
}
