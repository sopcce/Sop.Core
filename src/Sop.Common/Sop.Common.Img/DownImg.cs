using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sop.Common.Img.Utility;

namespace Sop.Common.Img
{
  public class DownImg
  {
    //  using (WebClient my = new WebClient())
    //{
    //byte[] mybyte = my.DownloadData(imgpath);
    //MemoryStream ms = new MemoryStream(mybyte);
    //Image img = Image.FromStream(ms);
    //img.Save("", ImageFormat.Gif);
    //}




    /// <summary>
    /// 
    /// </summary>
    /// <param name="remotePath"></param>
    /// <param name="virtualPath"></param>
    /// <returns></returns>
    public string GetRemoteImg(string remotePath, string virtualPath)
    {
      if (string.IsNullOrEmpty(remotePath))
        return null;
      int imgNamele = remotePath.Length - remotePath.LastIndexOf("/", StringComparison.Ordinal);
      string imgName = remotePath.Substring(remotePath.LastIndexOf("/", StringComparison.Ordinal), imgNamele);

      return GetRemoteImg(remotePath, virtualPath, imgName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="remotePath"></param>
    /// <param name="virtualPath"></param>
    /// <param name="imgName"></param>
    /// <param name="imgExt"></param>
    /// <returns></returns>
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public string GetRemoteImg(string remotePath, string virtualPath = null, string imgName = null, string imgExt = null)
    {
      if (string.IsNullOrEmpty(remotePath))
        return null;

      if (string.IsNullOrEmpty(virtualPath))
      {
        virtualPath = "/";
      }

      string saveFilePath = FileUtility.GetDiskFilePath(virtualPath);
      if (!Directory.Exists(saveFilePath))
        Directory.CreateDirectory(saveFilePath);
      if (string.IsNullOrWhiteSpace(imgExt))
      {
        imgExt = remotePath.Substring(remotePath.LastIndexOf(".", StringComparison.Ordinal), remotePath.Length - remotePath.LastIndexOf(".", StringComparison.Ordinal));
      }
      if (string.IsNullOrWhiteSpace(imgName))
      {
        imgName = Guid.NewGuid() + imgExt;
      }


      try
      {
        WebRequest wreq = WebRequest.Create(remotePath);
        wreq.Timeout = 10000;
        HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
        using (var s = wresp.GetResponseStream())
        {
          if (s != null)
          {
            using (var img = Image.FromStream(s))
            {
              string imgPathAndName = saveFilePath + imgName;
              switch (imgExt.ToLower())
              {
                case ".gif":
                  img.Save(imgPathAndName, ImageFormat.Gif);
                  break;
                case ".jpg":
                case ".jpeg":
                  img.Save(imgPathAndName, ImageFormat.Jpeg);
                  break;
                case ".png":
                  img.Save(imgPathAndName, ImageFormat.Png);
                  break;
                case ".icon":
                  img.Save(imgPathAndName, ImageFormat.Icon);
                  break;
                case ".bmp":
                  img.Save(imgPathAndName, ImageFormat.Bmp);
                  break;
                default:
                  img.Save(imgPathAndName);
                  break; ;
              }

            }
          }
        }
        return virtualPath + imgName;
      }
      catch (Exception ex)
      {
        return null;
      }
    }









  }
}
