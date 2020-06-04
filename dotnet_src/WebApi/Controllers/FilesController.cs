using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApi.Models.ApiResult;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Consumes("application/json", "multipart/form-data")]
    public class FilesController : ApiBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<List<FileUploadResult>>> Upload([FromForm]IFormCollection formFiles)
        {
            IFormFileCollection cols = Request.Form.Files;

            var apiResult = new ApiResult<List<FileUploadResult>>();
            apiResult.Code = Code.OK;
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var resultFile = new List<FileUploadResult>();
                foreach (var formFile in files)
                {
                    if (formFile != null)
                    {
                        var virtualRootPath = $"/upload/{DateTime.Now:yyyy/MM}";

                        var fileResult = new FileUploadResult();
                        fileResult.FileName = System.IO.Path.GetFileName(formFile.FileName);
                        fileResult.ContentType = formFile.ContentType;
                        fileResult.ContentLength = formFile.Length;
                        fileResult.Extension = System.IO.Path.GetExtension(formFile.FileName);
                        fileResult.FriendlyFileName = Guid.NewGuid().ToString("N") + fileResult.Extension;
                        fileResult.VirtualPath = $"{virtualRootPath}/{fileResult.FriendlyFileName}";
                        string fileName = System.IO.Path.GetFileName(formFile.FileName);

                        string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileResult.VirtualPath);

                        string saveRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, virtualRootPath);


                        if (!System.IO.Directory.Exists(saveRootPath))
                        {
                            System.IO.Directory.CreateDirectory(saveRootPath);
                        }

                        await using (var stream = System.IO.File.Create(savePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        resultFile.Add(fileResult);
                    }
                }
                apiResult.Code = Code.OK;
                apiResult.Message = "上传成功";
                apiResult.Data = resultFile;
            }
            else
            {
                apiResult.Code = Code.BAD_REQUEST;
                apiResult.Message = "请选择上传文件";

            }
            //TODO 目前不保存文件信息，后期需要再说

            return apiResult;
        }





    }
}