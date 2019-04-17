using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sop.Core.Models;
using Sop.Tools.Models;

namespace Sop.Tools.Controllers
{
    public class HomeController : Controller
    {  
         
        public IActionResult Index()
        { 
            return View();
        }
        public IActionResult Common()
        {
            return View();
        }
        public IActionResult Tool()
        {
            return View();
        }
        public IActionResult _Menu()
        {
            List<MenuInfo> list = new List<MenuInfo>
            {
                new MenuInfo()
                {
                    Name = "全部",
                    Url = "/",
                    IsActive = true
                },
                new MenuInfo()
                {
                    Name = "便民查询",
                    Url = "/",
                    IsActive = false
                },
                new MenuInfo()
                {
                    Name = "开发工具",
                    Url = "/",
                    IsActive = false
                },
                new MenuInfo()
                {
                    Name = "个人收藏",
                    Url = "/",
                    IsActive = false
                }
            };
            return ViewComponent("_Menu", list);
        }
        //public IActionResult _Menu()
        //{
           
        //    return PartialView("_Menu");
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
