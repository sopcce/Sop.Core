 
using System.Threading.Tasks;

namespace Microsoft.Owin.Security.SinaWeibo
{
    public interface ISinaWeiboAuthenticationProvider
    {
        Task Authenticated(SinaWeiboAuthenticatedContext context);
        Task ReturnEndpoint(SinaWeiboReturnEndpointContext context);
    }
}
