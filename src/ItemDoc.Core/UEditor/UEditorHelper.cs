using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ItemDoc.Core.UEditor
{
    /// <summary>
    /// UE配置信息类
    /// </summary>
    public static class Config
    {
        private static bool noCache = true;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static JObject BuildItems()
        {
            string path = HttpContext.Current.Server.MapPath("~/scripts/ueditor/config/config.json");
            var json = File.ReadAllText(path);
            return JObject.Parse(json);
        }

        /// <summary>
        /// 
        /// </summary>
        public static JObject GetItems()
        {
            if (noCache || _Items == null)
            {
                _Items = BuildItems();
            }
            return _Items;
        }
        private static JObject _Items;


        public static T GetValue<T>(string key)
        {
            return GetItems()[key].Value<T>();
        }

        public static String[] GetStringList(string key)
        {
            return GetItems()[key].Select(x => x.Value<String>()).ToArray();
        }

        public static String GetString(string key)
        {
            return GetValue<String>(key);
        }

        public static int GetInt(string key)
        {
            return GetValue<int>(key);
        }
    }
    /// <summary>
    /// 所有业务处理基类
    /// </summary>
    public abstract class Handler
    {
        /// <summary>
        /// 处理当前状态
        /// </summary>
        /// <param name="context"></param>
        public Handler(HttpContext context)
        {
            this.Request = context.Request;
            this.Response = context.Response;
            this.Context = context;
            this.Server = context.Server;
        }
        #region MyRegion
        /// <summary>
        /// 
        /// </summary>
        public HttpRequest Request { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public HttpResponse Response { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public HttpContext Context { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public HttpServerUtility Server { get; private set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public abstract void Process();
        /// <summary>
        /// 输出jion数据
        /// </summary>
        /// <param name="response">输出json</param>
        protected void WriteJson(object response)
        {
            string jsonpCallback = Request["callback"],
                json = JsonConvert.SerializeObject(response);
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                Response.AddHeader("Content-Type", "text/plain");
                Response.Write(json);
            }
            else
            {
                Response.AddHeader("Content-Type", "application/javascript");
                Response.Write(String.Format("{0}({1});", jsonpCallback, json));
            }
            Response.End();
        }


    }
    /// <summary>
    /// Config业务处理类
    /// </summary>
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContext context)
            : base(context)
        { }

        public override void Process()
        {
            object str = Config.GetItems();
            WriteJson(str);
        }
    }
    /// <summary>
    /// 抓取远程图片配置
    /// </summary>
    public class CrawlerHandler : Handler
    {
        private string[] Sources;
        private Crawler[] Crawlers;
        public CrawlerHandler(HttpContext context) : base(context) { }

        public override void Process()
        {
            Sources = Request.Form.GetValues("source[]");
            if (Sources == null || Sources.Length == 0)
            {
                WriteJson(new
                {
                    state = "参数错误：没有指定抓取源"
                });
                return;
            }
            Crawlers = Sources.Select(x => new Crawler(x, Server).Fetch()).ToArray();
            WriteJson(new
            {
                state = "SUCCESS",
                list = Crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = x.ServerUrl
                })
            });
        }
    }
    /// <summary>
    /// 上传业务处理类
    /// </summary>
    public class UploadHandler : Handler
    {

        public UploadConfig UploadConfig { get; private set; }
        public UploadResult Result { get; private set; }

        public UploadHandler(HttpContext context, UploadConfig config)
            : base(context)
        {
            this.UploadConfig = config;
            this.Result = new UploadResult() { State = UploadState.Unknown };
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Process()
        {
            byte[] uploadFileBytes = null;
            string uploadFileName = null;

            if (UploadConfig.Base64)
            {
                uploadFileName = UploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(Request[UploadConfig.UploadFieldName]);
            }
            else
            {
                var file = Request.Files[UploadConfig.UploadFieldName];
                uploadFileName = file.FileName;

                if (!CheckFileType(uploadFileName))
                {
                    Result.State = UploadState.TypeNotAllow;
                    WriteResult();
                    return;
                }
                if (!CheckFileSize(file.ContentLength))
                {
                    Result.State = UploadState.SizeLimitExceed;
                    WriteResult();
                    return;
                }

                uploadFileBytes = new byte[file.ContentLength];
                try
                {
                    file.InputStream.Read(uploadFileBytes, 0, file.ContentLength);
                }
                catch (Exception)
                {
                    Result.State = UploadState.NetworkError;
                    WriteResult();
                }
            }

            Result.OriginFileName = uploadFileName;

            var savePath = PathFormatter.Format(uploadFileName, UploadConfig.PathFormat);
            var localPath = Server.MapPath(savePath);

            //todo#存储数据库操作,可以添加水印等操作
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                File.WriteAllBytes(localPath, uploadFileBytes);
                Result.Url = savePath;
                Result.State = UploadState.Success;
            }
            catch (Exception e)
            {
                Result.State = UploadState.FileAccessError;
                Result.ErrorMessage = e.Message;
            }
            finally
            {
                WriteResult();
            }
        }

        private void WriteResult()
        {
            this.WriteJson(new
            {
                state = GetStateMessage(Result.State),
                url = Result.Url,
                title = Result.OriginFileName,
                original = Result.OriginFileName,
                error = Result.ErrorMessage
            });
        }

        private string GetStateMessage(UploadState state)
        {
            switch (state)
            {
                case UploadState.Success:
                    return "SUCCESS";
                case UploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }

        private bool CheckFileType(string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            return UploadConfig.AllowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
        }

        private bool CheckFileSize(int size)
        {
            return size < UploadConfig.SizeLimit;
        }
    }
    /// <summary>
    /// FileManager 的摘要说明
    /// </summary>
    public class ListFileManager : Handler
    {
        enum ResultState
        {
            Success,
            InvalidParam,
            AuthorizError,
            IOError,
            PathNotFound
        }

        private int Start;
        private int Size;
        private int Total;
        private ResultState State;
        private String PathToList;
        private String[] FileList;
        private String[] SearchExtensions;

        public ListFileManager(HttpContext context, string pathToList, string[] searchExtensions)
            : base(context)
        {
            this.SearchExtensions = searchExtensions.Select(x => x.ToLower()).ToArray();
            this.PathToList = pathToList;
        }

        public override void Process()
        {
            try
            {
                Start = String.IsNullOrEmpty(Request["start"]) ? 0 : Convert.ToInt32(Request["start"]);
                Size = String.IsNullOrEmpty(Request["size"]) ? Config.GetInt("imageManagerListSize") : Convert.ToInt32(Request["size"]);
            }
            catch (FormatException)
            {
                State = ResultState.InvalidParam;
                WriteResult();
                return;
            }
            var buildingList = new List<String>();
            try
            {
                var localPath = Server.MapPath(PathToList);
                buildingList.AddRange(Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
                    .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                    .Select(x => PathToList + x.Substring(localPath.Length).Replace("\\", "/")));
                Total = buildingList.Count;
                FileList = buildingList.OrderBy(x => x).Skip(Start).Take(Size).ToArray();
            }
            catch (UnauthorizedAccessException)
            {
                State = ResultState.AuthorizError;
            }
            catch (DirectoryNotFoundException)
            {
                State = ResultState.PathNotFound;
            }
            catch (IOException)
            {
                State = ResultState.IOError;
            }
            finally
            {
                WriteResult();
            }
        }

        private void WriteResult()
        {
            WriteJson(new
            {
                state = GetStateString(),
                list = FileList == null ? null : FileList.Select(x => new { url = x }),
                start = Start,
                size = Size,
                total = Total
            });
        }

        private string GetStateString()
        {
            switch (State)
            {
                case ResultState.Success:
                    return "SUCCESS";
                case ResultState.InvalidParam:
                    return "参数不正确";
                case ResultState.PathNotFound:
                    return "路径不存在";
                case ResultState.AuthorizError:
                    return "文件系统权限不足";
                case ResultState.IOError:
                    return "文件系统读取错误";
            }
            return "未知错误";
        }
    }
    /// <summary>
    /// NotSupportedHandler 的摘要说明
    /// </summary>
    public class NotSupportedHandler : Handler
    {
        public NotSupportedHandler(HttpContext context)
            : base(context)
        {
        }

        public override void Process()
        {
            WriteJson(new
            {
                state = "action 参数为空或者 action 不被支持。"
            });
        }
    }











    /// <summary>
    /// 涂鸦
    /// </summary>
    public class Crawler
    {
        /// <summary>
        /// 
        /// </summary>
        public string SourceUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServerUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }

        private HttpServerUtility Server { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="server"></param>
        public Crawler(string sourceUrl, HttpServerUtility server)
        {
            this.SourceUrl = sourceUrl;
            this.Server = server;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Crawler Fetch()
        {
            if (!IsExternalIPAddress(this.SourceUrl))
            {
                State = "INVALID_URL";
                return this;
            }
            var request = HttpWebRequest.Create(this.SourceUrl) as HttpWebRequest;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    State = "Url returns " + response.StatusCode + ", " + response.StatusDescription;
                    return this;
                }
                if (response.ContentType.IndexOf("image") == -1)
                {
                    State = "Url is not an image";
                    return this;
                }
                ServerUrl = PathFormatter.Format(Path.GetFileName(this.SourceUrl), Config.GetString("catcherPathFormat"));
                var savePath = Server.MapPath(ServerUrl);
                if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                }
                try
                {
                    var stream = response.GetResponseStream();
                    var reader = new BinaryReader(stream);
                    byte[] bytes;
                    using (var ms = new MemoryStream())
                    {
                        byte[] buffer = new byte[4096];
                        int count;
                        while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            ms.Write(buffer, 0, count);
                        }
                        bytes = ms.ToArray();
                    }
                    File.WriteAllBytes(savePath, bytes);
                    State = "SUCCESS";
                }
                catch (Exception e)
                {
                    State = "抓取错误：" + e.Message;
                }
                return this;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsExternalIPAddress(string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntry(uri.DnsSafeHost);
                    foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                    {
                        byte[] ipBytes = ipAddress.GetAddressBytes();
                        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (!IsPrivateIP(ipAddress))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case UriHostNameType.IPv4:
                    return !IsPrivateIP(IPAddress.Parse(uri.DnsSafeHost));
            }
            return false;
        }
        /// <summary>
        /// 是否私有网络
        /// </summary>
        /// <param name="myIPAddress"></param>
        /// <returns></returns>
        private bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 上传配置
    /// </summary>
    public class UploadConfig
    {
        /// <summary>
        /// 文件命名规则
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// 上传表单域名称
        /// </summary>
        public string UploadFieldName { get; set; }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        public int SizeLimit { get; set; }

        /// <summary>
        /// 上传允许的文件格式
        /// </summary>
        public string[] AllowExtensions { get; set; }

        /// <summary>
        /// 文件是否以 Base64 的形式上传
        /// </summary>
        public bool Base64 { get; set; }

        /// <summary>
        /// Base64 字符串所表示的文件名
        /// </summary>
        public string Base64Filename { get; set; }
    }
    /// <summary>
    /// 上传结果
    /// </summary>
    public class UploadResult
    {
        public UploadState State { get; set; }
        public string Url { get; set; }
        public string OriginFileName { get; set; }
        public string ErrorMessage { get; set; }
    }
    /// <summary>
    /// 上传状态
    /// </summary>
    public enum UploadState
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 超过大小限制
        /// </summary>
        SizeLimitExceed = -1,
        /// <summary>
        /// 不允许类型
        /// </summary>
        TypeNotAllow = -2,
        /// <summary>
        /// 文件访问错误
        /// </summary>
        FileAccessError = -3,
        /// <summary>
        /// 网络错误
        /// </summary>
        NetworkError = -4,
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown = 1,
    }







    /// <summary>
    /// 路径业务处理类
    /// </summary>
    public static class PathFormatter
    {
        public static string Format(string originFileName, string pathFormat)
        {
            if (String.IsNullOrWhiteSpace(pathFormat))
            {
                pathFormat = "{filename}{rand:6}";
            }

            var invalidPattern = new Regex(@"[\\\/\:\*\?\042\<\>\|]");
            originFileName = invalidPattern.Replace(originFileName, "");

            string extension = Path.GetExtension(originFileName);
            string filename = Path.GetFileNameWithoutExtension(originFileName);

            pathFormat = pathFormat.Replace("{filename}", filename);
            pathFormat = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(pathFormat, new MatchEvaluator(delegate (Match match)
            {
                var digit = 6;
                if (match.Groups.Count > 2)
                {
                    digit = Convert.ToInt32(match.Groups[2].Value);
                }
                var rand = new Random();
                return rand.Next((int)Math.Pow(10, digit), (int)Math.Pow(10, digit + 1)).ToString();
            }));

            pathFormat = pathFormat.Replace("{time}", DateTime.Now.Ticks.ToString());
            pathFormat = pathFormat.Replace("{yyyy}", DateTime.Now.Year.ToString());
            pathFormat = pathFormat.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            pathFormat = pathFormat.Replace("{ii}", DateTime.Now.Minute.ToString("D2"));
            pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));

            return pathFormat + extension;
        }
    }

}