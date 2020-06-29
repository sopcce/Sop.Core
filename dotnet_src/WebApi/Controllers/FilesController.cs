using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using Sop.Core;
using WebApi.Models;
using WebApi.Models.ApiResult;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Consumes("application/json", "multipart/form-data")]
    public class FilesController : ApiBaseController
    {
        private readonly IShortUrlService _shortUrlService;
        public FilesController(IShortUrlService shortUrlService)
        {
            _shortUrlService = shortUrlService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<List<FileUploadResult>>> Upload([FromForm] IFormCollection formFiles)
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


        [HttpGet("/api/captcha")]
        public IActionResult Captcha()
        {
            var bytes = CaptchaHelper.GetCaptcha();
            return File(bytes, "image/png");
        }
        /// <summary>  
        /// 该方法是将生成的随机数写入图像文件  
        /// </summary>  
        /// <param name="code">code是一个随机数</param>
        /// <param name="numbers">生成位数（默认4位）</param>  
        [HttpGet]
        public FileStream IdentifyingCode([FromQuery] int numbers = 4)
        {
            var tempTTFName = $"{DateTime.Now.Ticks}.ttf";
            System.IO.File.Copy("hyqh.ttf", tempTTFName);
            var tempCodeName = $"{DateTime.Now.Ticks}.jpg";
            var code = CaptchaHelper.RndNum(numbers);
            Random random = new Random();
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //创建画布
            var imageWidth = code.Length * 18;
            using (Image image = new Image(imageWidth, 32))
            using (var pixels = image.Lock())
            using (FileStream output = System.IO.File.OpenWrite(tempCodeName))
            {
                //背景设为白色  
                pixels[imageWidth, 32] = Color.White;

                //向画板中绘制贝塞尔样条  
                for (int i = 0; i < 2; i++)
                {
                    var p1 = new Vector2(0, random.Next(image.Height));
                    var p2 = new Vector2(random.Next(image.Width), random.Next(image.Height));
                    var p3 = new Vector2(random.Next(image.Width), random.Next(image.Height));
                    var p4 = new Vector2(image.Width, random.Next(image.Height));
                    Vector2[] p = { p1, p2, p3, p4 };
                    Color clr = color[random.Next(color.Length)];
                    Pen pen = new Pen(clr, 1);
                    image.Mutate(pen, p);
                }
                //画噪点
                for (int i = 0; i < 50; i++)
                {
                    GraphicsOptions noneDefault = new GraphicsOptions();
                    SixLabors.ImageSharp.Rectangle rectangle = new Rectangle(random.Next(image.Width), random.Next(image.Height), 1, 1);
                    image.Frames.AddFrame(Color.Gray, 1f, rectangle, noneDefault);
                }
                //画验证码字符串 
                for (int i = 0; i < code.Length; i++)
                {
                    int cindex = random.Next(7);//随机颜色索引值  
                    int findex = random.Next(5);//随机字体索引值  
                    var fontCollection = new FontCollection();
                    var fontTemple = fontCollection.Install(tempTTFName);
                    var font = new Font(fontTemple, 16);
                    var brush = new SolidBrush(color[cindex]);//颜色  
                    //var textColor = color[cindex];//颜色  
                    int ii = 4;
                    if ((i + 1) % 2 == 0)//控制验证码不在同一高度  
                    {
                        ii = 2;
                    }
                    image.DrawText(code.Substring(i, 1), font, brush, new System.Numerics.Vector2(3 + (i * 12), ii));//绘制一个验证字符  

                }
                image.Save(output);
            }
            if (System.IO.File.Exists(tempTTFName))
            {
                System.IO.File.Delete(tempTTFName);
            }
            return System.IO.File.OpenRead(tempCodeName);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        [HttpGet("/api/shortUrl/{code}/{shortUrl}")]
        public ApiResult<string> ShortUrl(string code, string shortUrl)
        {
            var result = new ApiResult<string>();

            var data = _shortUrlService.GetShortUrl(shortUrl);
            result.Data = data[0];
            return result;
        }
    }


}