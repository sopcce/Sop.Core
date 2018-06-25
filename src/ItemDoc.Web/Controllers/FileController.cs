using ItemDoc.Core.Extensions;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Parameter;
using ItemDoc.Services.Servers;
using ItemDoc.Web.Controllers.Base;
using Microsoft.Win32;
using Sop.Common.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ItemDoc.Web.Controllers
{
  public class FileController : BaseController
  {
    private IAttachmentService _attachmentService;
    private IFileServerService _fileServerService;

    public FileController(IAttachmentService attachmentService,
      IFileServerService fileServerService)
    {
      _attachmentService = attachmentService;
      _fileServerService = fileServerService;
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
    [HttpPost]
    public ActionResult UP(FormCollection form)
    {
      var file1 = Request.Files[0];
      var vlaue = Request.Form["ownerid"];

      //文件大小不为0
      HttpPostedFileBase Filedata = Request.Files[0];
      // 如果没有上传文件
      if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength <= 0)
      {
        return HttpNotFound();
      }
      string filename = Guid.NewGuid() + System.IO.Path.GetExtension(Filedata.FileName);

      string path = "/Uploads/" + DateTime.Now.ToString("yyyy/MM/dd/");

      Filedata.SaveAs(strPath() + filename);

      return Json(new { success = 1, url = path + filename, message = "sss", ownerId = vlaue });


    }


    #region 图片上传
    [HttpPost]
    public ActionResult UP1()
    {
      //_attachmentService.GetAll()

      Stopwatch sw = new Stopwatch();

      HttpPostedFileBase file = Request.Files[0];//接收用户传递的文件数据.
      if (file != null)
      {
        string fileName = Path.GetFileName(file.FileName);//获取文件名.
        string fileExt = Path.GetExtension(fileName);//获取文件扩展名.
        ImgServerParameter imgServer = new ImgServerParameter();


        var list = _fileServerService.GetAll().Where(c => c.Enabled == true).ToList();

        int imageServerCount = list.Count();//获取可用的图片服务器数量.
        Random random = new Random();
        int i = random.Next(imageServerCount);

        imgServer.ServerUrl = list[i].ServerUrl;
        imgServer.ServerId = list[i].ServerId;
        imgServer.FileName = file.FileName;
        imgServer.FileExtension = Path.GetExtension(fileName);
        imgServer.ContentType = file.ContentType;
        imgServer.ContentLength = file.ContentLength;
        imgServer.Key = Guid.NewGuid().ToString("N");
        imgServer.Date = DateTime.Now.ToTimestamp();
        imgServer.IP = Framework.Utility.WebUtility.GetIp();

        imgServer.Token = EncryptionUtility.Sha512Encode(imgServer.Key + imgServer.ServerUrl + imgServer.Date);


        string urlData = HttpUtility.UrlEncode(EncryptionUtility.AES_Encrypt(imgServer.ToJson(), imgServer.Token));
        string imageServerUrl = $"{imgServer.ServerUrl}/home/upload?token={HttpUtility.UrlEncode(imgServer.Token)}&data={urlData}&_={imgServer.Date}";




        sw.Start();

        WebClient client = new WebClient();
        var responseArray = client.UploadData(imageServerUrl, StreamToBytes(file.InputStream));//向图片服务器发送文件数据.

        //ItemDoc.Core.Web.HttpWebHelper httpWeb = new Core.Web.HttpWebHelper();
        //var responseArray1 = httpWeb.PostFile(imageServerUrl, new List<Stream>() { file.InputStream });

        string data = Encoding.GetEncoding("UTF-8").GetString(responseArray);
        sw.Stop();
        return Json(new { code = 200, oldData = imageServerUrl, data = data });
      }
      return Json(new { code = 404 });
    }
    private byte[] StreamToBytes(Stream stream)
    {
      byte[] buffer = new byte[stream.Length];
      stream.Read(buffer, 0, buffer.Length);
      stream.Seek(0, SeekOrigin.Begin);
      return buffer;
    }

    #endregion





























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
