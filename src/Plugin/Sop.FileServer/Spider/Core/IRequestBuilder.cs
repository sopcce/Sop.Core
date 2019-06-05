using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sop.FileServer.Spider.Core
{
    public interface IRequestBuilder
    {
        /// <summary>
        /// 构造起始链接对象并添加到网站信息对象中
        /// </summary>
        IEnumerable<Request> Build();
    }
}
