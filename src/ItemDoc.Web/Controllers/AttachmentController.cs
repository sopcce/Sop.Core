using Microsoft.Win32;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ItemDoc.Web.Controllers
{
  public class AttachmentController : Controller
  {


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
      //文件大小不为0
      HttpPostedFileBase Filedata = Request.Files[0];
      // 如果没有上传文件
      if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength <= 0)
      {
        return HttpNotFound();
      }
      string filename = Guid.NewGuid() + System.IO.Path.GetExtension(Filedata.FileName);

      string path = "~/Uploads/" + DateTime.Now.ToString("yyyy/MM/dd/");

      Filedata.SaveAs(strPath() + filename);

      return Json(new { success = 1, url = path + filename, message = "sss" });


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
