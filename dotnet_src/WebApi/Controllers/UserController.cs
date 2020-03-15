using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using Sop.Core.Authorize;
using Sop.Domain.Service;
using Sop.Domain.VModel;
using System;
using System.Threading.Tasks;

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
        public Task<ApiResult<string>> Login([FromBody] LoginVm model)
        {
            var apiResult = new ApiResult<string>();

            //bool isok = Captcha.ValidateCheckCode(model.CaptchaCode);
            //if (!isok)
            //{
            //    //验证码输入错误 
            //    return View(model);
            //}
            //var userInfo = _userService.PasswordSignIn(model.UserName, model.PassWord,model.RememberMe);

            string asdasdasd =  JwtTokenAuthorize.CreateToken(new JwtTokenVm() { UserName = "asdasd", Expires = DateTime.Now.AddDays(11) });

            var sdas = JwtTokenAuthorize.ReadToken(asdasdasd);

            //var user = _userService.Authenticate(model.UserName, model.PassWord);

            //if (user == null)
            //    return BadRequest(new {message = "Username or password is incorrect"});

            return Task.FromResult(apiResult);
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