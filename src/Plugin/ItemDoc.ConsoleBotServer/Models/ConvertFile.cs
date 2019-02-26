using ItemDoc.Services.Model;

namespace ItemDoc.ConsoleBotServer.Models
{
    public class ConvertFile
    {
        public AttachmentInfo attachmentInfo { get; set; }

        public string FromPath { get; set; }

        public string ToPath { get; set; }

        public string FileName { get; set; } 
      
        public string Extension { get;  set; }
    }


}