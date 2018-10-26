﻿using ItemDoc.Services.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Mapping
{
    public class ItemsInfoMapping : ClassMapping<ItemsInfo>
    {
        public ItemsInfoMapping()
        {
            Table("item_items");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Native));
            Property(t => t.Name);
            Property(t => t.Description);
            Property(t => t.UserId);
            Property(t => t.PassWord);
            Property(t => t.ParentId);
            Property(t => t.ParentIdList);
            Property(t => t.LastUpdateTime);
            Property(t => t.ChildCount);
            Property(t => t.Depth);
            Property(t => t.Enabled);
            Property(t => t.DisplayOrder);
            Property(t => t.DateCreated);

        }
    }
}