using Microsoft.AspNetCore.Authentication.QQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using Sop.Core.Authorize;
using Sop.Domain.Service;
using Sop.Domain.VModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.ApiResult.User;

namespace WebApi.Controllers
{
    /// <summary>
    /// UserController
    /// </summary>

    public class UserController : ApiBaseController
    {
        #region UserController
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        } 
        #endregion
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
            var data = new LoginResult();
            apiResult.Code = Code.OK;


            //bool isok = Captcha.ValidateCheckCode(model.CaptchaCode);
            //if (!isok)
            //{
            //    //验证码输入错误 
            //    return View(model);
            //}
            //var userInfo = _userService.PasswordSignIn(model.UserName, model.PassWord,model.RememberMe);

            string token = JwtTokenAuthorize.CreateToken(new JwtTokenVm()
            {
                UserName = model.UserName,
                Expires = DateTime.Now.AddDays(11),
                Role = new string[] {
                    "admin","people"
                }
            });

            //var sdas = JwtTokenAuthorize.ReadToken(token);

            //var user = _userService.Authenticate(model.UserName, model.PassWord);

            //if (user == null)
            //    return BadRequest(new {message = "Username or password is incorrect"});

            data.Userid = Guid.NewGuid().ToString();
            data.Username = "guojiaqiu";
            data.Mobile = "guojiaqiu";
            data.Email = "15810803044";
            data.Role = string.Join(",", new string[] {
                    "admin","people"
                });
            data.Token = token;
            apiResult.Data = data;

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
        /// <summary>
        /// Logout
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResult<string>> Logout()
        {
            var apiResult = new ApiResult<string>();
            apiResult.Code = Code.OK;

            return Task.FromResult(apiResult);
        } 
        /// <summary>
        /// Signin
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{type}")]
        public IActionResult Signin(string type, string code, string redirect)
        {


            return Redirect(redirect);

        }
        /// <summary>
        /// SearchUser
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult SearchUser(string type, string code, string redirect)
        {


            return Redirect(redirect);

        }
         
    }
}