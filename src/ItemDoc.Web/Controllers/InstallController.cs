using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItemDoc.Web.Controllers
{
  public class InstallController : Controller
  {
    // GET: Install
    public ActionResult Index()
    {

      //判断是否为第一次安装
      //第一次安装判断环境权限、数据库是否设置等操作。 
      // return RedirectToAction("Index", "Home");
      //return RedirectToAction("About", "Home");
      //About
      //return RedirectToAction("Index", "User");
      return View();

    }

    // GET: Install/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: Install/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Install/Create
    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
      try
      {
        // TODO: Add insert logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Install/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: Install/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Install/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Install/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
