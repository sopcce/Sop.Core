using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Cache.DynamicCacheBuster
{
    using Collection = NHibernate.Mapping.Collection;

    public delegate object GetCollectionHashInput(Collection collection);
}
