using System.Collections.Generic;
using System.Xml;

namespace Sop.FileServer.Helper
{
    public class ConsileBot : XmlEntity
    {

        public List<string> ItemProxyList { get; set; }


        //测试代理可用性

    }
    public class ItemProxy
    {
        public string RemoteUrl { get; set; }



    }
}