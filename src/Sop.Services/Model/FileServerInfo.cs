using System;
using Sop.Framework.Repositories;

namespace Sop.Services.Model
{
    /// <summary>
    /// 文件服务器实体
    /// </summary>
    public class FileServerInfo
    {

        public virtual int Id { get; set; }

        public virtual string ServerId { get; set; }
        public virtual string ServerName { get; set; }

        public virtual string ServerEnName { get; set; }
        public virtual string ServerUrl { get; set; }
        public virtual string RootPath { get; set; }
        public virtual string VirtualPath { get; set; }
        public virtual string DiskPath { get; set; }


        public virtual long MaxAmount { get; set; }
        public virtual long CurAmount { get; set; }

        public virtual long Size { get; set; }
              
        public virtual bool Enabled { get; set; }
               
        public virtual int DisplayOrder { get; set; }
               
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
    }





}
