using AutoMapper;
using Common.Logging;
using Sop.Common.Serialization.Json;
using Sop.Core;
using Sop.Core.Mvc;
using Sop.Core.Mvc.SystemMessage;
using Sop.Core.WebUtility;
using Sop.Services;
using Sop.Services.Auth;
using Sop.Services.Model;
using Sop.Services.Parameter;
using Sop.Services.Servers;
using Sop.Services.Treeview;
using Sop.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Sop.Web.Controllers
{
    //[Authorize]
    public class ItemController : BaseController
    {


        public OwinAuthenticationService _authentication { get; set; }
        public UsersService _usersService { get; set; }
        public ItemsService _itemsService { get; set; }
        public CatalogService _catalogService { get; set; }
        public PostService _postService { get; set; }
        private static readonly ILog Logger = LogManager.GetLogger<ItemController>();



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
            list = list.Any() ? list : new List<ItemsInfo>();
            return View(list);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Crawler()
        {


            ViewBag.Html = "<div>asdasd</div>";

            return View();
        }



        [HttpGet]
        public ActionResult CreatePost(int CatalogId)
        {
            #region Create 点击自动生成
            var info = new PostInfo()
            {
                CatalogId = CatalogId,
                UserId = UserContext.GetGetUserId(),
                Title = CatalogId + "-标题 Post",
                Content = CatalogId + "<br />" + @"

      [TOC]

      #### Disabled options

      - TeX (Based on KaTeX);
      - Emoji;
      - Task lists;
      - HTML tags decode;
      - Flowchart and Sequence Diagram;

      #### Editor.md directory

          editor.md/
                  lib/
                  css/
                  scss/
                  tests/
                  fonts/
                  images/
                  plugins/
                  examples/
                  languages/     
                  editormd.js
                  ...

      ```html
      guojiaqiu test
      <!-- English -->
      <script src=""../dist/js/languages/en.js""></script>

      <!-- 繁體中文 -->
      <script src=""../dist/js/languages/zh-tw.js""></script>
      ```

             ",
                CreatedIp = WebUtility.GetIp(),
                DateCreated = DateTime.Now.AddHours(-(new Random().Next(1000))),
                Description = "descriptiondescriptiondescriptiondescriptiondescriptiondescription-----" + new Random().Next(1000),

                HtmlContentPath = "",
                ViewCount = new Random().Next(1000),

            };
            _postService.Create(info);
            #endregion

            return Json("ok", JsonRequestBehavior.AllowGet);
        } 
        public ActionResult Catalog(int id)
        {
            ViewBag.Id = id;
            return View();
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
            info = _itemsService.GetById(id);
            if (info == null)
            {
                return View(info.MapTo<ItemsViewModel>());

            }
            var infoVM = info.MapTo<ItemsViewModel>();
            var infoParent = _itemsService.GetById(info.ParentId);
            infoVM.ParentName = infoParent == null ? "" : infoParent.Name;
            return View(infoVM);
        }
        [HttpPost]
        public ActionResult ItemsEdit(ItemsViewModel info)
        {
            info.UserId = "";
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
                var info = _itemsService.GetById(id);
                if (info != null)
                {
                    _itemsService.Delete(info);
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
        public JsonResult GetCataloTree(int itemId)
        {

            var list = _catalogService.GetByItemId(itemId);

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
            info.TenantId = 0;
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
            return Redirect(SiteUrls.Instance().ItemIndex(info.ItemId));

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

        #region Post
        [HttpGet]
        public JsonResult GetPostList(PostParameter parameter)
        {
            var list = _postService.GetPostList(parameter);

            return Json(new { total = list.TotalCount, rows = list, data = "" });
        }
        [HttpGet]
        public ActionResult Post(int id)
        {


            var info = _postService.Get(id);
            PostViewModel infoVM = new PostViewModel();
            UsersInfo userInfo = new UsersInfo();
            if (info != null)
            {
                infoVM = info.MapTo<PostViewModel>();
                userInfo = _usersService.GetByUserId(info.UserId);
                infoVM.NickName = userInfo == null ? "" : userInfo.NickName;
                return View(infoVM);
            }
            else
            {
                return Redirect(SiteUrls.Instance().Error404());
            }

        }
        [HttpPost]
        public JsonResult PostDelete(string ids)
        {
            var uid = UserContext.GetGetUserId();


            bool result = true;
            List<PostInfo> list = new List<PostInfo>();
            if (!string.IsNullOrEmpty(ids) && ids.Contains(','))
            {
                string[] strIds = ids.Split(',');

                foreach (var strid in strIds)
                {
                    int id = 0;
                    if (!string.IsNullOrEmpty(strid) && int.TryParse(strid, out id))
                    {
                        var info = _postService.Get(id);
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
                    _postService.Delete(item);
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

        [HttpGet]
        public ActionResult PostEdit(int catalogId = 0, int id = 0)
        {

            PostViewModel postVm = new PostViewModel();
            postVm.TitleImg = Guid.NewGuid().ToString("N");
            postVm.CatalogId = catalogId;

            if (id != 0)
            {
                var info = _postService.Get(id);
                if (info != null && info.CatalogId == catalogId)
                {
                    postVm = info.MapTo<PostViewModel>();
                }
            }
            return View(postVm);
        }
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PostEdit(PostViewModel postView)
        {
            //Sop.Framework.Utilities.WebUtility.GetIp();

            bool isModel = TryValidateModel(postView);
            postView.UserId = UserContext.GetGetUserId();
            postView.DateCreated = DateTime.Now;
            postView.HtmlContentPath = "HtmlContentPath";
            postView.ViewCount = 1;
            postView.CreatedIP = WebUtility.GetIp();

            var info = postView.MapTo<PostInfo>();

            if (isModel)
            {
                if (postView.Id > 0)
                {
                    _postService.Update(info);
                    return Redirect(SiteUrls.Instance().ItemPost(postView.CatalogId));

                }
                else
                {
                    _postService.Create(info);
                    return Redirect(SiteUrls.Instance().ItemPost(postView.CatalogId));
                }

            }
            return View(postView);

        }

        #endregion
         
    }
}
