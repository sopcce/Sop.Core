using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Core.Mvc
{
    /// <summary>
    /// 提示消息的类别
    /// </summary>
    public enum SystemMessageType
    {

        /// <summary>
        /// 默认样式 无图标
        /// </summary>
        Default = -1,
        /// <summary>
        /// 默认样式 叹号 
        /// </summary>
        Info = 0,
        /// <summary>
        /// 成功 对号
        /// </summary>
        Success = 1,
        /// <summary>
        /// 错误 叉号
        /// </summary>
        Error = 2,
        /// <summary>
        /// 询问  问号
        /// </summary>
        Ask = 3,

    }
}
