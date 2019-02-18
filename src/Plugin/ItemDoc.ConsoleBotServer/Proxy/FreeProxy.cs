using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Metadata;
using System.Threading.Tasks;
using ItemDoc.ConsoleBot.Helper;
using ItemDoc.ConsoleBot.Models;
using Sop.Common.Serialization;

namespace ItemDoc.ConsoleBot.Proxy
{
    public class FreeProxy
    {

        //测试代理可用性
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="proxyAddr"></param>
        /// <param name="proxyUser"></param>
        /// <param name="proxyPassWord"></param>
        /// <param name="proxyDomain"></param>
        /// <returns></returns>
        public static bool CheckProxy(string url, string proxyAddr, string proxyUser, string proxyPassWord, string proxyDomain)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                WebProxy currentWebProxy = new WebProxy(proxyAddr, true);
                if (!string.IsNullOrWhiteSpace(proxyPassWord) && !string.IsNullOrWhiteSpace(proxyUser))
                    currentWebProxy.Credentials = new System.Net.NetworkCredential(proxyUser, proxyPassWord, proxyDomain);
                else
                    currentWebProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

                //处理HttpWebRequest访问https有安全证书的问题（ 请求被中止: 未能创建 SSL/TLS 安全通道。）
                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                // 创建并配置Web请求  
                request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    request.Proxy = currentWebProxy;
                    request.Timeout = 5000;
                    request.GetResponse();
                    response = request.GetResponse() as HttpWebResponse;
                }
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {

                    var txt = response.StatusCode + ":" + response.StatusDescription;
                    return true;
                }
                else
                {
                    var txt = response?.StatusCode + ":" + response?.StatusDescription;
                    return false;
                }
            }
            catch (Exception ee)
            {
                //  LogFile("代理服务器状态检测: 代理地址:" + ProxyAddr + "  用户名:" +
                // ProxyUser + "  密码:" + ProxyPassWord + "  域:" + ProxyDomain + "  异常信息:" + ee.Message);

                return false;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
                response?.Close();
            }
        }

        public List<Roots> GetProyList()
        {

            var list = XmlHelper<Roots>.GetAll();

            return list;
        }


    }
}
