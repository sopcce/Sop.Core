using System.Collections.Generic;
using Sop.Core.Models;
using Sop.Tools.Models;

namespace Sop.Tools.Services.Interfaces
{
    public interface INavMenuService
    {
        void InitOrUpdate();
        IList<MenuInfo> GetNavMenus();
    }
}
