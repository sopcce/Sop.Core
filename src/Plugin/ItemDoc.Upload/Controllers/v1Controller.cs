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
            return  SystemMessage.Result("Test is SuccessResult"); 
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
                    return SystemMessage.Result("err",HttpStatusCode.BadRequest);
                }
                string upPath = imgServer.VirtualPath;
                string newFileName = Guid.NewGuid().ToString("N") + imgServer.FileExtension.ToLower();
                upPath = Format(newFileName, upPath);
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
             
                return SystemMessage.Result(new {  Path = info.ServerUrlPath, Message = e.Message },HttpStatusCode.BadRequest);
            }

        }











        private string strPath(object str)
        {
            //switch (state)
            //{
            //  case UploadState.Success:
            //    return "SUCCESS";
            //  case UploadState.FileAccessError:
            //    return "文件访问出错，请检查写入权限";
            //  case UploadState.SizeLimitExceed:
            //    return "文件大小超出服务器限制";
            //  case UploadState.TypeNotAllow:
            //    return "不允许的文件格式";
            //  case UploadState.NetworkError:
            //    return "网络错误";
            //}
            return "未知错误";
            //string basePath = Server.MapPath("~/Uploads/" + str + "File/" + DateTime.Now.ToString("yyyy/MM/dd/"));
            //if (!Directory.Exists(basePath))
            //  Directory.CreateDirectory(basePath);
            //return basePath;
        }
        /// <summary>
        ///  /*"upload/image/{yyyy}{mm}{dd}/{time}{rand:6}", /* 上传保存路径,可以自定义保存路径和文件名格式 */
        ///  /* {rand:6} 会替换成随机数,后面的数字是随机数的位数 */
        ///  /* {time} 会替换成时间戳 */
        ///  /* {yyyy} 会替换成四位年份 */
        ///  /* {yy} 会替换成两位年份 */
        ///  /* {MM} 会替换成两位月份 */
        ///  /* {dd} 会替换成两位日期 */
        ///  /* {hh} 会替换成两位小时 */
        ///  /* {mm} 会替换成两位分钟 */
        ///  /* {ss} 会替换成两位秒 */
        ///  /* {ffff} 会替换成两位秒 */
        ///  /* 非法字符 \ : * ? " < > | */
        /// </summary>
        /// <param name="originFileName"></param>
        /// <param name="pathFormat"></param>
        /// <returns></returns>
        private string Format(string originFileName, string pathFormat)
        {
            if (String.IsNullOrWhiteSpace(pathFormat))
            {
                pathFormat = "{rand:6}";
            }
            pathFormat = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(pathFormat,
              new MatchEvaluator(delegate (Match match)
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
            pathFormat = pathFormat.Replace("{MM}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Minute.ToString("D2"));
            pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));
            pathFormat = pathFormat.Replace("{ffff}", DateTime.Now.Millisecond.ToString("D2"));
            return pathFormat;
        }
        #region private Result

   

        #endregion private Result

    }
}
 