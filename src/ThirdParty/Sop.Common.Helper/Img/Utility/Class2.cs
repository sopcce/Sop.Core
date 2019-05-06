using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using RE = System.Text.RegularExpressions.Regex;
using System.Security.Cryptography.X509Certificates;

/***************************************************************************************************************************************************  
 * *文件名：HttpProc.cs  
 * *创建人：kenter  
 * *日 期：2010.02.23 修改  
* *描 述：实现HTTP协议中的GET、POST请求  
* *使 用：HttpProc.WebClient client = new HttpProc.WebClient();  
 client.Encoding = System.Text.Encoding.Default;//默认编码方式，根据需要设置其他类型  
client.OpenRead("http://www.baidu.com");//普通get请求  
MessageBox.Show(client.RespHtml);//获取返回的网页源代码  
client.DownloadFile("http://www.codepub.com/upload/163album.rar",@"C:\163album.rar");//下载文件  
client.OpenRead("http://passport.baidu.com/?login","username=zhangsan&password=123456");//提交表单，此处是登录百度的示例  
client.UploadFile("http://hiup.baidu.com/zhangsan/upload", @"file1=D:\1.mp3");//上传文件  
client.UploadFile("http://hiup.baidu.com/zhangsan/upload", "folder=myfolder&size=4003550",@"file1=D:\1.mp3");//提交含文本域和文件域的表单  
*****************************************************************************************************************************************************/

namespace HttpProc
{
  ///<summary>  
  ///上传事件委托  
  ///</summary>  
  ///<param name="sender"></param>  
  ///<param name="e"></param>  
  public delegate void WebClientUploadEvent(object sender, HttpProc.UploadEventArgs e);

  ///<summary>  
  ///下载事件委托  
  ///</summary>  
  ///<param name="sender"></param>  
  ///<param name="e"></param>  
  public delegate void WebClientDownloadEvent(object sender, HttpProc.DownloadEventArgs e);


  ///<summary>  
  ///上传事件参数  
  ///</summary>  
  public struct UploadEventArgs
  {
    ///<summary>  
    ///上传数据总大小  
    ///</summary>  
    public long totalBytes;
    ///<summary>  
    ///已发数据大小  
    ///</summary>  
    public long bytesSent;
    ///<summary>  
    ///发送进度(0-1)  
    ///</summary>  
    public double sendProgress;
    ///<summary>  
    ///发送速度Bytes/s  
    ///</summary>  
    public double sendSpeed;
  }

  ///<summary>  
  ///下载事件参数  
  ///</summary>  
  public struct DownloadEventArgs
  {
    ///<summary>  
    ///下载数据总大小  
    ///</summary>  
    public long totalBytes;
    ///<summary>  
    ///已接收数据大小  
    ///</summary>  
    public long bytesReceived;
    ///<summary>  
    ///接收数据进度(0-1)  
    ///</summary>  
    public double ReceiveProgress;
    ///<summary>  
    ///当前缓冲区数据  
    ///</summary>  
    public byte[] receivedBuffer;
    ///<summary>  
    ///接收速度Bytes/s  
    ///</summary>  
    public double receiveSpeed;
  }

  ///<summary>  
  ///实现向WEB服务器发送和接收数据  
  ///</summary>  
  public class WebClient
  {
    private WebHeaderCollection requestHeaders, responseHeaders;
    private TcpClient clientSocket;
    private MemoryStream postStream;
    private Encoding encoding = Encoding.Default;
    private const string BOUNDARY = "--HEDAODE--";
    private const int SEND_BUFFER_SIZE = 10245;
    private const int RECEIVE_BUFFER_SIZE = 10245;
    private string cookie = "";
    private string respHtml = "";
    private string strRequestHeaders = "";
    private string strResponseHeaders = "";
    private int statusCode = 0;
    private bool isCanceled = false;
    public event WebClientUploadEvent UploadProgressChanged;
    public event WebClientDownloadEvent DownloadProgressChanged;

    ///<summary>  
    ///初始化WebClient类  
    ///</summary>  
    public WebClient()
    {
      responseHeaders = new WebHeaderCollection();
      requestHeaders = new WebHeaderCollection();
    }

    /// <summary>  
    /// 获得字符串中开始和结束字符串中间得值  
    /// </summary>  
    /// <param name="str"></param>  
    /// <param name="s">开始</param>  
    /// <param name="e">结束</param>  
    /// <returns></returns>  
    public string gethtmlContent(string str, string s, string e)
    {
      Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
      return rg.Match(str).Value;
    }

    /// <summary>  
    /// 过滤HTML字符  
    /// </summary>  
    /// <param name="source"></param>  
    /// <returns></returns>  
    public string htmlConvert(string source)
    {
      string result;

      //remove line breaks,tabs  
      result = source.Replace("\r", " ");
      result = result.Replace("\n", " ");
      result = result.Replace("\t", " ");

      //remove the header  
      result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);

      result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
      result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);

      //remove all styles  
      result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase); //clearing attributes  
      result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);

      //insert tabs in spaces of <td> tags  
      result = Regex.Replace(result, @"<( )*td([^>])*>", " ", RegexOptions.IgnoreCase);

      //insert line breaks in places of <br> and <li> tags  
      result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
      result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);

      //insert line paragraphs in places of <tr> and <p> tags  
      result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
      result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);

      //remove anything thats enclosed inside < >  
      result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);

      //replace special characters:  
      result = Regex.Replace(result, @"&amp;", "&", RegexOptions.IgnoreCase);
      result = Regex.Replace(result, @"&nbsp;", " ", RegexOptions.IgnoreCase);
      result = Regex.Replace(result, @"&lt;", "<", RegexOptions.IgnoreCase);
      result = Regex.Replace(result, @"&gt;", ">", RegexOptions.IgnoreCase);
      result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);

      //remove extra line breaks and tabs  
      result = Regex.Replace(result, @" ( )+", " ");
      result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r");
      result = Regex.Replace(result, @"(\r\r)+", "\r\n");

      return result;
    }

    ///<summary>  
    ///读取指定URL的文本  
    ///</summary>  
    ///<param name="URL">请求的地址</param>  
    ///<returns>服务器响应文本</returns>  
    public string OpenRead(string URL)
    {
      requestHeaders.Add("Connection", "close");
      SendRequestData(URL, "GET");
      return GetHtml();
    }


    //解决证书过期无法访问的问题  
    class CertPolicy //: ICertificatePolicy
    {
      public bool CheckValidationResult(ServicePoint srvpt, X509Certificate cert, WebRequest req, int certprb)
      { return true; }
    }

    ///<summary>  
    ///采用https协议访问网络  
    ///</summary>  
    ///<param name="URL">url地址</param>  
    ///<param name="strPostdata">发送的数据</param>  
    ///<returns></returns>  
    public string OpenReadWithHttps(string URL, string strPostdata)
    {
      //ServicePointManager.CertificatePolicy = new CertPolicy();
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
      request.CookieContainer = new CookieContainer();
      request.Method = "POST";
      request.Accept = "*/*";
      request.ContentType = "application/x-www-form-urlencoded";
      byte[] buffer = this.encoding.GetBytes(strPostdata);
      request.ContentLength = buffer.Length;
      request.GetRequestStream().Write(buffer, 0, buffer.Length);
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
      this.respHtml = reader.ReadToEnd();
      foreach (System.Net.Cookie ck in response.Cookies)
      {
        this.cookie += ck.Name + "=" + ck.Value + ";";
      }
      reader.Close();
      return respHtml;
    }

    ///<summary>  
    ///读取指定URL的文本  
    ///</summary>  
    ///<param name="URL">请求的地址</param>  
    ///<param name="postData">向服务器发送的文本数据</param>  
    ///<returns>服务器响应文本</returns>  
    public string OpenRead(string URL, string postData)
    {
      byte[] sendBytes = encoding.GetBytes(postData);
      postStream = new MemoryStream();
      postStream.Write(sendBytes, 0, sendBytes.Length);

      requestHeaders.Add("Content-Length", postStream.Length.ToString());
      requestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
      requestHeaders.Add("Connection", "close");

      SendRequestData(URL, "POST");
      return GetHtml();
    }


    ///<summary>  
    ///读取指定URL的流  
    ///</summary>  
    ///<param name="URL">请求的地址</param>  
    ///<param name="postData">向服务器发送的数据</param>  
    ///<returns>服务器响应流</returns>  
    public Stream GetStream(string URL, string postData)
    {
      byte[] sendBytes = encoding.GetBytes(postData);
      postStream = new MemoryStream();
      postStream.Write(sendBytes, 0, sendBytes.Length);

      requestHeaders.Add("Content-Length", postStream.Length.ToString());
      requestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
      requestHeaders.Add("Connection", "close");

      SendRequestData(URL, "POST");

      MemoryStream ms = new MemoryStream();
      SaveNetworkStream(ms);
      return ms;
    }


    ///<summary>  
    ///上传文件到服务器  
    ///</summary>  
    ///<param name="URL">请求的地址</param>  
    ///<param name="fileField">文件域(格式如:file1=C:\test.mp3&file2=C:\test.jpg)</param>  
    ///<returns>服务器响应文本</returns>  
    public string UploadFile(string URL, string fileField)
    {
      return UploadFile(URL, "", fileField);
    }

    ///<summary>  
    ///上传文件和数据到服务器  
    ///</summary>  
    ///<param name="URL">请求地址</param>  
    ///<param name="textField">文本域(格式为:name1=value1&name2=value2)</param>  
    ///<param name="fileField">文件域(格式如:file1=C:\test.mp3&file2=C:\test.jpg)</param>  
    ///<returns>服务器响应文本</returns>  
    public string UploadFile(string URL, string textField, string fileField)
    {
      postStream = new MemoryStream();

      if (textField != "" && fileField != "")
      {
        WriteTextField(textField);
        WriteFileField(fileField);
      }
      else if (fileField != "")
      {
        WriteFileField(fileField);
      }
      else if (textField != "")
      {
        WriteTextField(textField);
      }
      else
        throw new Exception("文本域和文件域不能同时为空。");

      //写入结束标记  
      byte[] buffer = encoding.GetBytes("--" + BOUNDARY + "--\r\n");
      postStream.Write(buffer, 0, buffer.Length);

      //添加请求标头  
      requestHeaders.Add("Content-Length", postStream.Length.ToString());
      requestHeaders.Add("Content-Type", "multipart/form-data; boundary=" + BOUNDARY);
      requestHeaders.Add("Connection", "Keep-Alive");

      //发送请求数据  
      SendRequestData(URL, "POST", true);

      //返回响应文本  
      return GetHtml();
    }


    ///<summary>  
    ///分析文本域，添加到请求流  
    ///</summary>  
    ///<param name="textField">文本域</param>  
    private void WriteTextField(string textField)
    {
      string[] strArr = RE.Split(textField, "&");
      textField = "";
      foreach (string var in strArr)
      {
        Match M = RE.Match(var, "([^=]+)=(.+)");
        textField += "--" + BOUNDARY + "\r\n";
        textField += "Content-Disposition: form-data; name=\"" + M.Groups[1].Value + "\"\r\n\r\n" + M.Groups[2].Value + "\r\n";
      }
      byte[] buffer = encoding.GetBytes(textField);
      postStream.Write(buffer, 0, buffer.Length);
    }

    ///<summary>  
    ///分析文件域，添加到请求流  
    ///</summary>  
    ///<param name="fileField">文件域</param>  
    private void WriteFileField(string fileField)
    {
      string filePath = "";
      int count = 0;
      string[] strArr = RE.Split(fileField, "&");
      foreach (string var in strArr)
      {
        Match M = RE.Match(var, "([^=]+)=(.+)");
        filePath = M.Groups[2].Value;
        fileField = "--" + BOUNDARY + "\r\n";
        fileField += "Content-Disposition: form-data; name=\"" + M.Groups[1].Value + "\"; filename=\"" + Path.GetFileName(filePath) + "\"\r\n";
        fileField += "Content-Type: image/jpeg\r\n\r\n";

        byte[] buffer = encoding.GetBytes(fileField);
        postStream.Write(buffer, 0, buffer.Length);

        //添加文件数据  
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        buffer = new byte[50000];

        do
        {
          count = fs.Read(buffer, 0, buffer.Length);
          postStream.Write(buffer, 0, count);

        } while (count > 0);

        fs.Close();
        fs.Dispose();
        fs = null;

        buffer = encoding.GetBytes("\r\n");
        postStream.Write(buffer, 0, buffer.Length);
      }
    }

    ///<summary>  
    ///从指定URL下载数据流  
    ///</summary>  
    ///<param name="URL">请求地址</param>  
    ///<returns>数据流</returns>  
    public Stream DownloadData(string URL)
    {
      requestHeaders.Add("Connection", "close");
      SendRequestData(URL, "GET");
      MemoryStream ms = new MemoryStream();
      SaveNetworkStream(ms, true);
      return ms;
    }


    ///<summary>  
    ///从指定URL下载文件  
    ///</summary>  
    ///<param name="URL">文件URL地址</param>  
    ///<param name="fileName">文件保存路径,含文件名(如:C:\test.jpg)</param>  
    public void DownloadFile(string URL, string fileName)
    {
      requestHeaders.Add("Connection", "close");
      SendRequestData(URL, "GET");
      FileStream fs = new FileStream(fileName, FileMode.Create);
      SaveNetworkStream(fs, true);
      fs.Close();
      fs = null;
    }

    ///<summary>  
    ///向服务器发送请求  
    ///</summary>  
    ///<param name="URL">请求地址</param>  
    ///<param name="method">POST或GET</param>  
    ///<param name="showProgress">是否显示上传进度</param>  
    private void SendRequestData(string URL, string method, bool showProgress)
    {
      clientSocket = new TcpClient();
      Uri URI = new Uri(URL);
      clientSocket.Connect(URI.Host, URI.Port);

      requestHeaders.Add("Host", URI.Host);
      byte[] request = GetRequestHeaders(method + " " + URI.PathAndQuery + " HTTP/1.1");
      clientSocket.Client.Send(request);

      //若有实体内容就发送它  
      if (postStream != null)
      {
        byte[] buffer = new byte[SEND_BUFFER_SIZE];
        int count = 0;
        Stream sm = clientSocket.GetStream();
        postStream.Position = 0;

        UploadEventArgs e = new UploadEventArgs();
        e.totalBytes = postStream.Length;
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();//计时器  
        timer.Start();
        do
        {
          //如果取消就推出  
          if (isCanceled) { break; }

          //读取要发送的数据  
          count = postStream.Read(buffer, 0, buffer.Length);
          //发送到服务器  
          sm.Write(buffer, 0, count);

          //是否显示进度  
          if (showProgress)
          {
            //触发事件  
            e.bytesSent += count;
            e.sendProgress = (double)e.bytesSent / (double)e.totalBytes;
            double t = timer.ElapsedMilliseconds / 1000;
            t = t <= 0 ? 1 : t;
            e.sendSpeed = (double)e.bytesSent / t;
            if (UploadProgressChanged != null) { UploadProgressChanged(this, e); }
          }

        } while (count > 0);
        timer.Stop();
        postStream.Close();
        //postStream.Dispose();  
        postStream = null;

      }//end if  

    }

    ///<summary>  
    ///向服务器发送请求  
    ///</summary>  
    ///<param name="URL">请求URL地址</param>  
    ///<param name="method">POST或GET</param>  
    private void SendRequestData(string URL, string method)
    {
      SendRequestData(URL, method, false);
    }


    ///<summary>  
    ///获取请求头字节数组  
    ///</summary>  
    ///<param name="request">POST或GET请求</param>  
    ///<returns>请求头字节数组</returns>  
    private byte[] GetRequestHeaders(string request)
    {
      requestHeaders.Add("Accept", "*/*");
      requestHeaders.Add("Accept-Language", "zh-cn");
      requestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");

      string headers = request + "\r\n";

      foreach (string key in requestHeaders)
      {
        headers += key + ":" + requestHeaders[key] + "\r\n";
      }

      //有Cookie就带上Cookie  
      if (cookie != "") { headers += "Cookie:" + cookie + "\r\n"; }

      //空行，请求头结束  
      headers += "\r\n";

      strRequestHeaders = headers;
      requestHeaders.Clear();
      return encoding.GetBytes(headers);
    }



    ///<summary>  
    ///获取服务器响应文本  
    ///</summary>  
    ///<returns>服务器响应文本</returns>  
    private string GetHtml()
    {
      MemoryStream ms = new MemoryStream();
      SaveNetworkStream(ms);//将网络流保存到内存流  
      StreamReader sr = new StreamReader(ms, encoding);
      respHtml = sr.ReadToEnd();
      sr.Close(); ms.Close();
      return respHtml;
    }

    ///<summary>  
    ///将网络流保存到指定流  
    ///</summary>  
    ///<param name="toStream">保存位置</param>  
    ///<param name="needProgress">是否显示进度</param>  
    private void SaveNetworkStream(Stream toStream, bool showProgress)
    {
      //获取要保存的网络流  
      NetworkStream NetStream = clientSocket.GetStream();

      byte[] buffer = new byte[RECEIVE_BUFFER_SIZE];
      int count = 0, startIndex = 0;

      MemoryStream ms = new MemoryStream();
      for (int i = 0; i < 3; i++)
      {
        count = NetStream.Read(buffer, 0, 500);
        ms.Write(buffer, 0, count);
      }

      if (ms.Length == 0) { NetStream.Close(); throw new Exception("远程服务器没有响应"); }

      buffer = ms.GetBuffer();
      count = (int)ms.Length;

      GetResponseHeader(buffer, out startIndex);//分析响应，获取响应头和响应实体  
      count -= startIndex;
      toStream.Write(buffer, startIndex, count);

      DownloadEventArgs e = new DownloadEventArgs();

      if (responseHeaders["Content-Length"] != null)
      { e.totalBytes = long.Parse(responseHeaders["Content-Length"]); }
      else
      { e.totalBytes = -1; }

      //启动计时器  
      System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
      timer.Start();

      do
      {
        //如果取消就推出  
        if (isCanceled) { break; }

        //显示下载进度  
        if (showProgress)
        {
          e.bytesReceived += count;
          e.ReceiveProgress = (double)e.bytesReceived / (double)e.totalBytes;

          byte[] tempBuffer = new byte[count];
          Array.Copy(buffer, startIndex, tempBuffer, 0, count);
          e.receivedBuffer = tempBuffer;

          double t = (timer.ElapsedMilliseconds + 0.1) / 1000;
          e.receiveSpeed = (double)e.bytesReceived / t;

          startIndex = 0;
          if (DownloadProgressChanged != null) { DownloadProgressChanged(this, e); }
        }

        //读取网路数据到缓冲区  
        count = NetStream.Read(buffer, 0, buffer.Length);

        //将缓存区数据保存到指定流  
        toStream.Write(buffer, 0, count);
      } while (count > 0);

      timer.Stop();//关闭计时器  

      if (responseHeaders["Content-Length"] != null)
      {
        toStream.SetLength(long.Parse(responseHeaders["Content-Length"]));
      }
      //else  
      //{  
      // toStream.SetLength(toStream.Length);  
      // responseHeaders.Add("Content-Length", toStream.Length.ToString());//添加响应标头  
      //}  

      toStream.Position = 0;

      //关闭网络流和网络连接  
      NetStream.Close();
      clientSocket.Close();
    }


    ///<summary>  
    ///将网络流保存到指定流  
    ///</summary>  
    ///<param name="toStream">保存位置</param>  
    private void SaveNetworkStream(Stream toStream)
    {
      SaveNetworkStream(toStream, false);
    }



    ///<summary>  
    ///分析响应流，去掉响应头  
    ///</summary>  
    ///<param name="buffer"></param>  
    private void GetResponseHeader(byte[] buffer, out int startIndex)
    {
      responseHeaders.Clear();
      string html = encoding.GetString(buffer);
      StringReader sr = new StringReader(html);

      int start = html.IndexOf("\r\n\r\n") + 4;//找到空行位置  
      strResponseHeaders = html.Substring(0, start);//获取响应头文本  

      //获取响应状态码  
      //  
      if (sr.Peek() > -1)
      {
        //读第一行字符串  
        string line = sr.ReadLine();

        //分析此行字符串,获取服务器响应状态码  
        Match M = RE.Match(line, @"\d\d\d");
        if (M.Success)
        {
          statusCode = int.Parse(M.Value);
        }
      }

      //获取响应头  
      //  
      while (sr.Peek() > -1)
      {
        //读一行字符串  
        string line = sr.ReadLine();

        //若非空行  
        if (line != "")
        {
          //分析此行字符串，获取响应标头  
          Match M = RE.Match(line, "([^:]+):(.+)");
          if (M.Success)
          {
            try
            { //添加响应标头到集合  
              responseHeaders.Add(M.Groups[1].Value.Trim(), M.Groups[2].Value.Trim());
            }
            catch
            { }


            //获取Cookie  
            if (M.Groups[1].Value == "Set-Cookie")
            {
              M = RE.Match(M.Groups[2].Value, "[^=]+=[^;]+");
              cookie += M.Value.Trim() + ";";
            }
          }

        }
        //若是空行，代表响应头结束响应实体开始。（响应头和响应实体间用一空行隔开）  
        else
        {
          //如果响应头中没有实体大小标头，尝试读响应实体第一行获取实体大小  
          if (responseHeaders["Content-Length"] == null && sr.Peek() > -1)
          {
            //读响应实体第一行  
            line = sr.ReadLine();

            //分析此行看是否包含实体大小  
            Match M = RE.Match(line, "~[0-9a-fA-F]{1,15}");

            if (M.Success)
            {
              //将16进制的实体大小字符串转换为10进制  
              int length = int.Parse(M.Value, System.Globalization.NumberStyles.AllowHexSpecifier);
              responseHeaders.Add("Content-Length", length.ToString());//添加响应标头  
              strResponseHeaders += M.Value + "\r\n";
            }
          }
          break;//跳出循环   
        }//End If  
      }//End While  

      sr.Close();

      //实体开始索引  
      startIndex = encoding.GetBytes(strResponseHeaders).Length;
    }


    ///<summary>  
    ///取消上传或下载,要继续开始请调用Start方法  
    ///</summary>  
    public void Cancel()
    {
      isCanceled = true;
    }

    ///<summary>  
    ///启动上传或下载，要取消请调用Cancel方法  
    ///</summary>  
    public void Start()
    {
      isCanceled = false;
    }

    //*************************************************************  
    //以下为属性  
    //*************************************************************  

    ///<summary>  
    ///获取或设置请求头  
    ///</summary>  
    public WebHeaderCollection RequestHeaders
    {
      set { requestHeaders = value; }
      get { return requestHeaders; }
    }

    ///<summary>  
    ///获取响应头集合  
    ///</summary>  
    public WebHeaderCollection ResponseHeaders
    {
      get { return responseHeaders; }
    }

    ///<summary>  
    ///获取请求头文本  
    ///</summary>  
    public string StrRequestHeaders
    {
      get { return strRequestHeaders; }
    }

    ///<summary>  
    ///获取响应头文本  
    ///</summary>  
    public string StrResponseHeaders
    {
      get { return strResponseHeaders; }
    }

    ///<summary>  
    ///获取或设置Cookie  
    ///</summary>  
    public string Cookie
    {
      set { cookie = value; }
      get { return cookie; }
    }

    ///<summary>  
    ///获取或设置编码方式(默认为系统默认编码方式)  
    ///</summary>  
    public Encoding Encoding
    {
      set { encoding = value; }
      get { return encoding; }
    }

    ///<summary>  
    ///获取服务器响应文本  
    ///</summary>  
    public string RespHtml
    {
      get { return respHtml; }
    }


    ///<summary>  
    ///获取服务器响应状态码  
    ///</summary>  
    public int StatusCode
    {
      get { return statusCode; }
    }
  }
}