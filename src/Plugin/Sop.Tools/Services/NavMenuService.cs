using System.Collections.Generic;
using Sop.Core.Models;
using Sop.Tools.Models;
using Sop.Tools.Services.Interfaces;

namespace Sop.Tools.Services
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public class NavMenuService : INavMenuService
    {
     

        /// <summary>
        /// 获取导航菜单
        /// </summary>
        /// <returns></returns>
        public IList<MenuInfo> GetNavMenus()
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
            return list;
        }
        public void InitOrUpdate()
        {
           
        }
        #region MyRegion
        ///// <summary>
        ///// 生成导航菜单
        ///// </summary>
        ///// <returns></returns>
        //public void InitOrUpdate()
        //{
        //    var navMenus = new List<NavMenu>();

        //    var rootMenus = _context.Menus
        //        .Where(s => string.IsNullOrEmpty(s.ParentId))
        //        .AsNoTracking()
        //        .OrderBy(s => s.IndexCode)
        //        .ToList();

        //    foreach (var rootMenu in rootMenus)
        //    {
        //        navMenus.Add(GetOneNavMenu(rootMenu));
        //    }

        //    NavMenus = navMenus;
        //}
        ///// <summary>
        ///// 根据给定的Menu，生成对应的导航菜单
        ///// </summary>
        ///// <param name="menu"></param>
        ///// <returns></returns>
        //public MenuInfo GetOneNavMenu(MenuInfo menu)
        //{
        //    //构建菜单项
        //    var navMenu = new NavMenu
        //    {
        //        TemplateUrl = menu.Id,
        //        Name = menu.Name,
        //        MenuType = menu.MenuType.Value,
        //        Url = menu.Url,
        //        Icon = menu.Icon
        //    };

        //    //构建子菜单
        //    var subMenus = _context.Menus
        //        .Where(s => s.ParentId == menu.Id)
        //        .AsNoTracking()
        //        .OrderBy(s => s.IndexCode)
        //        .ToList();

        //    foreach (var subMenu in subMenus)
        //    {
        //        navMenu.SubNavMenus.Add(GetOneNavMenu(subMenu));
        //    }

        //    return navMenu;
        //} 
        #endregion
    }
}
