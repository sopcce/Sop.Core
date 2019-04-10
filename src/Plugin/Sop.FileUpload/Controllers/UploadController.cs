using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sop.FileUpload.Helper;
using Sop.FileUpload.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sop.FileUpload.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        #region private
        private readonly SopFileUploadContext _context;

        public UploadController(SopFileUploadContext context)
        {
            _context = context;
        }
        #endregion

        [HttpPost()]
        [RequestSizeLimit(100_000_000)]
        //[DisableRequestSizeLimit]  //或者取消大小的限制
        public async Task<JsonResult> UploadFlie()
        {
            try
            {

                string data = Request.Query["data"];
                string token = Request.Query["token"];
                string date = Request.Query["-"];
                if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data) ||
                    string.IsNullOrWhiteSpace(date))
                {
                    return new JsonResult(new { status = 2 });
                }



                var path = PathString.FromUriComponent("/Image");

                var asdas = Request;

                var buffer = new byte[Convert.ToInt32(Request.ContentLength)];
                await Request.Body.ReadAsync(buffer, 0, buffer.Length);
                var body = Encoding.UTF8.GetString(buffer);


                if (Request.Body.CanRead)
                {

                    using (var writer = new StreamWriter(HttpContext.Response.Body))
                    {
                        await writer.WriteLineAsync(,);
                        
                    }

                    //using (Bitmap bitmap = new Bitmap(Request.Body))
                    //{ 
                    //    var returnFileInfo = await GetFilePathTask(); 
                    //    bitmap.Save(returnFileInfo.PhysicalFilePath);
                    //}
                }
            }
            catch (Exception ex)
            {

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
            return new JsonResult(new { status = 0, msg = "图片上传失败" });

        }

        public async Task<ReturnFileInfo> GetFilePathTask()
        {
            var fileserver = await _context.Fileserver.FindAsync(1);
            string upPath = fileserver.VirtualPath;
            string newFileName = Guid.NewGuid().ToString("N") + ".jpg";
            upPath = FileUtility.Format(newFileName, upPath);
            string diskPath = FileUtility.GetDiskFilePath(upPath) + newFileName;
            string diskPath1 = Path.Combine(Directory.GetCurrentDirectory(), upPath.Replace("~", ""), newFileName);


            string serverUrlPath = FileUtility.Combine(fileserver.RootPath, upPath.Replace("~", ""), newFileName);


            return await Task.FromResult<ReturnFileInfo>(new ReturnFileInfo()
            {
                PhysicalFilePath = diskPath,
                FilePath = upPath,
                FileName = newFileName,
                UrlFilePath = serverUrlPath
            });

        }






        // GET: v1/api/Upload
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fileserver>>> GetFileserver()
        {
            return await _context.Fileserver.ToListAsync();
        }

        // GET: api/Upload/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fileserver>> GetFileserver(int id)
        {
            var fileserver = await _context.Fileserver.FindAsync(id);

            if (fileserver == null)
            {
                return NotFound();
            }

            return fileserver;
        }

        // PUT: api/Upload/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileserver(int id, Fileserver fileserver)
        {
            if (id != fileserver.Id)
            {
                return BadRequest();
            }

            _context.Entry(fileserver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileserverExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Upload
        //[HttpPost]
        //public async Task<ActionResult<Fileserver>> PostFileserver(Fileserver fileserver)
        //{
        //    _context.Fileserver.Add(fileserver);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetFileserver", new { id = fileserver.Id }, fileserver);
        //}

        // DELETE: api/Upload/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Fileserver>> DeleteFileserver(int id)
        {
            var fileserver = await _context.Fileserver.FindAsync(id);
            if (fileserver == null)
            {
                return NotFound();
            }

            _context.Fileserver.Remove(fileserver);
            await _context.SaveChangesAsync();

            return fileserver;
        }

        private bool FileserverExists(int id)
        {
            return _context.Fileserver.Any(e => e.Id == id);
        }
    }


}
