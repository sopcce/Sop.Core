using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sop.FileUpload.Models
{
    [Table("item_fileserver")]
    public class Fileserver
    {
        ///<Summary>
        /// Id 
        ///</Summary>
        public virtual int Id { get; set; }
        ///<Summary>
        /// ServerId 
        ///</Summary>
        public virtual string ServerId { get; set; }
        ///<Summary>
        /// ServerName 
        ///</Summary>
        public virtual string ServerName { get; set; }
        ///<Summary>
        /// ServerEnName 
        ///</Summary>
        public virtual string ServerEnName { get; set; }
        ///<Summary>
        /// ServerUrl 
        ///</Summary>
        public virtual string ServerUrl { get; set; }
        ///<Summary>
        /// RootPath 
        ///</Summary>
        public virtual string RootPath { get; set; }
        ///<Summary>
        /// VirtualPath 
        ///</Summary>
        public virtual string VirtualPath { get; set; }
        ///<Summary>
        /// DiskPath 
        ///</Summary>
        public virtual string DiskPath { get; set; }
        ///<Summary>
        /// MaxAmount 
        ///</Summary>
        public virtual long MaxAmount { get; set; }
        ///<Summary>
        /// CurAmount 
        ///</Summary>
        public virtual long CurAmount { get; set; }
        ///<Summary>
        /// Size 
        ///</Summary>
        public virtual long Size { get; set; }
        ///<Summary>
        /// Enabled 
        ///</Summary>
        public virtual int Enabled { get; set; }
        ///<Summary>
        /// DateCreated 
        ///</Summary>
        
        public virtual DateTime DateCreated { get; set; }
        ///<Summary>
        /// DisplayOrder 
        ///</Summary>
        public virtual int DisplayOrder { get; set; }
    }
}