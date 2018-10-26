using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary>
    //[SopTableName("Item_FileServer")]
    //[SopTablePrimaryKey("Id", AutoIncrement = true)]
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
