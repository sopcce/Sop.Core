
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sop.Framework.SystemLog
{
    /// <summary>
    /// 日志类型
    /// 控制级别,(高) OFF > FATAL > ERROR > WARN > INFO > DEBUG > ALL (全部记录)
    /// </summary>
    public enum LogLevel
    {
       
        /// <summary>
        /// 调试
        /// </summary>
        Debug,
        /// <summary>
        /// 信息
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal,
      
    }
}
