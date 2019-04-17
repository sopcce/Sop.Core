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
        public long Id { get; set; }
        ///<Summary>
        /// Code 
        ///</Summary>
        public string Code { get; set; }
        ///<Summary>
        /// Name 
        ///</Summary>
        public string Name { get; set; }
        ///<Summary>
        /// Description 
        ///</Summary>
        public string Description { get; set; }
        ///<Summary>
        /// ParentCode 
        ///</Summary>
        public long ParentCode { get; set; }
        ///<Summary>
        /// ParentCodeList 
        ///</Summary>
        public string ParentCodeList { get; set; }
        ///<Summary>
        /// ChildCount 
        ///</Summary>
        public int ChildCount { get; set; }
        ///<Summary>
        /// Depth 
        ///</Summary>
        public int Depth { get; set; }
        ///<Summary>
        /// Enabled 
        ///</Summary>
        public int Enabled { get; set; }
        ///<Summary>
        /// DateCreated 
        ///</Summary>
        public DateTime DateCreated { get; set; }
        ///<Summary>
        /// Icon 
        ///</Summary>
        public string Icon { get; set; }
    }
}
