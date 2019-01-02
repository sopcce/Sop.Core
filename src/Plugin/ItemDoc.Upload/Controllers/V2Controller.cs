using ItemDoc.Core.API;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Model;
using ItemDoc.Services.Parameter;
using ItemDoc.Services.Servers;
using Sop.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Script.Serialization;
using ItemDoc.Core.Mvc.SystemMessage;

namespace ItemDoc.Upload.Controllers
{
    /// <summary>
    /// Test
    /// </summary>
    public class V2Controller : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        public HttpResponseMessage Post()
        {
            try
            {


                string root = HostingEnvironment.MapPath("~/content/images/uploaded/fromwechart");
                if (!System.IO.Directory.Exists(root))
                    System.IO.Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                string OssFilename = "";

                //下面代码让异步变成同步, 因为要返回上传完图片之后新的图片绝对路径
                IEnumerable<HttpContent> parts = null;
                Task.Factory
                    .StartNew(() =>
                        {
                            parts = Request.Content.ReadAsMultipartAsync(provider)?.Result?.Contents;
                            foreach (MultipartFileData file in provider.FileData)
                            {
                                //Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                                //Trace.WriteLine("Server file path: " + file.LocalFileName);
                                var fileName = file.Headers.ContentDisposition.FileName;
                                OssFilename = System.Guid.NewGuid() + "." + fileName.Split('.')[1];
                                using (FileStream fs = new FileStream(file.LocalFileName, FileMode.Open))
                                {
                                    //PutImageFileToOss(fs, "image/" + fileName.Split('.')[1], OssFilename);

                                }
                                if (File.Exists(file.LocalFileName))//上传完删除
                                    File.Delete(file.LocalFileName);
                            }
                        },
                        CancellationToken.None,
                        TaskCreationOptions.LongRunning, // guarantees separate thread
                        TaskScheduler.Default)
                    .Wait();

                string picUrl = "http://";

              
                return SystemMessage.Result(new { code = "" });
            }
            catch (Exception ex)
            { 
                return SystemMessage.Result(new { Message = ex.Message }, HttpStatusCode.BadRequest);
            }

        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}