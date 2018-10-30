using ItemDoc.Core.Extensions;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Parameter;
using ItemDoc.Services.Servers;
using ItemDoc.Web.Controllers.Base;
using Microsoft.Win32;
using Sop.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ItemDoc.Core.API;

namespace ItemDoc.Web.Controllers
{
    public class UploadController : BaseController
    {
        public AttachmentService _attachmentService { get; set; }
        public FileServerService _fileServerService { get; set; }



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
                HttpPostedFileBase file = Request.Files[0];//接收用户传递的文件数据.
                if (file != null)
                {
                    var ownerid = Request.Form["ownerId"] ?? Guid.NewGuid().ToString("N");
                    string fileName = Path.GetFileName(file.FileName);//获取文件名.
                    string fileExt = Path.GetExtension(fileName);//获取文件扩展名.
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    var result = await UploadFile(ownerid, fileName, file.ContentType, file.ContentLength, StreamToBytes(file.InputStream));
                    sw.Stop();
                    var ElapsedMilliseconds = sw.ElapsedMilliseconds;
                    
                    var cc = result.FromJson<DataPackage>();
                    var path = cc.data?.ToJson()?.FromJson<DataInfo>()?.path;
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        return JsonErrorResult(null, "操作失败");
                    } 
                    return JsonSuccessResult(new DataInfo { path= path }, "操作成功");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return JsonErrorResult(null, "操作失败");
        }

        #endregion


        //{"code":200,"data":{"data":"","Path":" http://localhost:8014/Upload3//Uploads/3File/2018/10/30/138f076b24eb442bb8ccbabcab7f1bd2.jpg","status":"ok","Message":""},"paging":null,"description":""}

        private byte[] StreamToBytes(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return buffer;
        }
        public class DataInfo
        {
            public string path { get; set; }
        }

        public async Task<string> UploadFile(string ownerId, string fileName, string contentType, int contentLength, byte[] data)
        {
            ImgServerParameter imgServer = new ImgServerParameter();
            var list = _fileServerService.GetAll().Where(c => c.Enabled == true).ToList();
            int imageServerCount = list.Count();//获取可用的图片服务器数量.
            Random random = new Random();
            int i = random.Next(imageServerCount);
            imgServer.ServerUrl = list[i].ServerUrl;
            imgServer.ServerId = list[i].ServerId;
            imgServer.ServerName = list[i].ServerName;
            imgServer.ServerEnName = list[i].ServerEnName;
            imgServer.OwnerId = ownerId;
            imgServer.RootPath = list[i].RootPath;
            imgServer.FileName = fileName;
            imgServer.FileExtension = Path.GetExtension(fileName);
            imgServer.ContentType = contentType;
            imgServer.ContentLength = contentLength;
            imgServer.Key = Guid.NewGuid().ToString("N");
            imgServer.Date = DateTime.Now.ToTimestamp();
            imgServer.IP = Framework.Utility.WebUtility.GetIp();
            imgServer.VirtualPath = list[i].VirtualPath;
            imgServer.Token = EncryptionUtility.Sha512Encode(imgServer.Key + imgServer.ServerUrl + imgServer.Date);
            string urlData = HttpUtility.UrlEncode(EncryptionUtility.AES_Encrypt(imgServer.ToJson(), imgServer.Token));
            string imageServerUrl = $"{imgServer.ServerUrl}?token={HttpUtility.UrlEncode(imgServer.Token)}&data={urlData}&_={imgServer.Date}";

            string result = string.Empty;
            try
            {
                WebClient client = new WebClient();
                var responseArray = client.UploadData(imageServerUrl, data);//向图片服务器发送文件数据.
                result = Encoding.GetEncoding("UTF-8").GetString(responseArray);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //try
            //{
            //    ItemDoc.Core.Web.HttpWebHelper httpWeb = new Core.Web.HttpWebHelper();
            //    data = httpWeb.PostFile(imageServerUrl, new List<Stream>() { file.InputStream });

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}


            return await Task.FromResult<string>(result);
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
