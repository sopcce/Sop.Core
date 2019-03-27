using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Sop.Common.Serialization.XML;

namespace Sop.ConsoleBotServer.Helper
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