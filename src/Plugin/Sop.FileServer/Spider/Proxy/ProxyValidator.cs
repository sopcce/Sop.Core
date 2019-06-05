using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog;

namespace Sop.FileServer.Spider.Proxy
{
	/// <summary>
	/// 代理验证器
	/// 提供代理的验证
	/// </summary>
	public class ProxyValidator : IProxyValidator
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly string _targetUrl = "http://www.baidu.com";
		private readonly int _reuseInterval;

		/// <summary>
		/// 获取代理
		/// </summary>
		public IWebProxy WebProxy { get; set; }
		#region ProxyValidator
		public ProxyValidator()
		{
		}

		/// <summary>
		/// 代理验证器
		/// </summary>
		/// <param name="proxyHost">代理服务器域名或ip</param>
		/// <param name="proxyPort">代理服务器端口</param>
		// ReSharper disable once UnusedMember.Local
		public ProxyValidator(string proxyHost, int proxyPort)
			: this(new HttpProxy(proxyHost, proxyPort))
		{
		}

		/// <summary>
		/// 代理验证器
		/// </summary>
		/// <param name="webProxy">代理</param>
		/// <exception cref="ArgumentNullException"></exception>
		public ProxyValidator(IWebProxy webProxy)
		{
			WebProxy = webProxy ?? throw new ArgumentNullException(nameof(webProxy));
		}


		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="proxy"></param>
		/// <returns></returns>
		public bool IsAvailable(WebProxy proxy)
		{
			var timeout = TimeSpan.FromSeconds(1d);
			var validator = new ProxyValidator(proxy);
			var targetAddress = new Uri(_targetUrl);
			return validator.Validate(targetAddress, timeout) == HttpStatusCode.OK;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="proxy"></param>
		/// <param name="targetUrl"></param>
		/// <returns></returns>
		public bool IsAvailable(WebProxy proxy, string targetUrl)
		{
			var timeout = TimeSpan.FromSeconds(1d);
			var validator = new ProxyValidator(proxy);
			var targetAddress = new Uri(targetUrl);
			return validator.Validate(targetAddress, timeout) == HttpStatusCode.OK;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="proxy"></param>
		/// <param name="targetUrl"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public bool IsAvailable(WebProxy proxy, string targetUrl, TimeSpan timeout)
		{

			var validator = new ProxyValidator(proxy);
			var targetAddress = new Uri(targetUrl);
			return validator.Validate(targetAddress, timeout) == HttpStatusCode.OK;
		}






		/// <summary>
		/// 使用http tunnel检测代理状态
		/// </summary>
		/// <param name="targetAddress">目标地址，可以是http或https</param>
		/// <param name="timeout">发送或等待数据的超时时间</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <returns></returns>
		public HttpStatusCode Validate(Uri targetAddress, TimeSpan? timeout = null)
		{
			if (targetAddress == null)
			{
				throw new ArgumentNullException(nameof(targetAddress));
			}
			return Validate(WebProxy, targetAddress, timeout);
		}

		/// <summary>
		/// 转换为字符串
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return WebProxy.ToString();
		}

		/// <summary>
		/// 使用http tunnel检测代理状态
		/// </summary>
		/// <param name="webProxy">web代理</param>      
		/// <param name="targetAddress">目标地址，可以是http或https</param>
		/// <param name="timeout">发送或等待数据的超时时间</param>
		/// <exception cref="ArgumentNullException"></exception>    
		/// <returns></returns>
		public static HttpStatusCode Validate(IWebProxy webProxy, Uri targetAddress, TimeSpan? timeout = null)
		{
			if (webProxy == null)
			{
				throw new ArgumentNullException(nameof(webProxy));
			}

			var httpProxy = webProxy as HttpProxy;
			if (httpProxy == null)
			{
				httpProxy = HttpProxy.FromWebProxy(webProxy, targetAddress);
			}
			Socket socket = null;
			try
			{

			 
				Encoding encoding = Encoding.UTF8;// .GetEncoding("gb2312");
				var request = httpProxy.ToTunnelRequestString(targetAddress);
				var sendBuffer = encoding.GetBytes(request);

				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				if (timeout.HasValue)
				{
					socket.SendTimeout = (int)timeout.Value.TotalMilliseconds;
					socket.ReceiveTimeout = (int)timeout.Value.TotalMilliseconds;
				}
				//socket.Connect(new IPEndPoint(host.AddressList[0], httpProxy.Port));
				socket.Connect(httpProxy.Host, httpProxy.Port);
			
				socket.Send(sendBuffer);
				var receivedBytes = new byte[1024 * 100];

				var length = socket.Receive(receivedBytes);
				socket.Shutdown(SocketShutdown.Both);
				socket.Close();

				var response = encoding.GetString(receivedBytes, 0, length);
				var statusCode = int.Parse(Regex.Match(response, "(?<=HTTP/1.1 )\\d+", RegexOptions.IgnoreCase).Value);
				return (HttpStatusCode)statusCode;
			}
			catch (Exception exception)
			{
				Log.Error(exception.Message, exception);
				return HttpStatusCode.ServiceUnavailable;
			}
			finally
			{
				socket?.Dispose();
			}
		}

	}
}
