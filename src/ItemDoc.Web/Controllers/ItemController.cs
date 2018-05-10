using Common.Logging;
using ItemDoc.Core;
using ItemDoc.Core.Auth;
using ItemDoc.Core.Mvc;
using ItemDoc.Core.Mvc.SystemMessage;
using ItemDoc.Services.Mapping;
using ItemDoc.Services.Model;
using ItemDoc.Services.Servers;
using ItemDoc.Services.ViewModel;
using ItemDoc.Web.Controllers.Base;
using Sop.Common.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using ItemDoc.Services.Treeview;

namespace ItemDoc.Web.Controllers
{
  [Authorize]
  public class ItemController : BaseController
  {

    #region MyRegion
    private readonly IAuthenticationService _authentication;
    private readonly IUsersService _usersService;
    private readonly IItemsService _itemsService;
    private readonly ICatalogService _catalogService;
    private readonly IPostService _postService;
    private static readonly ILog Logger = LogManager.GetLogger<ItemController>();

    public ItemController(
        IUsersService usersService,
        IAuthenticationService authentication,
        IItemsService itemsService,
        ICatalogService catalogService,
        IPostService postService)
    {
      _catalogService = catalogService;
      _postService = postService;
      _authentication = authentication;
      _itemsService = itemsService;
      _usersService = usersService;
    }
    #endregion

    public ActionResult Index()
    {
      //_itemsService.GetById("-1");

      Logger.Info("wo shi ri zhi ");
      try
      {
        string a = "asd";
        int b = Convert.ToInt32(a);

      }
      catch (Exception ex)
      {
        Logger.Error(ex.Message, ex);

      }

      var list = _itemsService.GetAll().ToList();
      return View(list);
    }

    #region Items

    public ActionResult _ItemList()
    {
      return View();
    }

    public ActionResult ItemsEdit(int id = 0)
    {
      ItemsInfo info = new ItemsInfo();
      if (id == 0)
      {
        return View(info.AsModel());
      }
      info = _itemsService.GetById(id.ToString());
      if (info == null)
      {
        return View(info.AsModel());

      }
      var infoVM = info.AsModel();
      var infoParent = _itemsService.GetById(info.ParentId.ToString());
      infoVM.ParentName = infoParent == null ? "" : infoParent.Name;
      return View(infoVM);
    }
    [HttpPost]
    public ActionResult ItemsEdit(ItemsViewModel info)
    {
      if (info.Id > 0)
      {
        if (ModelState.IsValid)
        {
          _itemsService.Update(info.AsInfo());
        }
      }
      else
      {
        _itemsService.Insert(info.AsInfo());
      }
      //todo 后期可以考虑json 提示成功失败
      return RedirectToAction("Index", "Item");
    }


    public JsonResult ItemsDelete(int id)
    {
      if (!string.IsNullOrEmpty(id.ToString()))
      {
        var info = _itemsService.GetById(id.ToString());
        if (info != null)
        {
          _itemsService.Delete(new ItemsInfo() { Id = id });
          info = _itemsService.GetById(id.ToString());
          if (info == null)
            return Json(new SystemMessageData() { Content = "删除成功", Type = SystemMessageType.Success }, PropertyNameType.ToLower);
        }
      }
      return Json(new SystemMessageData() { Content = "删除失败", Type = SystemMessageType.Error }, PropertyNameType.ToLower);
    }
    public ActionResult ItemsIndex(int id)
    {
      if (string.IsNullOrEmpty(id.ToString()))
      {
        return RedirectToAction("Index");
      }
      ViewBag.itemId = id;
      return View();
    }
    #endregion

    #region Catalog
    [HttpPost]
    public JsonResult GetCataloTree(string itemId)
    {
      #region 注释
      //List<Node> list = new List<Node>
      //{
      //  new Node()
      //  {
      //    Text = "Parent1",
      //    Tags = new string [2]{"10","2"},
      //    Href = "#",
      //    Nodes = new List<Node>()
      //    {
      //      new Node() {
      //        Text = "Child 1",
      //        Href ="#",
      //        Tags = new string [1]{"2"},
      //        Nodes = new List<Node>()
      //        {
      //          new Node() { Text ="Grandchild 1", Href="#", Tags = new string [1]{"1"}},
      //          new Node() { Text ="Grandchild 2", Href="#", Tags = new string [1]{"1"}}
      //        }
      //      },
      //      new Node()
      //      {
      //        Text ="Child 2",
      //        Href ="#",
      //        Tags = new string [1]{"2"},
      //        Nodes = new List<Node>()
      //        {
      //          new Node() { Text ="Grandchild 1", Href="#", Tags = new string [1]{"1"}},
      //          new Node() { Text ="Grandchild 2", Href="#", Tags = new string [1]{"1"}}
      //        }
      //      } ,
      //      new Node()
      //      {
      //        Text = "Child 3",
      //        Href = "#" ,
      //        Tags = new string [1]{"2"},
      //        Nodes = new List<Node>()
      //        {
      //          new Node() { Text ="Grandchild 1", Href="#", Tags = new string [1]{"1"}},
      //          new Node() { Text ="Grandchild 2", Href="#", Tags = new string [1]{"1"}}
      //        }
      //      }
      //    }
      //  },

      //  new Node() { Text = "Parent2", Href = "#", Tags = new string [1]{"2"}  },
      //  new Node() { Text = "Parent3", Href = "#", Tags = new string [1]{"2"}  },
      //  new Node() { Text = "Parent4", Href = "#", Tags = new string [1]{"2"}  },
      //  new Node() { Text = "Parent5", Href = "#", Tags = new string [1]{"2"}}
      //}; 
      #endregion

      var list = _catalogService.GetByItemId(itemId).OrderByDescending(n => n.DisplayOrder);

      var json = list.ToNodes();
      return Json(json);

    }
    [HttpGet]
    public ActionResult CatalogEdit(int id = 0, int itemId = 0, int ParentId = 0)
    {
      CatalogInfo info = new CatalogInfo();
      info.ItemId = itemId;
      info.ParentId = ParentId;
      if (info.ParentId == 0)
      {
        if (id > 0)
        {
          info = _catalogService.Get(id);
          var infoVm = info.AsModel();
          if (info.ParentId != 0)
          {
            var infoParent = _catalogService.Get(info.ParentId);
            infoVm.ParentName = infoParent == null ? "" : infoParent.Name;
          }
          return View(infoVm);
        }
        else
        {
          return View(info.AsModel());
        }
      }
      else
      {
        var infoVm = info.AsModel();
        if (id > 0)
        {
          info = _catalogService.Get(id);
          if (info.ParentId != 0)
          {
            var infoParent = _catalogService.Get(info.ParentId);
            infoVm.ParentName = infoParent == null ? "" : infoParent.Name;
          }
          return View(infoVm);
        }
        else
        {
          if (info.ParentId != 0)
          {
            var infoParent = _catalogService.Get(info.ParentId);
            infoVm.ParentName = infoParent == null ? "" : infoParent.Name;
          }
          return View(infoVm);
        }



      }

    }
    [HttpPost]
    public ActionResult CatalogEdit(CatalogViewModel info)
    {
      info.UserId = UserContext.GetGetUserId();


      if (info.Id > 0)
      {
        if (ModelState.IsValid)
        {
          var upInfo = info.AsInfo();
          var oldInfo = _catalogService.Get(upInfo.Id);
          upInfo.ChildCount = oldInfo.ChildCount;
          upInfo.Depth = oldInfo.Depth;
          upInfo.ParentIdList = oldInfo.ParentIdList;
          upInfo.ParentId = oldInfo.ParentId;
          upInfo.DateCreated = DateTime.Now;
          upInfo.ContentCount = oldInfo.ContentCount;


          _catalogService.Update(upInfo);
        }
      }
      else
      {
        _catalogService.Create(info.AsInfo());
      }
      return Redirect(SiteUrls.Instance().ItemItemsIndex(info.ItemId));

    }
    [HttpPost]
    public JsonResult CatalogDelete(string ids)
    {
      var uid = UserContext.GetGetUserId();


      bool result = true;
      List<CatalogInfo> list = new List<CatalogInfo>();
      if (!string.IsNullOrEmpty(ids) && ids.Contains(','))
      {
        string[] strIds = ids.Split(',');

        foreach (var strid in strIds)
        {
          int id = 0;
          if (!string.IsNullOrEmpty(strid) && int.TryParse(strid, out id))
          {
            var info = _catalogService.Get(id);
            if (info != null)
            {
              //TODO 判断是否具有权限删除
              if (info.UserId == UserContext.GetGetUserId())
              {
                list.Add(info);
              }



            }
            else
            {
              //TODO 不存在删除记录
              result = false;
              break;

            }

          }
        }
      }
      if (result)
      {
        foreach (var item in list)
        {
          _catalogService.Delete(item);
        }

      }
      if (result)
      {
        return Json(new SystemMessageData() { Content = "删除成功", Type = SystemMessageType.Success });

      }
      else
      {
        return Json(new SystemMessageData() { Content = "删除失败", Type = SystemMessageType.Error });

      }
    }

    #endregion

    [HttpGet]
    public JsonResult GetDepartment(int catalogId, int pageSize, int pageIndex, string keyword, string sortOrder, string sortName)
    {
      var lstRes = new List<Department>();
      for (var i = 0; i < 500; i++)
      {
        var oModel = new Department();
        oModel.ID = Guid.NewGuid().ToString();

        oModel.ParentName = catalogId.ToString();
        oModel.Name = catalogId + "" + i;
        oModel.Level = i.ToString();
        oModel.Desc = "暂无描述信息";
        lstRes.Add(oModel);
      }

      var total = lstRes.Count;
      var rows = lstRes.Skip(pageIndex).Take(pageSize);
      switch (sortName)
      {
        case "ID":
          rows = sortOrder == "asc"
            ? rows.OrderBy(n => n.ID).ToList()
            : rows.OrderByDescending(n => n.ID).ToList();
          break;
        case "Name":
          rows = sortOrder == "asc"
            ? rows.OrderBy(n => n.Name).ToList()
            : rows.OrderByDescending(n => n.Name).ToList();
          break;
        case "Level":
          rows = sortOrder == "asc"
            ? rows.OrderBy(n => n.Level).ToList()
            : rows.OrderByDescending(n => n.Level).ToList();
          break;

      }




      return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    public JsonResult GetPostList(PostParameter parameter)
    {

      _postService.Create(new PostInfo()
      {
        CatalogId = parameter.CatalogId,
        UserId = UserContext.GetGetUserId(),
        Title = "测试-" + parameter.CatalogId,
        Content = "测试内容啊啊啊啊" + parameter.CatalogId,
        DateCreated = DateTime.Now.AddDays(new Random().Next(1000)),
        DisplayOrder = 1,
        HtmlContentPath = "",
        ViewCount = 0,

      });

      Stopwatch swStopwatch = new Stopwatch();
      swStopwatch.Start();
      //
      //var list = _postService.GetPostList(parameter);

      swStopwatch.Stop();
      Stopwatch swStopwatch1 = new Stopwatch();
      swStopwatch1.Start();
      //
      var list = _postService.GetPostList1(parameter);

      swStopwatch1.Stop();

      //var personViews = list.Select(x => x.AsModel<PostViewModel>());




      return Json(new { total = list.TotalCount, rows = list, data = swStopwatch.ElapsedMilliseconds, data1 = swStopwatch1.ElapsedMilliseconds });
    }























    public ActionResult Catalog(int id)
    {
      ViewBag.Id = id;
      return View();
    }










    // GET: Item/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Item/Create
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

    // GET: Item/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: Item/Edit/5
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

    // GET: Item/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Item/Delete/5
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


  public class Department
  {
    public string ID { set; get; }

    public string Name { set; get; }

    public string ParentName { set; get; }

    public string Level { set; get; }

    public string Desc { set; get; }
  }
}
