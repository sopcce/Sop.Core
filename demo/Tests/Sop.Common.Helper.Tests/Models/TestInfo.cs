using System;

namespace Sop.Common.Helper.Tests.Models
{
    [Serializable]
    public class TestInfo
    {
        public TestInfo()
        {
            Type = TestInfoType.Success;
            DateCreated = DateTime.Now;
        }

        /// <summary>
        /// Type
        /// </summary>		
        public TestInfoType Type { get; set; }

        /// <summary>
        /// IsDel
        /// </summary>		
        public bool IsDel { get; set; }

        /// <summary>
        /// Status
        /// </summary>		
        public bool Status { get; set; }

        /// <summary>
        /// LongValue
        /// </summary>		
        public long LongValue { get; set; }

        /// <summary>
        /// FloatValue
        /// </summary>		
        public float FloatValue { get; set; }

        /// <summary>
        /// DecimalValue
        /// </summary>		
        public decimal DecimalValue { get; set; }

        /// <summary>
        /// Body
        /// </summary>		
        public string Body { get; set; }

        /// <summary>
        /// DateCreated
        /// </summary>		
        public DateTime DateCreated { get; set; }


    }


   
}
