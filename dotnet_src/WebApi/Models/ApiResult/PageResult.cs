using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ApiResult
{
    public class PageResult<T>
    {
        public int Total { get; set; }


        public List<T> Items { get; set; }
    }
}
