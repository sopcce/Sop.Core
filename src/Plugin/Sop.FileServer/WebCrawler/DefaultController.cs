//using Common.Logging;
//using Sop.Core.Mvc.SystemMessage;
//using Sop.Services.Model;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using Sop.Core.Utilities;
//using Sop.Framework.Utility;
//using Sop.Services.Parameter;
//using Sop.Common.Serialization;
//using Sop.Services.Servers;
//using System.Net.Http;

//namespace Sop.File.Controllers
//{
//    public class DefaultController : Controller
//    {
//        private static readonly ILog Logger = LogManager.GetLogger<DefaultController>();
//        // GET: Default
//        public ActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public JsonResult Upload()
//        {
//            if (Request.InputStream == null)
//            {
//                return Json(new { status = 0, msg = "没有图片" });
//            }
//            try
//            {
//                Bitmap bitmap = new Bitmap(Request.InputStream);

//                try
//                {
//                    if (bitmap != null)
//                    {

//                        var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
//                        var filePath = "/image/" + Folder + "/" + DateTime.Now.ToString("yyyyMM") + "/";
//                        var path = System.Web.HttpContext.Current.Server.MapPath("~" + filePath);
//                        if (!System.IO.Directory.Exists(path))
//                        {
//                            System.IO.Directory.CreateDirectory(path);//不存在则创建文件夹 
//                        }
//                        var savePath = Path.Combine(path, imageName);
//                        bitmap.Save(savePath, ImageFormat.Jpeg);

//                        return Json(new { status = 1, msg = "图片上传成功", data = filePath + imageName });
//                    }
//                    else
//                    {
                        
//                    }
//                }
//                catch (Exception ex)
//                {
                    
//                    return Json(new { status = 0, msg = "图片上传失败" });
//                }
//                finally
//                {
//                    bitmap.Dispose();
//                }
//            }
//            catch (Exception ex)
//            {
                
//                return Json(new { status = 0, msg = "图片上传失败" });
//            }


//            return Json(new { status = 0, msg = "图片上传失败" });
//        }
//        [HttpPost]
//        public JsonResult UploadFlie()
//        {
           
//            var context = System.Web.HttpContext.Current; 
//            string token = context.Request["token"]; 
//            string data = context.Request["data"]; 
//            string date = context.Request["_"];
//            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(date))
//            {
//                return SystemMessage.Result("token is err ");
//            }
//            AttachmentInfo info = new AttachmentInfo();
//            try
//            {
//                Stopwatch sw = new Stopwatch();
//                sw.Start();
//                ImgServerParameter imgServer = new ImgServerParameter();
//                data = EncryptionUtility.AES_Decrypt(data, token);
//                imgServer = data.FromJson<ImgServerParameter>();
//                string newToken = EncryptionUtility.Sha512Encode(imgServer.Key + imgServer.ServerUrl + imgServer.Date);
//                if (token != imgServer.Token || token != newToken)
//                {
//                    //return Json(new { code = 002, status = "err" }, JsonRequestBehavior.AllowGet);
//                    return SystemMessage.Result("err", HttpStatusCode.BadRequest);
//                }
//                string upPath = imgServer.VirtualPath;
//                string newFileName = Guid.NewGuid().ToString("N") + imgServer.FileExtension.ToLower();
//                upPath = CommonUtility.Format(newFileName, upPath);
//                string diskPath = FileUtility.GetDiskFilePath(upPath) + newFileName;
//                string serverUrlPath = imgServer.RootPath + upPath.Replace("~", "") + newFileName;
//                using (FileStream fileStream = System.IO.File.OpenWrite(diskPath))
//                {
//                    context.Request.InputStream.CopyTo(fileStream);
//                }
//                //存储数据库

//                info.ServerId = imgServer.ServerId;
//                info.AttachmentId = Guid.NewGuid().ToString("N");
//                info.OwnerId = imgServer.OwnerId;
//                info.ServerUrlPath = serverUrlPath;
//                info.Status = AttachmentStatus.Fail;
//                info.FileNames = newFileName;
//                info.Extension = imgServer.FileExtension;
//                info.Size = imgServer.ContentLength;
//                info.MimeType = imgServer.ContentType;
//                info.UploadFileName = imgServer.FileName;
//                info.DateCreated = DateTime.Now;
//                info.Ip = imgServer.IP;
//                info.Id = AttachmentService.Create(info);

//                sw.Stop();
//                return SystemMessage.Result(new { Path = info.ServerUrlPath });
//            }
//            catch (Exception e)
//            {

//                return SystemMessage.Result(new { Path = info.ServerUrlPath, Message = e.Message }, HttpStatusCode.BadRequest);
//            }

//        }

//    }
//}