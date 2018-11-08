using AutoMapper;
using Common.Logging;
using ItemDoc.Core;
//using ItemDoc.Core.Auth;
using ItemDoc.Core.Mvc;
using ItemDoc.Core.Mvc.SystemMessage;
using ItemDoc.Core.WebCrawler.Events;
using ItemDoc.Framework.Utility;
using ItemDoc.Services;
using ItemDoc.Services.Auth;
using ItemDoc.Services.Model;
using ItemDoc.Services.Parameter;
using ItemDoc.Services.Servers;
using ItemDoc.Services.Treeview;
using ItemDoc.Services.ViewModel;
using ItemDoc.Web.Controllers.Base;
using Sop.Common.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ItemDoc.Core.WebCrawler;
using OpenQA.Selenium;


namespace ItemDoc.Web.Controllers
{
    [Authorize]
    public class ItemController : BaseController
    {


        public OwinAuthenticationService _authentication { get; set; }
        public UsersService _usersService { get; set; }
        public ItemsService _itemsService { get; set; }
        public CatalogService _catalogService { get; set; }
        public PostService _postService { get; set; }
        private static readonly ILog Logger = LogManager.GetLogger<ItemController>();

        private static StringBuilder sb = new StringBuilder();

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
        public async Task<ActionResult> Crawler()
        {


            string html = "<div>asdasd</div>";


            await Main();

            ViewBag.Html = html;

            return View();
        }
        static async Task Main()
        {
            var hotelUrl = "http://hotels.ctrip.com/hotel/434938.html";
            var hotelCrawler = new Crawler();
            hotelCrawler.OnStart += (s, e) =>
            { 
                Logger.Info("爬虫开始抓取地址：" + e.Uri.ToString());
            };
            hotelCrawler.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出现错误：" + e.Uri.ToString() + "，异常消息：" + e.Exception.ToString());
            };
            hotelCrawler.OnCompleted += (s, e) =>
            {
                HotelCrawler(e);
            };
            var operation = new Operation
            {
                Action = (x) =>
                {
                    //通过Selenium驱动点击页面的“酒店评论”
                    x.FindElement(By.XPath("//*[@id='commentTab']")).Click();
                },
                Condition = (x) =>
                {
                    //判断Ajax评论内容是否已经加载成功
                    return x.FindElement(By.XPath("//*[@id='commentList']")).Displayed && x.FindElement(By.XPath("//*[@id='hotel_info_comment']/div[@id='commentList']")).Displayed && !x.FindElement(By.XPath("//*[@id='hotel_info_comment']/div[@id='commentList']")).Text.Contains("点评载入中");
                },
                Timeout = 5000
            };

            await hotelCrawler.Start(new Uri(hotelUrl), null, operation);//不操作JS先将参数设置为NULL

           
        }
        private static void HotelCrawler(OnCompletedEventArgs e)
        {
            //Console.WriteLine(e.PageSource);
            //File.WriteAllText(Environment.CurrentDirectory + "\\cc.html", e.PageSource, Encoding.UTF8);

            var hotelName = e.WebDriver.FindElement(By.XPath("//*[@id='J_htl_info']/div[@class='name']/h2[@class='cn_n']")).Text;
            var address = e.WebDriver.FindElement(By.XPath("//*[@id='J_htl_info']/div[@class='adress']")).Text;
            var price = e.WebDriver.FindElement(By.XPath("//*[@id='div_minprice']/p[1]")).Text;
            var score = e.WebDriver.FindElement(By.XPath("//*[@id='divCtripComment']/div[1]/div[1]/span[3]/span")).Text;
            var reviewCount = e.WebDriver.FindElement(By.XPath("//*[@id='commentTab']/a")).Text;

            var comments = e.WebDriver.FindElement(By.XPath("//*[@id='hotel_info_comment']/div[@id='commentList']/div[1]/div[1]/div[1]"));
            var currentPage = Convert.ToInt32(comments.FindElement(By.XPath("div[@class='c_page_box']/div[@class='c_page']/div[contains(@class,'c_page_list')]/a[@class='current']")).Text);
            var totalPage = Convert.ToInt32(comments.FindElement(By.XPath("div[@class='c_page_box']/div[@class='c_page']/div[contains(@class,'c_page_list')]/a[last()]")).Text);
            var messages = comments.FindElements(By.XPath("div[@class='comment_detail_list']/div"));
            var nextPage = Convert.ToInt32(comments.FindElement(By.XPath("div[@class='c_page_box']/div[@class='c_page']/div[contains(@class,'c_page_list')]/a[@class='current']/following-sibling::a[1]")).Text);

            sb.Clear();
            Console.WriteLine();
            Console.WriteLine("名称：" + hotelName);
            Console.WriteLine("地址：" + address);
            Console.WriteLine("价格：" + price);
            Console.WriteLine("评分：" + score);
            Console.WriteLine("数量：" + reviewCount);
            Console.WriteLine("页码：" + "当前页（" + currentPage + "）" + "下一页（" + nextPage + "）" + "总页数（" + totalPage + "）" + "每页（" + messages.Count + "）");
            Console.WriteLine();
            Console.WriteLine("===============================================");
            Console.WriteLine();
            Console.WriteLine("点评内容：");

            foreach (var message in messages)
            {
                Console.WriteLine("帐号：" + message.FindElement(By.XPath("div[contains(@class,'user_info')]/p[@class='name']")).Text);
                Console.WriteLine("房型：" + message.FindElement(By.XPath("div[@class='comment_main']/p/a")).Text);
                Console.WriteLine("内容：" + message.FindElement(By.XPath("div[@class='comment_main']/div[@class='comment_txt']/div[1]")).Text.Substring(0, 50) + "....");
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("===============================================");
            Console.WriteLine("地址：" + e.Uri.ToString());
            Console.WriteLine("耗时：" + e.Milliseconds + "毫秒");

          
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
                infoVM.NickName = userInfo != null ? "" : userInfo.NickName;
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
            postVm.Id = 0;
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
            postView.DisplayOrder = 0;
            postView.HtmlContentPath = "HtmlContentPath";
            postView.ViewCount = 1;
            postView.CreatedIP = WebUtility.GetIp();

            var info = postView.MapTo<PostInfo>();

            if (isModel)
            {
                if (postView.Id > 0)
                {
                    _postService.Update(info);
                    return Redirect(SiteUrls.Instance().ItemPost(postView.Id));

                }
                else
                {
                    var id = _postService.Create(info);
                    return Redirect(SiteUrls.Instance().ItemPost(id));
                }

            }
            return View(postView);

        }

        #endregion



        [HttpGet]
        public ActionResult CreatePost(int CatalogId)
        {
            #region Create 点击自动生成

            _postService.Create(new PostInfo()
            {
                CatalogId = CatalogId,
                UserId = UserContext.GetGetUserId(),
                Title = CatalogId + "-标题 Post-",
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
                DateCreated = DateTime.Now.AddDays(new Random().Next(1000)),
                Description = "descriptiondescriptiondescriptiondescriptiondescriptiondescription-----" + new Random().Next(1000),
                DisplayOrder = 1,
                HtmlContentPath = "",
                ViewCount = new Random().Next(1000),

            });
            #endregion

            return Json("ok", JsonRequestBehavior.AllowGet);
        }












        public ActionResult Catalog(int id)
        {
            ViewBag.Id = id;
            return View();
        }


    }
}
