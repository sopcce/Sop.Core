using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ItemDoc.Upload.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Title = "Home Page";

      return View();
    }

    public JsonResult Upload()
    {


      var context = System.Web.HttpContext.Current;

      HttpFileCollection files = context.Request.Files;
      //var asd  = Request.Content.ReadAsStringAsync<string>("ext");
      context.Response.ContentType = "text/plain";

      string fileExt = context.Request["ext"];//接收文件的扩展名
      System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
      sw.Start();
      if (!string.IsNullOrEmpty(fileExt))
      {

        int serverId = Convert.ToInt32(context.Request["serverId"]);//图片所在的图片服务器的编号。
        string newFileName = Guid.NewGuid().ToString();//使用GUID作为文件的新名称，当然也可以使用其他的方式对文件进行重命名。
        string fullDir = strPath(serverId) + newFileName + fileExt;//完整的图片存储的路径
        using (FileStream fileStream = System.IO.File.OpenWrite(fullDir))
        {
          context.Request.InputStream.CopyTo(fileStream);//将文件数据写到磁盘上。


          //MyImageServerEntities db = new MyImageServerEntities();
          //ImageInfo imageInfo = new ImageInfo();
          //imageInfo.ImageName = fullDir;//存储图片路径
          //imageInfo.ImageServerId = serverId;
          //db.ImageInfo.Add(imageInfo);//将数据添加到上下文中，并且打上了添加标记。
          //db.SaveChanges();//保存数据.
        }

        sw.Stop();
      }

      return Json(new { data = "", time = sw.Elapsed, status = "ok" }, JsonRequestBehavior.AllowGet);
    }

    private string strPath(object str)
    {

      string basePath = Server.MapPath("~/Uploads/" + str + "File/" + DateTime.Now.ToString("yyyy/MM/dd/"));
      if (!Directory.Exists(basePath))
        Directory.CreateDirectory(basePath);
      return basePath;
    }
  }
}
