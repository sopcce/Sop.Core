using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Services.Parameter
{
  public class ImgServerParameter
  {
    public string ServerId { get; set; }
    public string ServerUrl { get; set; }

    public string ServerName { get; set; }

    public string ServerEnName { get; set; }

    public string OwnerId { get; set; }

    public string Token { get; set; }

    public string Key { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public string ContentType { get; set; }
    public int ContentLength { get; set; }
    public long Date { get; set; }
    public string IP { get; set; }
    public string VirtualPath { get; set; }
  }
}
