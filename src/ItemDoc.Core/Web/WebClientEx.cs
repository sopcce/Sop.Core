using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ItemDoc.Core.Web
{
    public class WebClientEx : WebClient
    {
        /// <summary>
        /// 毫秒单位
        /// </summary>
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = Timeout; 
                return request;
            }
            return null;
        }
    }
     
}
