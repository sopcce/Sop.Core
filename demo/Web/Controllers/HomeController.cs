//using Sop.Core.Auth;
using AutoMapper;
using Sop.Services.Servers;
using Sop.Services.ViewModel;
using System.Linq;
using System.Web.Mvc;

namespace Sop.Web.Controllers
{
    public class HomeController : Controller
    {
        //private IAuthenticationService _authentication;
        public UsersService _usersService { get; set; }
        public PostService _postService { get; set; }
 

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            var allList = _postService.GetAll();
            var list = allList.MapToList<PostViewModel>().ToList();
            ViewData["postView"] = list;
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




















        #region 展示图片
        public ActionResult ShowImage()
        {
            //var imageList = db.ImageServerInfo.Where<ImageServerInfo>(c => c.FlgUsable == true).ToList();
            //ViewData["imageList"] = imageList;
            return View();
        }
        #endregion


    }
}