using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sop.Core;
using System;
using System.Collections.Generic;
using Sop.Data;

namespace Sop.Tools.Controllers
{
    public class IdController : Controller
    {
        private readonly SopContext _context;

        public IdController(SopContext context)
        {
            _context = context;
        }
        // GET: ID
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Create(int code)
        {
            int number = 0;
            try
            {
                number = Convert.ToInt16(code);
            }
            catch
            {
                return View("Index");
            }
            if (number < 1)
                return View("Index");
            List<IdCardHelper> list = IdCardHelper.Radom(number);
            return View("Index", list);
        }

        [HttpGet]
        public IActionResult CheckId(string code)
        {
            //try
            //{
            //    IDCardNumber card = IDCardNumber.Get(code);
            //    //发证地
            //    lbAddress.Text = card.Province + card.Area + card.City;
            //    //出生日期
            //    lbBirthday.Text = card.Age.ToString("yyyy年MM月dd日");
            //    //性别
            //    lbSex.Text = card.Sex == 0 ? "女" : "男";


            //}
            //catch (Exception ex)
            //{
            //    return View(new { })；
            //}
            return View();

        }

        // GET: ID/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ID/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ID/Create
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

        // GET: ID/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ID/Edit/5
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

        // GET: ID/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ID/Delete/5
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