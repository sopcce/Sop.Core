/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Aliyun.OSS.Model
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("AccessControlPolicy")]
    public class AccessControlPolicy
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Owner")]
        public Owner Owner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("AccessControlList")]
        [XmlArrayItem("Grant")]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<string> Grants { get; set; }
    }
}
