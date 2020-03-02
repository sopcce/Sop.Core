
using Microsoft.Owin.Security.Provider;
using System.Collections.Generic;

namespace Microsoft.Owin.Security.SinaWeibo
{
    public class SinaWeiboReturnEndpointContext : ReturnEndpointContext
    {
        public SinaWeiboReturnEndpointContext(
            IOwinContext context,
            AuthenticationTicket ticket)
            : base(context, ticket)
        {
        }
    }
}
