using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace Sop.Common.Helper.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkUtility
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);

        /// <summary>
        /// 用于检查网络是否可以连接互联网,true表示连接成功,false表示连接失败 
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectInternet()
        {
            bool isok = false;
            int i = 0;
            do
            {
                try
                {
                    int Description = 0;
                    bool connectInternet = InternetGetConnectedState(Description, 0);
                    if (connectInternet)
                    {
                        connectInternet = PingIpOrDomainName("www.baidu.com");
                    }
                    return connectInternet;
                }
                catch (Exception e)
                {
                    i++;
                    isok = true;
                    if (i < 5)
                    {
                        return false;
                    }
                }

            } while (isok);

            return false;

        }
        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令)
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns>true表示Ping成功,false表示Ping失败</returns>
        public static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                strIpOrDName = strIpOrDName.Replace("http://", "").Replace("http://", "");
                if (strIpOrDName.Contains(":"))
                {
                    strIpOrDName = strIpOrDName.Substring(0, strIpOrDName.LastIndexOf(":", StringComparison.Ordinal));
                }
                Ping ping = new Ping();
                PingOptions pingOptions = new PingOptions();
                pingOptions.DontFragment = true;
                byte[] buffer = Encoding.UTF8.GetBytes("hi");
                int intTimeout = 120;
                PingReply pingReply = ping.Send(strIpOrDName, intTimeout, buffer, pingOptions);
                if (pingReply != null)
                {
                    if (pingReply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
