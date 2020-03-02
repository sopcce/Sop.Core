using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Sop.Core.Web
{
  /// <summary>
  /// 网页抓取帮助
  /// </summary>
  public class HttpWebHelper
  {
    #region 构造函数
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cookie"></param>
    public HttpWebHelper(CookieContainer cookie)
    {
      cookieContainer = cookie;
    }

    /// <summary>
    /// 
    /// </summary>
    public HttpWebHelper()
    {
      cookieContainer = new CookieContainer();
    }

    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
      return true; //总是接受
    }
    /// <summary>
    /// cookie集合 
    /// </summary>
    private CookieContainer cookieContainer;
    #endregion

    #region Get
    /// <summary>
    /// 获取页面html   encodingname:gb2312
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <returns></returns>
    public string Get(string uri)
    {
      return Get(uri, uri, "gb2312");
    }

    /// <summary>
    /// 获取页面html   encodingname:gb2312
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <returns></returns>
    public string Get(string uri, string refererUri)
    {
      return Get(uri, refererUri, "gb2312");
    }

    /// <summary>
    /// 获取页面html
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <param name="encodingName">编码名称  例如：gb2312</param>
    /// <returns></returns>
    public string Get(string uri, string refererUri, string encodingName)
    {
      return Get(uri, refererUri, encodingName, (WebProxy)null);
    }

    /// <summary>
    /// 获取页面html
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <param name="encodingName">编码名称  例如：gb2312</param>
    /// <param name="webproxy">代理</param>
    /// <returns></returns>
    public string Get(string uri, string refererUri, string encodingName, WebProxy webproxy)
    {
      string html = string.Empty;

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

      request.ContentType = "text/html;charset=" + encodingName;
      request.Method = "Get";
      request.CookieContainer = cookieContainer;

      if (null != webproxy)
      {
        request.Proxy = webproxy;
        if (null != webproxy.Credentials)
          request.UseDefaultCredentials = true;
      }

      if (!string.IsNullOrEmpty(refererUri))
        request.Referer = refererUri;

      using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
      {
        using (Stream streamResponse = response.GetResponseStream())
        {
          using (StreamReader streamResponseReader = new StreamReader(streamResponse, Encoding.GetEncoding(encodingName)))
          {
            html = streamResponseReader.ReadToEnd();
          }
        }
      }

      return html;

    }

   

    /// <summary>
    /// 创建GET方式的HTTP请求
    /// </summary>
    /// <param name="url">请求的URL</param>
    /// <param name="timeout">请求的超时时间</param>
    /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
    /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
    /// <returns></returns>
    public static HttpWebResponse Get(string url, int? timeout, string userAgent, CookieCollection cookies)
    {
      if (string.IsNullOrEmpty(url))
      {
        throw new ArgumentNullException("url");
      }
      HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
      request.Method = "GET";
      request.UserAgent = DefaultUserAgent;
      if (!string.IsNullOrEmpty(userAgent))
      {
        request.UserAgent = userAgent;
      }
      if (timeout.HasValue)
      {
        request.Timeout = timeout.Value;
      }
      if (cookies != null)
      {
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(cookies);
      }
      return request.GetResponse() as HttpWebResponse;
    }
    #endregion

    #region GetBytes
    /// <summary>
    /// 获取文件或图片 （验证码）
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <returns></returns>
    public Byte[] GetBytes(string uri)
    {
      return GetBytes(uri, uri);
    }

    /// <summary>
    /// 获取文件或图片 （验证码）
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <returns></returns>
    public Byte[] GetBytes(string uri, string refererUri)
    {
      return GetBytes(uri, refererUri, (WebProxy)null);
    }

    /// <summary>
    /// 获取文件或图片 （验证码）
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <returns></returns>
    public Byte[] GetBytes(string uri, string refererUri, WebProxy webproxy)
    {
      byte[] buffer = new byte[1024];

      using (Stream responseStream = GetStream(uri, refererUri, webproxy))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          int count = 0;
          do
          {
            count = responseStream.Read(buffer, 0, buffer.Length);
            memoryStream.Write(buffer, 0, count);

          } while (count != 0);

          return memoryStream.ToArray();
        }
      }
    }
    #endregion


    #region GetStream
    /// <summary>
    /// 获取文件或图片 （验证码）
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <returns></returns>
    public Stream GetStream(string uri)
    {
      return GetStream(uri, uri);
    }

    /// <summary>
    /// 获取文件或图片 （验证码）
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <returns></returns>
    public Stream GetStream(string uri, string refererUri)
    {
      return GetStream(uri, refererUri, (WebProxy)null);
    }

    /// <summary>
    /// 获取文件或图片 （验证码）
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <param name="webproxy">代理</param>
    /// <returns></returns>
    public Stream GetStream(string uri, string refererUri, WebProxy webproxy)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

      request.Method = "GET";
      request.CookieContainer = cookieContainer;
      if (null != webproxy)
      {
        request.Proxy = webproxy;
        if (null != webproxy.Credentials)
          request.UseDefaultCredentials = true;
      }

      if (!string.IsNullOrEmpty(refererUri))
        request.Referer = refererUri;

      using (HttpWebResponse reponse = (HttpWebResponse)request.GetResponse())
      {
        return reponse.GetResponseStream();
      }
    }
    #endregion

    #region Post
    public string PostFile(string url, string[] files, NameValueCollection formFields = null)
    {
      string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.ContentType = "multipart/form-data; boundary=" + boundary;
      request.Method = "POST";
      request.KeepAlive = true;

      Stream memStream = new System.IO.MemoryStream();

      var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
      var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--");


      string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

      if (formFields != null)
      {
        foreach (string key in formFields.Keys)
        {
          string formitem = string.Format(formdataTemplate, key, formFields[key]);
          byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
          memStream.Write(formitembytes, 0, formitembytes.Length);
        }
      }

      string headerTemplate =
          "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
          "Content-Type: application/octet-stream\r\n\r\n";

      for (int i = 0; i < files.Length; i++)
      {
        memStream.Write(boundarybytes, 0, boundarybytes.Length);
        var header = string.Format(headerTemplate, "uplTheFile", files[i]);
        var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

        memStream.Write(headerbytes, 0, headerbytes.Length);

        using (var fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
        {

          var buffer = new byte[1024];
          var bytesRead = 0;
          while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
          {
            memStream.Write(buffer, 0, bytesRead);
          }
        }
      }

      memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
      request.ContentLength = memStream.Length;

      using (Stream requestStream = request.GetRequestStream())
      {
        memStream.Position = 0;
        byte[] tempBuffer = new byte[memStream.Length];
        memStream.Read(tempBuffer, 0, tempBuffer.Length);
        memStream.Close();
        requestStream.Write(tempBuffer, 0, tempBuffer.Length);
      }

      using (var response = request.GetResponse())
      {
        Stream stream2 = response.GetResponseStream();
        StreamReader reader2 = new StreamReader(stream2);
        return reader2.ReadToEnd();
      }
    }

    public string PostFile(string url, List<Stream> files, NameValueCollection formFields = null)
    {
      string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.ContentType = "multipart/form-data; boundary=" + boundary;
      request.Method = "POST";
      request.KeepAlive = true;
      Stream memStream = new MemoryStream();
      var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
      var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--");


      string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

      if (formFields != null)
      {
        foreach (string key in formFields.Keys)
        {
          string formitem = string.Format(formdataTemplate, key, formFields[key]);
          byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
          memStream.Write(formitembytes, 0, formitembytes.Length);
        }
      }

      string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" + "Content-Type: application/octet-stream\r\n\r\n";

      for (int i = 0; i < files.Count; i++)
      {
        memStream.Write(boundarybytes, 0, boundarybytes.Length);
        var header = string.Format(headerTemplate, "uplTheFile", files[i]);
        var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

        memStream.Write(headerbytes, 0, headerbytes.Length);

        memStream.CopyTo(files[i]);
        memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
      }


      request.ContentLength = memStream.Length;
      using (Stream requestStream = request.GetRequestStream())
      {
        memStream.Position = 0;
        byte[] tempBuffer = new byte[memStream.Length];
        memStream.Read(tempBuffer, 0, tempBuffer.Length);
        memStream.Close();
        requestStream.Write(tempBuffer, 0, tempBuffer.Length);
      }

      using (var response = request.GetResponse())
      {
        Stream stream2 = response.GetResponseStream();
        StreamReader reader2 = new StreamReader(stream2);
        return reader2.ReadToEnd();
      }
    }

    /// <summary>
    /// POST提交        默认GB2312
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="postData">提交的数据</param>
    /// <returns></returns>
    public string Post(string uri, string postData)
    {
      return Post(uri, uri, postData, "gb2312");
    }

    /// <summary>
    /// POST提交        默认GB2312
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <param name="postData">提交的数据</param>
    /// <returns></returns>
    public string Post(string uri, string refererUri, string postData)
    {
      return Post(uri, refererUri, postData, "gb2312");
    }

    /// <summary>
    /// POST提交    
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <param name="postData">提交的数据</param>
    /// <param name="encodingName">编码名称  例如：gb2312</param>
    /// <returns></returns>
    public string Post(string uri, string refererUri, string postData, string encodingName)
    {
      return Post(uri, refererUri, postData, encodingName, (WebProxy)null);
    }

    /// <summary>
    /// POST提交    
    /// </summary>
    /// <param name="uri">访问url</param>
    /// <param name="refererUri">来源url</param>
    /// <param name="postData">提交的数据</param>
    /// <param name="encodingName">编码名称  例如：gb2312</param>
    /// <param name="webproxy">代理</param>
    /// <returns></returns>
    public string Post(string uri, string refererUri, string postData, string encodingName, WebProxy webproxy)
    {
      string html = string.Empty;

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
      request.Accept = "*/*";
      request.Headers.Add("Accept-Language", "zh-cn");
      request.ContentType = "application/x-www-form-urlencoded";
      request.Headers.Add("UA-CPU", "x86");
      request.Headers.Add("Accept-Encoding", "gzip, deflate");
      request.UserAgent = DefaultUserAgent;
      request.Headers.Add("Cache-Control", "no-cache");
      request.CookieContainer = cookieContainer;
      request.Method = "POST";
      if (!string.IsNullOrEmpty(refererUri))
        request.Referer = refererUri;
      if (null != webproxy)
      {
        request.Proxy = webproxy;
        if (null != webproxy.Credentials)
          request.UseDefaultCredentials = true;
      }
      Encoding encode = Encoding.GetEncoding(encodingName);
      byte[] bytesSend = encode.GetBytes(postData);
      request.ContentLength = bytesSend.Length;

      StringBuilder cookieTextSend = new StringBuilder();
      if (cookieContainer != null)
      {
        CookieCollection cc = cookieContainer.GetCookies(request.RequestUri);

        foreach (System.Net.Cookie cokie in cc)
        {
          cookieTextSend.Append(cokie + ";");
        }
      }
      if (cookieTextSend.Length > 0)
      {
        long time;
        time = DateTime.UtcNow.Ticks - (new DateTime(1970, 1, 1)).Ticks;
        time = (long)(time / 10000);
        request.Headers.Add("Cookie", string.Concat("cookLastGetMsgTime=", time.ToString(), "; ", cookieTextSend.ToString()));
      }

      using (Stream streamSend = request.GetRequestStream())
      {
        streamSend.Write(bytesSend, 0, bytesSend.Length);
      }

      using (HttpWebResponse reponse = (HttpWebResponse)request.GetResponse())
      {
        using (Stream streamRespone = reponse.GetResponseStream())
        {
          using (StreamReader streamReaderResponse = new StreamReader(streamRespone, encode))
          {
            html = streamReaderResponse.ReadToEnd();
          }
        }
      }

      return html;
    }

    /// <summary>
    /// 创建POST方式的HTTP请求
    /// </summary>
    /// <param name="url">请求的URL</param>
    /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>
    /// <param name="timeout">请求的超时时间</param>
    /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
    /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
    /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
    /// <returns></returns>
    public static HttpWebResponse Post(string url, IDictionary<string, string> parameters, int? timeout, string userAgent,
      Encoding requestEncoding = null, CookieCollection cookies = null)
    {
      if (string.IsNullOrEmpty(url))
      {
        throw new ArgumentNullException("url");
      }
      HttpWebRequest request = null;
      //如果是发送HTTPS请求
      if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
      {
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        request = WebRequest.Create(url) as HttpWebRequest;
        request.ProtocolVersion = HttpVersion.Version10;
      }
      else
      {
        request = WebRequest.Create(url) as HttpWebRequest;
      }
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";

      if (!string.IsNullOrEmpty(userAgent))
      {
        request.UserAgent = userAgent;
      }
      else
      {
        request.UserAgent = DefaultUserAgent;
      }

      requestEncoding = requestEncoding ?? Encoding.UTF8;

      if (timeout.HasValue)
      {
        request.Timeout = timeout.Value;
      }
      if (cookies != null)
      {
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(cookies);
      }
      //如果需要POST数据
      if (!(parameters == null || parameters.Count == 0))
      {
        StringBuilder buffer = new StringBuilder();
        int i = 0;
        foreach (string key in parameters.Keys)
        {
          if (i > 0)
          {
            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
          }
          else
          {
            buffer.AppendFormat("{0}={1}", key, parameters[key]);
          }
          i++;
        }

        byte[] data = requestEncoding.GetBytes(buffer.ToString());
        using (Stream stream = request.GetRequestStream())
        {
          stream.Write(data, 0, data.Length);
        }
      }
      return request.GetResponse() as HttpWebResponse;
    }
    #endregion


    public static byte[] StreamToBytes(Stream stream)
    {
      byte[] buffer = new byte[stream.Length];
      stream.Read(buffer, 0, buffer.Length);
      stream.Seek(0, SeekOrigin.Begin);
      return buffer;
    }

  }
}

