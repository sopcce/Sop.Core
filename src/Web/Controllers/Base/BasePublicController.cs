using System.Web.Mvc;

namespace Sop.Web.Controllers
{

    public abstract partial class BasePublicController : BaseController
    {
        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
       
            return new EmptyResult();
        }

    }
}