using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItemDoc.Framework.Utilities;
using ItemDoc.Framework.Utility;
using Sop.Common.Img;

namespace ItemDoc.Web.Controllers
{
  public class ToolController : Controller
  {
    // GET: Tool
    public ActionResult Index()
    {
      return View();
    }
    public ActionResult Enc()
    {
      return View();
    }

    public ActionResult CodeFormat()
    {
      return View();
    }


    public ActionResult Img()
    {

      string text = FileUtility.ReadTextFile("E:\\Item\\ItemDoc\\src\\ItemDoc.Web\\assets\\editor.md\\plugins\\emoji-dialog\\emoji_1.json", 0);

      var list = Sop.Common.Serialization.JsonUtility.FromJson<Root>(text);
      //foreach (string t in list.twemoji)
      //{
      //  new DownImg().GetRemoteImg("https://twemoji.maxcdn.com/36x36/" + t.ToString() + ".png", "/html/twemoji/");
      //}
      string value = "";
      int count = 0;
      foreach (Github_emojiItem t in list.github_emoji)
      {
        value += t.category + ":" + t.list.Count + "-";
        count += t.list.Count;
        foreach (string item in t.list)
        {
          //new DownImg().GetRemoteImg("https://assets-cdn.github.com/images/icons/emoji/" + item.ToString() + ".png", "/html/github_emoji/1/");
          //new DownImg().GetRemoteImg("https://assets-cdn.github.com/images/icons/emoji/" + item.ToString() + ".png", "/html/github_emoji/" + t.category + "/");
        }
      }


      //foreach (string t in list.twemoji)
      //{
      //  new DownImg().GetRemoteImg("https://assets-cdn.github.com/images/icons/emoji/" + t.ToString() + ".png", "/html/twemoji/");
      //}

      //path: "https://assets-cdn.github.com/images/icons/emoji/",        
      //


      string asd = new DownImg().GetRemoteImg("http://img1.3lian.com/2015/w7/98/d/21.jpg", "/html/");


      ViewBag.cc = string.Format("github_emoji:{0}-{3}-{4},twemoji:{1},font_awesome:{2}",
        list.github_emoji.Count, list.twemoji.Count, list.font_awesome.Count,
      value, count
        );
      ViewBag.imgUrl = asd;

      return View();
    }





  }



  public class Github_emojiItem
  {
    /// <summary>
    /// 
    /// </summary>
    public string category { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> list { get; set; }
  }

  public class Root
  {
    /// <summary>
    /// 
    /// </summary>
    public List<Github_emojiItem> github_emoji { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> twemoji { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> font_awesome { get; set; }
  }


}