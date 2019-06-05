using System;
using System.Net;

namespace Sop.FileServer.Spider.Proxy
{
	public interface IProxyValidator
	{
		IWebProxy WebProxy { get; }
	 
		bool IsAvailable(WebProxy proxy);
		bool IsAvailable(WebProxy proxy, string targetUrl);
		bool IsAvailable(WebProxy proxy, string targetUrl, TimeSpan timeout);
	 
	}
}
