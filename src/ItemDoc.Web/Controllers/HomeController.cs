using ItemDoc.Core.Auth;
using ItemDoc.Services.Servers;
using ItemDoc.Services.ViewModel;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ItemDoc.Services;

namespace ItemDoc.Web.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationService _authentication;
        private IUsersService _usersService;
        private IPostService _postService;

        public HomeController(
          IUsersService usersService,
          IAuthenticationService authentication,
          IPostService postService)
        {
            _authentication = authentication;
            _postService = postService;
            _usersService = usersService;
        }

        public ActionResult Index()
        {
            var list = _postService.GetAll().MapToList<PostViewModel>().ToList();
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