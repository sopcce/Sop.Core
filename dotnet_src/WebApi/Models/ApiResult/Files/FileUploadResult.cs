using System;

namespace WebApi.Models.ApiResult
{
   
    public class FileUploadResult
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string FriendlyFileName { get; set; }
        public string Extension { get; set; }
        public string VirtualPath { get; internal set; }
    }
}
