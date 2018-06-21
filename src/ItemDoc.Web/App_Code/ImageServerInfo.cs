
using System.Collections.Generic;

namespace ItemDoc.Web.Controllers
{


  public partial class ImageInfo
  {
    public int Id { get; set; }
    public string ImageName { get; set; }
    public int ImageServerId { get; set; }

    public virtual ImageServerInfo ImageServerInfo { get; set; }
  }

  public partial class ImageServerInfo
  {
    public ImageServerInfo()
    {
      this.ImageInfo = new HashSet<ImageInfo>();
    }

    public int ServerId { get; set; }
    public string ServerName { get; set; }
    public string ServerUrl { get; set; }
    public string PicRootPath { get; set; }
    public int MaxPicAmount { get; set; }
    public int CurPicAmount { get; set; }
    public bool FlgUsable { get; set; }

    public virtual ICollection<ImageInfo> ImageInfo { get; set; }
  }
}