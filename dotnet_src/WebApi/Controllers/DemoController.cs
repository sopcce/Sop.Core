using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    public class DemoController : ApiBaseController
    {
        /// <summary>
        /// 这是一个带参数的get请求
        /// </summary>
        /// <remarks>
        /// 例子:
        /// Get api/Values/1
        /// </remarks>
        /// <param name="id">主键</param>
        /// <returns>测试字符串</returns> 
        /// <response code="201">返回value字符串</response>
        /// <response code="400">如果id为空</response>  
        // GET api/values/2
        [HttpGet("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<string> GetById(int id)
        {
            return $"你请求的 id 是 {id}";
        }
        /// <summary>
        ///  GET:   这是注释 summary
        /// </summary>
        /// <remarks>
        ///  GET:  这是注释 remarks
        /// </remarks>
        /// <returns>这是注释 returns </returns>
        /// <response code="200">这是注释 200 </response> 
        /// <response code="404">Superhero not foundd</response>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        ///  GET: api/Demo/5
        /// </summary>
        /// <param name="id"> id 这是注释</param>
        /// <remarks> 
        /// 这是注释 
        /// </remarks> 
        /// <returns>这是注释</returns>
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// POST: api/Demo 这是注释
        /// </summary>
        /// <param name="value">这是注释</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        /// <summary>
        /// 这是注释   PUT: api/Demo/5
        /// </summary>
        /// <param name="id">这是注释</param>
        /// <param name="value"> 这是注释</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
