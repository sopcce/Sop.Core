using System.Collections.Generic;

namespace ItemDoc.Upload.Models
{
  public partial class UploadServerInfo
  {
        public UploadServerInfo()
        {
            this.FileInfo = new HashSet<ImageInfo>();
        }
    
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerUrl { get; set; }
        public string PicRootPath { get; set; }
        public int MaxPicAmount { get; set; }
        public int CurPicAmount { get; set; }
        public bool FlgUsable { get; set; }
    
        public virtual ICollection<ImageInfo> FileInfo { get; set; }
    }
}
