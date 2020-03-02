using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sop.Data;
using Sop.Data.Utility;
using Sop.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sop.Core.Models;
using Sop.Core.Utility;

namespace Sop.FileUpload.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        #region private
        private readonly SopContext _context;
     
        public UploadController(SopContext context)
        {
            _context = context;
        }
        #endregion

        [HttpPost()]
        [RequestSizeLimit(100_000_000)]
        [DisableRequestSizeLimit]  //或者取消大小的限制
        public async Task<IActionResult> UploadFlie()
        {
            try
            {

                string data = Request.Query["data"];
                string token = Request.Query["token"];
                string time = Request.Query["-"];
                if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data) ||
                    string.IsNullOrWhiteSpace(time))
                {
                    return new JsonResult(new { status = 2 });
                }
                //data
                //token = EncryptionUtility.Sha512Encode(data + time);
                //data = EncryptionUtility.AES_Decrypt(data, token);



                data = EncryptionUtility.AES_Decrypt(data, token);
                var info = data.FromJson<AttachmentInfo>();
                var datajson = data.ToJson();
                var newToken = EncryptionUtility.Sha512Encode(datajson + time);
                if (newToken != token)
                {
                    //return new JsonResult(new { status = 2 });
                }


                var fileserver = await _context.FileServer.FindAsync(1);
                string upPath = fileserver.VirtualPath;
                string newFileName = Guid.NewGuid().ToString("N") + info.Extension;
                upPath = FileUtility.Format(newFileName, upPath);
                string diskPath = FileUtility.GetDiskFilePath(upPath) + newFileName;
                string serverUrlPath = FileUtility.Combine(fileserver.RootPath, upPath.Replace("~", ""), newFileName);


                var fileInfo = new ReturnFileInfo()
                {
                    PhysicalFilePath = diskPath,
                    FilePath = upPath,
                    FileName = newFileName,
                    UrlFilePath = serverUrlPath
                };



                var path = PathString.FromUriComponent("/Image");

                var asdas = Request;

                if (Request.Body.CanRead)
                {
                    using (var stream = new FileStream(fileInfo.PhysicalFilePath, FileMode.Create))
                    {
                        await Request.Body.CopyToAsync(stream);
                    } 
                    return new JsonResult(new { status = 1, msg = "图片上传", path = fileInfo.UrlFilePath }); 
                }
            }
            catch (Exception ex)
            {

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
            return new JsonResult(new { status = 0, msg = "图片上传失败" });

        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileServerInfo>>> GetFileserver()
        {
            return await _context.FileServer.ToListAsync();
        }
         

        // GET: api/Upload/5
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<FileServerInfo>> GetFileserver(int id)
        {
            var fileserver = await _context.FileServer.FindAsync(id);

            if (fileserver == null)
            {
                return NotFound();
            }

            return fileserver;
        }

        // PUT: api/Upload/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileserver(int id, FileServerInfo fileserver)
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
        //public async Task<ActionResult<FileServerInfo>> PostFileserver(FileServerInfo fileserver)
        //{
        //    _context.FileServerInfo.Add(fileserver);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetFileserver", new { id = fileserver.Id }, fileserver);
        //}

        // DELETE: api/Upload/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FileServerInfo>> DeleteFileserver(int id)
        {
            var fileserver = await _context.FileServer.FindAsync(id);
            if (fileserver == null)
            {
                return NotFound();
            }

            _context.FileServer.Remove(fileserver);
            await _context.SaveChangesAsync();

            return fileserver;
        }

        private bool FileserverExists(int id)
        {
            return _context.FileServer.Any(e => e.Id == id);
        }
    }


}
