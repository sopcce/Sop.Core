using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using Sop.Core.Authorize;
using Sop.Domain.Service;
using Sop.Domain.VModel;
using System;
using System.Threading.Tasks;
using WebApi.Models.ApiResult;

namespace WebApi.Controllers
{
    /// <summary>
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }




        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public Task<ApiResult<LoginResult>> Login([FromBody] LoginVm model)
        {
          

            //bool isok = Captcha.ValidateCheckCode(model.CaptchaCode);
            //if (!isok)
            //{
            //    //验证码输入错误 
            //    return View(model);
            //}
            //var userInfo = _userService.PasswordSignIn(model.UserName, model.PassWord,model.RememberMe);

            var result = new ApiResult<LoginResult>();

            result.Code = Code.OK;

            string asdasdasd =  JwtTokenAuthorize.CreateToken(new JwtTokenVm() { UserName = model.UserName, Expires = DateTime.Now.AddDays(11) });
            result.Code

            return Task.FromResult(result); 
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}