using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sop.Domain.Service;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;


        // private readonly IUserAboutRepository _userAboutRepository;
        // private readonly IUnitOfWork _unitOfWork;


        public HomeController(IUserService userService)
        {
            _userService = userService;
            // _userAboutRepository = userAboutRepository;
            // _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var readMeVm = _userService.GetReadMeVmByUserId(1);

            return View(readMeVm);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}