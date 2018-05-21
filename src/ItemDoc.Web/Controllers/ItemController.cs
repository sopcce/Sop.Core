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
using System.Text;
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
        return View(info.MapTo<ItemsViewModel>());
      }
      info = _itemsService.GetById(id.ToString());
      if (info == null)
      {
        return View(info.MapTo<ItemsViewModel>());

      }
      var infoVM = info.MapTo<ItemsViewModel>();
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
          _itemsService.Update(info.MapTo<ItemsInfo>());
        }
      }
      else
      {
        _itemsService.Insert(info.MapTo<ItemsInfo>());
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
          var infoVm = info.MapTo<CatalogViewModel>();
          if (info.ParentId != 0)
          {
            var infoParent = _catalogService.Get(info.ParentId);
            infoVm.ParentName = infoParent == null ? "" : infoParent.Name;
          }
          return View(infoVm);
        }
        else
        {
          return View(info.MapTo<CatalogViewModel>());
        }
      }
      else
      {
        var infoVm = info.MapTo<CatalogViewModel>();
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
          var upInfo = info.MapTo<CatalogInfo>();
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
        _catalogService.Create(info.MapTo<CatalogInfo>());
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
    public JsonResult GetPostList(PostParameter parameter)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("                     <p>Cum sociis natoque penatibus et magnis <a href=\"#\">dis parturient montes</a>, nascetur ridiculus mus. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum. Sed posuere consectetur est at lobortis. Cras mattis consectetur purus sit amet fermentum.</p>");
      sb.Append("                     ");
      sb.Append("                     ");
      sb.Append("                     ");
      sb.Append("                     <p>Cum sociis natoque penatibus et magnis <a href=\"#\">dis parturient montes</a>, nascetur ridiculus mus. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum. Sed posuere consectetur est at lobortis. Cras mattis consectetur purus sit amet fermentum.</p>");
      sb.Append("                     <blockquote>");
      sb.Append("                         <p>Curabitur blandit tempus porttitor. <strong>Nullam quis risus eget urna mollis</strong> ornare vel eu leo. Nullam id dolor id nibh ultricies vehicula ut id elit.</p>");
      sb.Append("                     </blockquote>");
      sb.Append("                     <p>Etiam porta <em>sem malesuada magna</em> mollis euismod. Cras mattis consectetur purus sit amet fermentum. Aenean lacinia bibendum nulla sed consectetur.</p>");
      sb.Append("                     <h2>Heading</h2>");
      sb.Append("                     <p>Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor. Duis mollis, est non commodo luctus, nisi erat porttitor ligula, eget lacinia odio sem nec elit. Morbi leo risus, porta ac consectetur ac, vestibulum at eros.</p>");
      sb.Append("                     <h3>Sub-heading</h3>");
      sb.Append("                     <p>Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.</p>");
      sb.Append("                     <pre><code>Example code block</code></pre>");
      sb.Append("                     <p>Aenean lacinia bibendum nulla sed consectetur. Etiam porta sem malesuada magna mollis euismod. Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh, ut fermentum massa.</p>");
      sb.Append("                     <h3>Sub-heading</h3>");
      sb.Append("                     <p>Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aenean lacinia bibendum nulla sed consectetur. Etiam porta sem malesuada magna mollis euismod. Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus.</p>");
      sb.Append("                     <ul>");
      sb.Append("                         <li>Praesent commodo cursus magna, vel scelerisque nisl consectetur et.</li>");
      sb.Append("                         <li>Donec id elit non mi porta gravida at eget metus.</li>");
      sb.Append("                         <li>Nulla vitae elit libero, a pharetra augue.</li>");
      sb.Append("                     </ul>");
      sb.Append("                     <p>Donec ullamcorper nulla non metus auctor fringilla. Nulla vitae elit libero, a pharetra augue.</p>");
      sb.Append("                     <ol>");
      sb.Append("                         <li>Vestibulum id ligula porta felis euismod semper.</li>");
      sb.Append("                         <li>Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.</li>");
      sb.Append("                         <li>Maecenas sed diam eget risus varius blandit sit amet non magna.</li>");
      sb.Append("                     </ol>");
      sb.Append("                     <p>Cras mattis consectetur purus sit amet fermentum. Sed posuere consectetur est at lobortis.</p>");
      sb.Append("                     <p class=\"blog-post-meta\">December 14, 2013 by <a href=\"#\">Chris</a></p>");
      sb.Append(" ");
      sb.Append("                     <p>Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. </p>");
      sb.Append("                     <ul>");
      sb.Append("                         <li>Praesent commodo cursus magna, vel scelerisque nisl consectetur et.</li>");
      sb.Append("                         <li>Donec id elit non mi porta gravida at eget metus.</li>");
      sb.Append("                         <li>Nulla vitae elit libero, a pharetra augue.</li>");
      sb.Append("                     </ul>");
      sb.Append("                     ");
      sb.Append("                     <p>Donec ullamcorper nulla non metus auctor fringilla. Nulla vitae elit libero, a pharetra augue.</p>");




      _postService.Create(new PostInfo()
      {
        CatalogId = parameter.CatalogId,
        UserId = UserContext.GetGetUserId(),
        Title = parameter.CatalogId + "-标题 Post-",
        Content = parameter.CatalogId + "<br />" + sb.ToString(),
        DateCreated = DateTime.Now,
        DisplayOrder = 1,
        HtmlContentPath = "",
        ViewCount = new Random().Next(1000),

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






    #region Post
    [HttpGet]
    public ActionResult Post(int id)
    {


      var info = _postService.Get(id);
      var infoVM = info.MapTo<PostViewModel>();
      var userInfo = _usersService.GetByUserId(info.UserId);


      infoVM.NickName = userInfo != null ? "" : userInfo.NickName;
      //PostViewModel infoModel = info.AsModel();

      return View(infoVM);
    }

    #endregion















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
