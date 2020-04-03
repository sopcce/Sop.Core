using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using WebApi.Models.ApiResult;
using Sop.Core.Utility;

namespace WebApi.Controllers
{

    public class ArticleController : ApiBaseController
    {
        /// <summary>
        /// GetInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult<PageResult<ArticleListResult>>> GetList(int page = 1, int limit = 5)
        {
            var apiResult = new ApiResult<PageResult<ArticleListResult>>();

            var data = new List<ArticleListResult>();
            for (int i = 0; i < 1000; i++)
            {
                var cc = new Random().Next(i * -100, i * 100);
                data.Add(new ArticleListResult()
                {
                    id = i,
                    timestamp = DateTime.Now.AddMinutes(cc).ConvertTime(),
                    image_uri = "http://img.ivsky.com/Photo/UploadFiles/2009-3-2/2009030216510384101.jpg",
                    comment_disabled = i % 2 == 0 ? true : false,
                    content="ASDASDASDASDASDASDASD",
                    content_short="AAAAAAAAAAAAAAAAA", 
                    importance = 2,
                    author = $"屁小球{i}",
                    status=new string[]{ "published" },
                    type = new string[] { "CN" },

                }); 
            }


            apiResult.Data = new PageResult<ArticleListResult>()
            {
                Total = 1000,
                Items = data.Skip((page - 1) * limit).Take(limit).ToList(),

            };
            return Task.FromResult(apiResult);

        }

    }
}
