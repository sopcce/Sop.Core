using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Cache.DynamicCacheBuster
{
    public delegate void ChangeEventHandler(string oldCacheRegionName, string newCacheRegionName, string version);
}
