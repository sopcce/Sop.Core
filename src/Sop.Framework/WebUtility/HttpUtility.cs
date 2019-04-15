using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Sop.Framework.SystemLog;
using Sop.Framework.Utility;

namespace Sop.Framework.WebUtility
{

    public class HttpUtility
    {
        #region Instance

        private static volatile HttpUtility _instance = null;
        private static readonly object Lock = new object();
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public static HttpUtility Instance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HttpUtility();
                    }
                }
            }
            return _instance;
        }

        #endregion Instance


        /// <summary>
        /// 提交Get数据
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="strEncoding">编码(gb2312|utf-8)</param>
        public string SendGet(string url, string strEncoding = null)
        {
            strEncoding = (strEncoding == null || strEncoding.Trim() == "") ? "utf-8" : strEncoding;
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(url);
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                if (respStream != null)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(strEncoding)))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance().Write($"【Sop.Framework.WebUtility.HttpUtility】SendGet异常,URL:{url}", ex, typeof(HttpUtility), LogLevel.Error);
            }
            return null;
        }
        /// <summary>
        /// 提交Post数据t
        /// </summary>
        /// <param name="Url">url地址</param>
        /// <param name="strPostdata">发送的数据</param>
        ///  <param name="strEncoding">编码</param>
        /// <returns></returns>
        public string SendPost(string Url, string strPostdata, string strEncoding = null)
        {
            strEncoding = (strEncoding == null || strEncoding.Trim() == "") ? "utf-8" : strEncoding;
            try
            {
                Encoding encoding = Encoding.UTF8;
                System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] buffer = encoding.GetBytes(strPostdata);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (System.IO.StreamReader reader = new StreamReader(response?.GetResponseStream(), Encoding.GetEncoding(strEncoding)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Log.Instance().Write($"【Sop.Framework.WebUtility.HttpUtility】SendPost异常,url:{Url}", ex, typeof(HttpUtility), LogLevel.Error);
                return null;
            }
        }


        public string HttpGet(string url, Encoding encoding = null)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = encoding ?? Encoding.UTF8;
                return wc.DownloadString(url);

            }
            catch (Exception ex)
            {
                Log.Instance().Write($"【Sop.Framework.WebUtility.HttpUtility】HttpGet异常,url:{url}", ex, typeof(HttpUtility), LogLevel.Error);
                return null;
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
        public string HttpPost(string url, string postdata, Encoding encoding = null, int timeOut = 30000)
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
        public string HttpPost(string url, Dictionary<string, string> fileDictionary = null, Encoding encoding = null, int timeOut = 30000)
        {
            return HttpPost(url, null, fileDictionary, encoding, timeOut: timeOut);
        }

        public string HttpPost(string url, Stream postStream = null, Dictionary<string, string> fileDictionary = null, Encoding encoding = null, int timeOut = 30000)
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
                            using (var fileStream = FileUtility.GetFileStream(fileName))
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

                request.ContentLength = postStream?.Length ?? 0;
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
                Log.Instance().Write($"【Sop.Framework.WebUtility.HttpUtility】HttpPost异常,url:{url}", ex, typeof(HttpUtility), LogLevel.Error);
                return "";
            }

        }


    }
}
