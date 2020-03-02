using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping;

namespace NHibernate.Cache.DynamicCacheBuster
{
    public delegate object GetRootClassHashInput(RootClass rootClass);
}
