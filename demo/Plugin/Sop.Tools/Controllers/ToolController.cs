using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sop.Data;

namespace Sop.Tools.Controllers
{
    public class ToolController : Controller
    {
        private readonly SopContext _context;

        public ToolController(SopContext context)
        {
            _context = context;
        }
        // GET: Tool
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult UpperAndLower()
        {
            return View();
        }

        public IActionResult CardNumber()
        {
            var list = _context.CitysServer.ToList();
            return View();
        }
        public IActionResult GetAreInfo(long parentCode = 86)
        {
            var list = _context.CitysServer.Select(n => n.ParentCode == parentCode).ToList();
            return Json(list);
        }












        // GET: Tool/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tool/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tool/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Tool/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tool/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Tool/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tool/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}