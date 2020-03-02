
using System;
using System.Threading.Tasks;

namespace Microsoft.Owin.Security.SinaWeibo
{
    public class SinaWeiboAuthenticationProvider : ISinaWeiboAuthenticationProvider
    {
        public SinaWeiboAuthenticationProvider()
        {
            OnAuthenticated = (context) => Task.FromResult<Task>(null);
            OnReturnEndpoint = (context) => Task.FromResult<Task>(null);
        }

        public Func<SinaWeiboAuthenticatedContext, Task> OnAuthenticated { get; set; }

        public Func<SinaWeiboReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

        public Task Authenticated(SinaWeiboAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }

        public Task ReturnEndpoint(SinaWeiboReturnEndpointContext context)
        {
            return OnReturnEndpoint(context);
        }
    }
}
