using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Sop.FileServer.Common
{
    public class HttpHelper
    {

        #region 提交数据get
        ///<summary>
        ///提交数据get
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        /// <param name="strEncoding">编码(gb2312|utf-8)</param>
        public static string SendGetInfo(string Url, string strEncoding = null)
        {
            strEncoding = (strEncoding == null || strEncoding.Trim() == "") ? "utf-8" : strEncoding;
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(strEncoding)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                return "";
            }

        }
        #endregion

        #region 提交数据post

        ///<summary>
        ///提交数据post
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        /// <param name="strEncoding">编码</param>
        ///<returns></returns>
        public static string SendPostInfo(string Url, string strPostdata, string strEncoding = null)
        {
            strEncoding = (strEncoding == null || strEncoding.Trim() == "") ? "utf-8" : strEncoding;
            try
            {
                Encoding encoding = Encoding.UTF8;
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Url);
                request.Method = "post";
                //request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] buffer = encoding.GetBytes(strPostdata);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strEncoding)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
              
                return "";
            }
        }
        #endregion

        public static string HttpGet(string url, Encoding encoding = null)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = encoding ?? Encoding.UTF8;
                return wc.DownloadString(url);

            }
            catch (Exception ex)
            {
             
                return "";
            }

        }


        /// <summary>
        /// 发送post数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postdata, Encoding encoding = null, int timeOut = 18000)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(postdata);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return HttpPost(url, ms, null, encoding, timeOut: timeOut);
            }
        }

        /// <summary>
        /// post提交表单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileDictionary"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> fileDictionary = null, Encoding encoding = null, int timeOut = 18000)
        {
            return HttpPost(url, null, fileDictionary, encoding, timeOut: timeOut);
        }

        public static string HttpPost(string url, Stream postStream = null, Dictionary<string, string> fileDictionary = null, Encoding encoding = null, int timeOut = 18000)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = timeOut;

                #region 处理Form表单文件上传
                var formUploadFile = fileDictionary != null && fileDictionary.Count > 0;//是否用Form上传文件
                if (formUploadFile)
                {
                    //通过表单上传文件

                    postStream = new MemoryStream();

                    string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                    //byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                    string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";

                    foreach (var file in fileDictionary)
                    {
                        try
                        {
                            var fileName = file.Value;
                            //准备文件流
                            using (var fileStream = GetFileStream(fileName))
                            {
                                var formdata = string.Format(formdataTemplate, file.Key, Path.GetFileName(fileName) /*Path.GetFileName(fileName)*/);
                                var formdataBytes = Encoding.ASCII.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                                postStream.Write(formdataBytes, 0, formdataBytes.Length);

                                //写入文件
                                byte[] buffer = new byte[1024];
                                int bytesRead = 0;
                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    postStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    //结尾
                    var footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    postStream.Write(footer, 0, footer.Length);

                    request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                }
                else
                {
                    request.ContentType = "application/json;charset=UTF-8";//"application/x-www-form-urlencoded";
                }
                #endregion

                request.ContentLength = postStream != null ? postStream.Length : 0;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";

                #region 输入二进制流
                if (postStream != null)
                {
                    postStream.Position = 0;

                    //直接写入流
                    Stream requestStream = request.GetRequestStream();

                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }

                    postStream.Close();//关闭文件访问
                }
                #endregion

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
                    {
                        string retString = myStreamReader.ReadToEnd();
                        return retString;
                    }
                }
            }
            catch (Exception ex)
            {
              
                return "";
            }

        }


        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return fileStream;
        }


        public static string PostMoths(string url, string param)
        {
            try
            {
                string strURL = url;
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                string strValue = "";
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
                return strValue;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string HttpPost(string url, out Exception exception)
        {
            url = (url.IndexOf("http://", StringComparison.Ordinal) > -1 || url.IndexOf("https://", StringComparison.Ordinal) > -1) ? url : "http://" + url;
            try
            {
                exception = null;

                ////处理HttpWebRequest访问https有安全证书的问题（ 请求被中止: 未能创建 SSL/ TLS 安全通道。）
                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

                myHttpWebRequest.ServicePoint.Expect100Continue = true;//加快载入速度
                myHttpWebRequest.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                myHttpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
                myHttpWebRequest.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                myHttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");//定义gzip压缩页面支持

                // 有些网站会阻止程序访问，需要加入下面这句
                myHttpWebRequest.AllowAutoRedirect = false;//禁止自动跳转
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.86 Safari/537.36";
                myHttpWebRequest.Accept =
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.Timeout = 60000;  //1分钟超时 


                // Sends the HttpWebRequest and waits for a response.
                using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
                {
                    // 判断是否重定向  Ambiguous 300  Found 302  Moved 301
                    if (myHttpWebResponse.StatusCode == HttpStatusCode.Ambiguous ||
                        myHttpWebResponse.StatusCode == HttpStatusCode.Found ||
                        myHttpWebResponse.StatusCode == HttpStatusCode.Moved)
                    {
                        string newUrl = myHttpWebResponse.Headers["Location"];//获取重定向的网址
                        if (!string.IsNullOrEmpty(newUrl))
                        {
                            myHttpWebResponse.Close();
                            return HttpPost(newUrl, out exception);
                        }
                    }
                    if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return GetContent(myHttpWebResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                return string.Empty;
            }
            return null;
        }

        public static string GetContent(HttpWebResponse myHttpWebResponse)
        {
            using (var stream = myHttpWebResponse.GetResponseStream())
            {
                if (stream == null)
                {
                    return null;
                }
                var content = myHttpWebResponse.ContentEncoding.ToLower();
                if (content == "gzip")
                {
                    using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        using (var myStreamReader = new StreamReader(zipStream, Encoding.UTF8))
                        {
                            string retString = myStreamReader.ReadToEnd();
                            return retString;
                        }
                    }
                }
                else if (content == "deflate")
                {
                    using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
                    {
                        using (var myStreamReader = new StreamReader(deflateStream, Encoding.UTF8))
                        {
                            string retString = myStreamReader.ReadToEnd();
                            return retString;
                        }
                    }
                }
                else
                {
                    using (StreamReader myStreamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string retString = myStreamReader.ReadToEnd();
                        return retString;
                    }
                }


            }
        }
    }
}
