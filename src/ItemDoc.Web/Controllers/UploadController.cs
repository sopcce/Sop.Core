using Common.Logging;
using ItemDoc.Core.API;
using ItemDoc.Core.Extensions;
using ItemDoc.Core.Utilities;
using ItemDoc.Core.Web;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Parameter;
using ItemDoc.Services.Servers;
using Microsoft.Win32;
using Sop.Common.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace ItemDoc.Web.Controllers
{
    public class UploadController : BaseController
    {
        private static readonly ILog Logger = LogManager.GetLogger<UploadController>();
        public AttachmentService _attachmentService { get; set; }
        public FileServerService _fileServerService { get; set; }


        public ActionResult Test()
        {

            return View();
        }


        /// <summary>
        /// asd
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var homePath = strPath();
            homePath = homePath.Substring(0, homePath.LastIndexOf('\\'));
            DirectoryInfo directoryInfo = new DirectoryInfo(homePath);
            if (!directoryInfo.Exists)
                return View(); ;
            string theimg = "";
            foreach (var fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
            {


                string fullName = fileInfo.FullName;
                string key = fullName.Substring(fullName.IndexOf("SopUpLoad"), fullName.Length - 49);



                theimg += "<img src='" + key + "'/>";

            }
            ViewData["msg"] = theimg;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var file1 = Request.Files[0];
            //文件大小不为0
            HttpPostedFileBase Filedata = Request.Files[0];
            // 如果没有上传文件
            if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength <= 0)
            {
                return HttpNotFound();
            }
            string filename = Guid.NewGuid() + System.IO.Path.GetExtension(Filedata.FileName);

            Filedata.SaveAs(strPath() + filename);

            return View();


        }



        #region 图片上传
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Files()
        {
            //_attachmentService.GetAll()
            //暂时使用数据库方式，后期优化方案
            /*1、上传文件，文件存储base64返回前端。记录附件信息，存储到缓存队列中，等待上传。
             *2、文件上传图片服务器，要求：服务器在Global.asax配置，直接读取缓存或者本地存储配置文件。
             *3、文件上传图片服务器，要求：考虑到服务器之间传输信息效率问题，采用读取缓存队列，上传文件到文件服务器，要求容错，
             *   3次失败之后记录错误队列，错误队列信息等存储到服务器等待定时任务重试。
             */

            try
            {
                var file1 = Request.Files[0];
                //文件大小不为0

                ////接收用户传递的文件数据.
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null)
                    {
                        var ownerid = Request.Form["ownerId"] ?? Guid.NewGuid().ToString("N");
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        var result = await SaveUploadFile(ownerid, file);

                        sw.Stop();
                        var elapsedMilliseconds = sw.ElapsedMilliseconds;

                        var cc = result.FromJson<DataPackage>();
                        var paths = Framework.Utility.WebUtility.GetRootPath();


                        var Data = cc?.Data?.ToJson()?.FromJson<DataInfo>();
                        if (string.IsNullOrWhiteSpace(cc?.Data?.ToJson()))
                        {
                            return JsonErrorResult(null, "操作失败");
                        }
                        return JsonSuccessResult(Data, "操作成功" + elapsedMilliseconds.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);

            }
            return JsonErrorResult(null, "操作失败");
        }

        #endregion


        #region 异步保存图片

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(string ownerId, HttpPostedFileBase file)
        {

            string fileName = Path.GetFileName(file.FileName);//获取文件名.
            string fileExt = Path.GetExtension(fileName);//获取文件扩展名.

            ImgServerParameter imgServer = new ImgServerParameter();
            var list = _fileServerService.GetAll().Where(c => c.Enabled == true).ToList();

            var i = new Random().Next(list.Count());

            imgServer.ServerUrl = list[i].ServerUrl;
            imgServer.ServerId = list[i].ServerId;
            imgServer.ServerName = list[i].ServerName;
            imgServer.ServerEnName = list[i].ServerEnName;
            imgServer.OwnerId = ownerId;
            imgServer.RootPath = list[i].RootPath;
            imgServer.FileName = fileName;
            imgServer.FileExtension = Path.GetExtension(fileName);
            imgServer.ContentType = file.ContentType;
            imgServer.ContentLength = file.ContentLength;
            imgServer.Key = Guid.NewGuid().ToString("N");
            imgServer.Date = DateTime.Now.ToTimestamp();
            imgServer.IP = Framework.Utility.WebUtility.GetIp();
            imgServer.VirtualPath = list[i].VirtualPath;
            imgServer.Token = EncryptionUtility.Sha512Encode(imgServer.Key + imgServer.ServerUrl + imgServer.Date);

            ////加入文件队列，等待上传文件,返回文件base64及地址信息，。
            //var redisService = DiContainer.Resolve<ICacheManager>();
            ////string url = redisService.Get<ImgServerParameter>(cacheKey);
            //redisService.Set(ownerId, list[i], TimeSpan.FromDays(30));

            var d = ConvertUtility.StreamToBytes(file.InputStream);
            var ba = Convert.ToBase64String(d);


            string urlData = HttpUtility.UrlEncode(EncryptionUtility.AES_Encrypt(imgServer.ToJson(), imgServer.Token));
            string imageServerUrl = $"{imgServer.ServerUrl}?token={HttpUtility.UrlEncode(imgServer.Token)}&data={urlData}&_={imgServer.Date}";

            string result = string.Empty;
            try
            {
                //HttpClient client = new HttpClient();
                //var content = new MultipartFormDataContent();
                //string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                //content.Add(new StreamContent(file.InputStream), "file", fileName);
                //content.Add(new StringContent(server.ServerID.ToString()), "serverID");
                //var html = client.PostAsync(address, content).Result;
                //context.Response.Write(html.Content.ReadAsStringAsync().Result);

                using (WebClientEx client = new WebClientEx())
                {
                    client.Proxy = null;
                    client.Timeout = 1000 * 60 * 10; //10分钟
                    client.Encoding = Encoding.UTF8;
                    var responseArray = await client.UploadDataTaskAsync(imageServerUrl, ConvertUtility.StreamToBytes(file.InputStream));//向图片服务器发送文件数据.
                    result = Encoding.GetEncoding("UTF-8").GetString(responseArray);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(imageServerUrl);

            }
            return await Task.FromResult<string>(result);
        }

        public async Task<string> SaveUploadFile(string ownerId, HttpPostedFileBase file)
        {

            string fileName = Path.GetFileName(file.FileName);//获取文件名.
            string fileExt = Path.GetExtension(fileName);//获取文件扩展名.
            //
            string upPath = "~/Uploads/LocalFile/{yyyy}/{MM}/{dd}/";
            string newFileName = Guid.NewGuid().ToString("N") + fileExt?.ToLower();
            upPath = CommonUtility.Format(newFileName, upPath);
            string diskPath = FileUtility.GetDiskFilePath(upPath) + newFileName;
            string serverUrlPath = upPath.Replace("~", "") + newFileName;



            string result = string.Empty;
            try
            {
                file.SaveAs(diskPath);
                result = new DataPackage
                {
                    Code = HttpStatusCode.OK,
                    Data = new DataInfo()
                    {
                        path = serverUrlPath,
                        ServerPath = Request.RawUrl + serverUrlPath
                    },
                    Msg = "保存成功"
                }.ToJson();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(serverUrlPath);

            }
            return await Task.FromResult<string>(result);
        }



        #endregion


        public class DataInfo
        {
            public string path { get; set; }

            public string ServerPath { get; set; }
        }

        public void ddd()
        {

            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

            using (var client = new HttpClient())//httpclient的post方式, 需要被实体接收..
            {
                string apiurl = "http://192.168.1.225:9090/api/V3ImageUploadApi/posttestform2";
                //3个枚举, stringContent,ByteArrayContent,和FormUrlContent, 一般的post用StringContent就可以了
                using (var content = new StringContent(
                    "Email=321a&Name=kkfew", Encoding.UTF8, "application/x-www-form-urlencoded"))
                {
                    content.Headers.Add("aa", "11"); //默认会使用
                    var result = client.PostAsync(apiurl, content).Result;
                    Console.WriteLine(result);
                }
            }
            using (var client = new HttpClient())
            {
                string apiurl = "http://192.168.1.225:9090/api/V3ImageUploadApi/posttestform";
                using (var content = new StringContent(json.Serialize(new { Email = "1", Name = "2" })))
                {
                    content.Headers.Add("aa", "11");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result = client.PostAsync(apiurl, content).Result;
                    Console.WriteLine(result);
                }
            }
            using (WebClient webclient = new WebClient())
            {

                string apiurl = "http://192.168.1.225:9090/api/V3ImageUploadApi/posttestform2";

                webclient.Encoding = UTF8Encoding.UTF8;
                webclient.Headers.Add("Custom-Auth-Key", "11");
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//x-www-form-urlencoded
                                                                                           //如果webapi的接收参数对象是dynamic, 则请求的头是json, 如果是用实体接收, 那么则是上面这个


                var re = webclient.UploadData(apiurl, Encoding.UTF8.GetBytes("Email=321a&Name=kkfew"));

                Console.Write(System.Text.Encoding.UTF8.GetString(re));
            }

            using (WebClient webclient = new WebClient())
            {

                string apiurl = "http://192.168.1.225:9090/api/V3ImageUploadApi/posttestform";

                webclient.Encoding = UTF8Encoding.UTF8;
                webclient.Headers.Add("Custom-Auth-Key", "11");
                webclient.Headers.Add("Content-Type", "application/json");//x-www-form-urlencoded
                                                                          //如果webapi的接收参数对象是dynamic, 则请求的头是json, 如果是用实体接收, 那么则是上面这个


                var re = webclient.UploadData(apiurl,
                    System.Text.Encoding.UTF8.GetBytes(json.Serialize(new { email = "123456@qq.com", password = "111111" })));

                Console.Write(System.Text.Encoding.UTF8.GetString(re));
            }
        }

























        /// <summary>
        /// 
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase filedata)
        {

            HttpFileCollection files1 = System.Web.HttpContext.Current.Request.Files;
            //如果目录不存在，则创建目录
            if (files1 != null)
            {

                for (int i = 0; i < files1.Count; i++)
                {
                    files1[i].SaveAs(strPath() + "//" + Guid.NewGuid() + "-1-" + files1[i].FileName);
                }
            }


            if (filedata != null)
            {
                var s1 = filedata.ContentLength;
                var s2 = filedata.ContentType;
                var s3 = filedata.FileName;
                string ext = Path.GetExtension(s3).ToLower();
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                {
                    string mimeType = regKey.GetValue("Content Type").ToString();
                }
                var s4 = filedata.InputStream;
                var s5 = s4.CanRead;
                string filename = Guid.NewGuid() + "-2-" + Path.GetExtension(filedata.FileName);

                filedata.SaveAs(strPath() + "//" + filename);
            }
            HttpPostedFileBase file = Request.Files[0];
            if (file != null)
            {
                file.SaveAs(strPath() + "//" + Guid.NewGuid() + "-3-" + Path.GetExtension(file.FileName));
            }
            return Json(new { success = 1, url = "123", message = "sss" });
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string strPath(string path = "")
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "~/Uploads/" + DateTime.Now.ToString("yyyy/MM/dd/");


            }
            string basePath = Server.MapPath(path);
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            return basePath;
        }
    }





}
