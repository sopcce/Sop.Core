using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sop.Core.Models
{
    [Table("sop_citys")]
    public abstract class CitysInfo
    {

        ///<Summary>
        /// Id 
        ///</Summary>
        public virtual long Id { get; set; }
        ///<Summary>
        /// Code 
        ///</Summary>
        public virtual string Code { get; set; }
        ///<Summary>
        /// Name 
        ///</Summary>
        public virtual string Name { get; set; }
        ///<Summary>
        /// Description 
        ///</Summary>
        public virtual string Description { get; set; }
        ///<Summary>
        /// ParentCode 
        ///</Summary>
        public virtual long ParentCode { get; set; }
        ///<Summary>
        /// ParentCodeList 
        ///</Summary>
        public virtual string ParentCodeList { get; set; }
        ///<Summary>
        /// ChildCount 
        ///</Summary>
        public virtual int ChildCount { get; set; }
        ///<Summary>
        /// Depth 
        ///</Summary>
        public virtual int Depth { get; set; }
        ///<Summary>
        /// Enabled 
        ///</Summary>
        public virtual int Enabled { get; set; }
        ///<Summary>
        /// DateCreated 
        ///</Summary>
        public virtual DateTime DateCreated { get; set; }
        ///<Summary>
        /// Icon 
        ///</Summary>
        public virtual string Icon { get; set; }
    }
}
