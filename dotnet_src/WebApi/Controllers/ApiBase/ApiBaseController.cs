using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Sop.Core.Api;
using Sop.Core.Authorize;
using System;
using System.Linq;

namespace WebApi.Controllers
{


    [ApiAuthorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class ApiBaseController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly string _token = "token";
        public override void OnActionExecuting(ActionExecutingContext requestContext)
        {
            var result = new ApiResult<string>();
            result.Code = Code.UNAUTHORIZED;
            result.Message = "授权失败";

            HttpContext httpContext = requestContext.HttpContext;
            if (requestContext.Filters.Any(f => f is IAllowAnonymousFilter))
            {
                return;
            }
            var key = httpContext.Request.Headers.Keys;
            if (httpContext.Request.Headers.ContainsKey(_token))
            {
                try
                {
                    var tokenHeader = httpContext.Request.Headers[_token].ToString();
                    if (!string.IsNullOrWhiteSpace(tokenHeader))
                    {
                        //解析token
                        var info = JwtTokenAuthorize.ReadToken(tokenHeader);
                        if (info != null)
                        {

                            //获取用户是否存在

                            //判断是否有权限访问


                            //增加缓存



                            //返回结果 

                        }
                    }
                }
                catch (Exception)
                {
                    //  Logger.Error($"{DateTime.Now} middleware wrong:{ex.Message}");
                }

            }


            base.OnActionExecuting(requestContext);
        }

    }
}