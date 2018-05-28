using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ItemDoc.Core.Auth;
using ItemDoc.Services.Model;
using ItemDoc.Services.Servers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ItemDoc.Web.Controllers
{
  public class HomeController : Controller
  {
    private IAuthenticationService _authentication;
    private IUsersService _usersService;

    public HomeController(
      IUsersService usersService,
      IAuthenticationService authentication)
    {
      _authentication = authentication;
      _usersService = usersService;
    }

    public ActionResult Index()
    {

      return View();
    }



   
 public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }
    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();



    }


  public ActionResult Question()
    {
      ViewBag.Message = "Your contact page.";

      return View();



    }


















    #region 图片上传
    public ActionResult FileUpload()
    {
      Stopwatch sw = new Stopwatch();
      sw.Start();
      HttpPostedFileBase file = Request.Files["fileUp"];//接收用户传递的文件数据.
      string fileName = Path.GetFileName(file.FileName);//获取文件名.
      string fileExt = Path.GetExtension(fileName);//获取文件扩展名.

      var list = new List<ImageServerInfo>
      {
        new ImageServerInfo() { ServerId = 1, ServerName = "Upload1", ServerUrl = "http://localhost:8014/Upload1" },
        new ImageServerInfo() { ServerId = 2, ServerName = "Upload2", ServerUrl = "http://localhost:8014/Upload2" },
        new ImageServerInfo() { ServerId = 3, ServerName = "Upload3", ServerUrl = "http://localhost:8014/Upload3" }
      };

      //db.ImageServerInfo.Where<ImageServerInfo>(c => c.FlgUsable == true).ToList();
      //将可用的图片服务器信息查询出来.
      //从状态表筛选出可用的图片服务器集合记作C,并获取集合的总记录数N。然后用随机函数产生一个随机数R1并用R1与N进行取余运算记作I=R1%N。则C[I]即为要保存图片的图片服务器
      int imageServerCount = list.Count();//获取可用的图片服务器数量.
      Random random = new Random();
      int r = random.Next();
      int i = r % imageServerCount;
      string serverUrl = list[i].ServerUrl;//获取图片服务器的地址。
      int serverId = list[i].ServerId;//获取图片服务器编号.
                                      //string imageServerUrl = "http://" + serverUrl + "/FileUp.ashx?ext=" + fileExt + "&serverId=" + serverId;//构建图片服务器地址.
                                      //http://localhost:8014/Upload1
      string imageServerUrl = string.Format("{0}/home/upload?ext={1}&serverId={2}", serverUrl, fileExt, serverId);//构建图片服务器地址.
      sw.Stop();
      string time1 = sw.ElapsedMilliseconds.ToString();
      sw.Restart();


      WebClient client = new WebClient();
      var responseArray = client.UploadData(imageServerUrl, StreamToBytes(file.InputStream));//向图片服务器发送文件数据.

      ItemDoc.Core.Web.HttpWebHelper httpWeb = new Core.Web.HttpWebHelper();
      var responseArray1 = httpWeb.PostFile(imageServerUrl, new List<Stream>() { file.InputStream });



      string getPath = Encoding.GetEncoding("UTF-8").GetString(responseArray);
      sw.Stop();
      string msg = string.Format("运行时间：{0}，上次时间：{1}， 返回参数{2}", time1, sw.Elapsed.ToString(), getPath);
      return Content("文件上传成功:" + msg);
    }
    #endregion

    #region 展示图片
    public ActionResult ShowImage()
    {
      //var imageList = db.ImageServerInfo.Where<ImageServerInfo>(c => c.FlgUsable == true).ToList();
      //ViewData["imageList"] = imageList;
      return View();
    }
    #endregion
    /// <summary>
    /// 将流转成字节数组
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private byte[] StreamToBytes(Stream stream)
    {
      byte[] buffer = new byte[stream.Length];
      stream.Read(buffer, 0, buffer.Length);
      stream.Seek(0, SeekOrigin.Begin);
      return buffer;
    }
  }
}