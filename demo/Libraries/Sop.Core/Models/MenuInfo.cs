using System.Collections.Generic;

namespace Sop.Core.Models
{
    public class MenuInfo
    {  
        public string AspArea { get; set; }
        public string AspController { get; set; }
        public string AspAction { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool UserMvcRoute { get; set; } = false; 
        /// <summary>
        /// 子菜单
        /// </summary>
        public IList<MenuInfo> SubNavMenus { get; set; } 
    }

 
}
