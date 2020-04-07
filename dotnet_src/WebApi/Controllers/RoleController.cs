using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using Sop.Domain.VModel;
using System;
using System.Threading.Tasks;
using WebApi.Models.ApiResult;

namespace WebApi.Controllers
{

    public class RoleController : ApiBaseController
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public Task<ApiResult<LoginResult>> Login([FromBody] LoginVm model)
        {
            var apiResult = new ApiResult<LoginResult>(); 
            
            return Task.FromResult(apiResult);
        }
        /// <summary>
        /// GetInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult<InfoResult>> GetInfo()
        {
            var apiResult = new ApiResult<InfoResult>();

            var data = new InfoResult();

            data.Userid = Guid.NewGuid().ToString();
            data.UserName = "guojiaqiu";
            data.Mobile = "guojiaqiu";
            data.Email = "15810803044";
            data.Roles = new string[] {
                    "admin"
                };
            apiResult.Data = data;
            return Task.FromResult(apiResult);

        }

        [HttpGet]
        public Task<ApiResult<RouteDataResult>> GetRoutes()
        {
            var apiResult = new ApiResult<RouteDataResult>();

            var data = new InfoResult();

            data.Userid = Guid.NewGuid().ToString();
            data.UserName = "guojiaqiu";
            data.Mobile = "guojiaqiu";
            data.Email = "15810803044";
            data.Roles = new string[] {
                    "admin"
                };
           
            return Task.FromResult(apiResult);

        }
        

    }
}