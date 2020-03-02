using Sop.Framework.Repositories;
using Sop.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sop.Services.Servers
{
    public class ItemsService
    {
        public IRepository<ItemsInfo> _ItemRepository { get; set; }

        public IEnumerable<ItemsInfo> GetAll()
        {
            return _ItemRepository.Table;
        }




        /// <summary>
        /// Gets the by userid.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        public ItemsInfo GetById(int primaryKey)
        {
            return _ItemRepository.Get(primaryKey);
        }



        /// <summary>
        /// Inserts the specified information.
        /// </summary>
        /// <param name="info">The information.</param>
        public void Insert(ItemsInfo info)
        {
            _ItemRepository.Create(info);
            info.DisplayOrder = info.Id;
            this.Update(info);
        }
        /// <summary>
        /// Updates the specified information.
        /// </summary>
        /// <param name="info">The information.</param>
        public void Update(ItemsInfo info)
        {
            _ItemRepository.Update(info);
        }

        public void Delete(ItemsInfo info)
        {
            _ItemRepository.Delete(info);
        }
    }

}
