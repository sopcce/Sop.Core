using ItemDoc.Core.API;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Model;
using ItemDoc.Services.Parameter;
using ItemDoc.Services.Servers;
using Sop.Common.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ItemDoc.Core.Mvc.SystemMessage;
using ItemDoc.Core.Utilities;

namespace ItemDoc.Upload.Controllers
{
    /// <summary>
    /// 1、数据是以文件的形式存在，提供 Open、Read、Write、Seek、Close 等API 进行访问；
    /// 2、文件以树形目录进行组织，提供原子的重命名（Rename）操作改变文件或者目录的位置。 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    public class V1Controller : ApiController
    {

        /// <summary>
        /// /
        /// </summary>
        public AttachmentService _attachmentService { get; set; }


        [HttpGet]
        public HttpResponseMessage Test()
        {
            return SystemMessage.Result("Test is SuccessResult");
        }
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            //string imageServerUrl = $"{imgServer.ServerUrl}/home/upload?token={HttpUtility.UrlEncode(imgServer.Token)}&data={urlData}&_={imgServer.Date}";
            var context = System.Web.HttpContext.Current;
            //context.Response.ContentType = "text/plain";

            string token = context.Request["token"];
            //token = HttpUtility.UrlDecode(token);
            string data = context.Request["data"];
            //data = HttpUtility.UrlDecode(data);
            string date = context.Request["_"];
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(date))
            {
                return SystemMessage.Result("token is err ");
            }
            AttachmentInfo info = new AttachmentInfo();
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ImgServerParameter imgServer = new ImgServerParameter();
                data = EncryptionUtility.AES_Decrypt(data, token);
                imgServer = data.FromJson<ImgServerParameter>();
                string newToken = EncryptionUtility.Sha512Encode(imgServer.Key + imgServer.ServerUrl + imgServer.Date);
                if (token != imgServer.Token || token != newToken)
                {
                    //return Json(new { code = 002, status = "err" }, JsonRequestBehavior.AllowGet);
                    return SystemMessage.Result("err", HttpStatusCode.BadRequest);
                }
                string upPath = imgServer.VirtualPath;
                string newFileName = Guid.NewGuid().ToString("N") + imgServer.FileExtension.ToLower();
                upPath = CommonUtility.Format(newFileName, upPath);
                string diskPath = FileUtility.GetDiskFilePath(upPath) + newFileName;
                string serverUrlPath = imgServer.RootPath + upPath.Replace("~", "") + newFileName;
                using (FileStream fileStream = System.IO.File.OpenWrite(diskPath))
                {
                    context.Request.InputStream.CopyTo(fileStream);
                }
                //存储数据库

                info.ServerId = imgServer.ServerId;
                info.AttachmentId = Guid.NewGuid().ToString("N");
                info.OwnerId = imgServer.OwnerId;
                info.ServerUrlPath = serverUrlPath;
                info.Status = AttachmentStatus.Fail;
                info.FileNames = newFileName;
                info.Extension = imgServer.FileExtension;
                info.Size = imgServer.ContentLength;
                info.MimeType = imgServer.ContentType;
                info.UploadFileName = imgServer.FileName;
                info.DateCreated = DateTime.Now;
                info.Ip = imgServer.IP;
                info.Id = _attachmentService.Create(info);

                sw.Stop();
                return SystemMessage.Result(new { Path = info.ServerUrlPath });
            }
            catch (Exception e)
            {

                return SystemMessage.Result(new { Path = info.ServerUrlPath, Message = e.Message }, HttpStatusCode.BadRequest);
            }

        }












       


    }
}
