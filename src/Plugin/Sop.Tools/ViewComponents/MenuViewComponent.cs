using Microsoft.AspNetCore.Mvc;
using Sop.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sop.Tools.ViewComponents
{
    [ViewComponent(Name = "_Menu")]
    public class MenuViewComponent : ViewComponent
    {


        public async Task<IViewComponentResult> InvokeAsync(int id)
        {

            var items = await GetItemsAsync(id);
            return View("_Menu", items);
        }
        private Task<List<MenuInfo>> GetItemsAsync(int id)
        {
            List<MenuInfo> list = new List<MenuInfo>();
            var area = RouteData.Values["area"]?.ToString();
            var controller = RouteData.Values["controller"]?.ToString();
            var action = RouteData.Values["action"]?.ToString();

            list.Add(new MenuInfo()
            {
                Name = "全部",
                UserMvcRoute = true,
                AspController = "Home",
                AspAction = "Index",
                IsActive = false
            });
            list.Add(new MenuInfo()
            {
                Name = "便民查询",
                IsActive = false,
                UserMvcRoute = true,
                AspController = "Home",
                AspAction = "Common",
            });
            list.Add(new MenuInfo()
            {
                Name = "开发工具",
                UserMvcRoute = true,
                AspController = "Home",
                AspAction = "Tool",
                IsActive = false
            });
            List<MenuInfo> Newlist = new List<MenuInfo>();
            foreach (var info in list)
            {
                if (info.AspArea == area &&
                    info.AspController == controller &&
                    info.AspAction == action)
                {
                    info.IsActive = true;
                }
                else
                {
                    info.IsActive = false;
                }
                Newlist.Add(info);
            }

           












            //foreach (var info in list)
            //{
            //if (info.AspArea == area &&
            //    info.AspController == controller &&
            //    info.AspAction == action
            //   )
            //    {
            //        info.IsActive = true;
            //    }
            //    else
            //    {

            //    }

            //}
            return Task.FromResult(list);
        }
    }
}
