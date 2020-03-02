using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sop.Data;
using Sop.FileUpload.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sop.FileUpload.Controllers
{
    public class HomeController : Controller
    {
        #region private
        private readonly SopContext _context;

        public HomeController(SopContext context)
        {
            _context = context;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost()]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
       
        //public async Task<IActionResult> About()
        //{

        //    //XmlDocument docXml = new XmlDocument();
        //    //string file = FileUtility.GetDiskFilePath("/wwwroot/AreaCode/AreaCodeInfo.xml");
        //    //docXml.Load(file);
        //    //XmlNodeList nodelist = docXml.GetElementsByTagName("area");
        //    //List<CitysInfo> list = new List<CitysInfo>();
        //    //foreach (XmlNode node in nodelist)
        //    //{
        //    //    list.Add(new CitysInfo()
        //    //    {
        //    //        Code = node.Attributes["code"].Value,
        //    //        Name = node.Attributes["name"].Value,

        //    //    });
        //    //}

        //    //foreach (var info in list)
        //    //{
        //    //    info.DateCreated = DateTime.Now;
        //    //    info.ChildCount = 0;
        //    //    info.Enabled = 1;

        //    //    _context.CitysServer.Add(info);
        //    //    await _context.SaveChangesAsync();
        //    //}


        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
