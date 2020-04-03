using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using Sop.Domain.VModel;
using WebApi.Models.ApiResult.User;

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

    }
}