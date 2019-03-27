using System;

namespace Sop.ConsoleBotServer.Proxy
{
    public class IpProxy
    { 
        ///<Summary>
        /// ID
        ///</Summary>
        public virtual long ID { get; set; }
        ///<Summary>
        /// 国家
        ///</Summary>
        public virtual string Country { get; set; }
        ///<Summary>
        /// IP
        ///</Summary>
        public virtual string Ip { get; set; }
        ///<Summary>
        /// 端口
        ///</Summary>
        public virtual int Port { get; set; }
        ///<Summary>
        /// 端口ip
        ///</Summary>
        public virtual string ProxyIp { get; set; }
        ///<Summary>
        /// 地址坐标
        ///</Summary>
        public virtual string Position { get; set; }
        ///<Summary>
        /// 匿名
        ///</Summary>
        public virtual string Anonymity { get; set; }
        ///<Summary>
        /// 类型
        ///</Summary>
        public virtual string Type { get; set; }
        ///<Summary>
        /// 响应速度
        ///</Summary>
        public virtual int Speed { get; set; }
        ///<Summary>
        /// 最后验证时间
        ///</Summary>
        public virtual byte[] ConnectTime { get; set; }
        ///<Summary>
        /// 爬取创建日期
        ///</Summary>
        public virtual DateTime DateCreate { get; set; }
    }
}