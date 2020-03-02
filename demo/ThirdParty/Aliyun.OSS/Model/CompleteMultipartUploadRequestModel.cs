/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Xml.Serialization;

namespace Aliyun.OSS.Model
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("CompleteMultipartUpload")]
    public class CompleteMultipartUploadRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Part")]
        public CompletePart[] Parts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlRoot("Part")]
        public class CompletePart
        {
            /// <summary>
            /// 
            /// </summary>
            [XmlElement("PartNumber")]
            public int PartNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [XmlElement("ETag")]
            public string ETag { get; set; }
        }
    }
}
