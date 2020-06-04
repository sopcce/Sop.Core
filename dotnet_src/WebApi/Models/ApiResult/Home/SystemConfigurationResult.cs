using System.Collections.Generic;
using System.IO;

namespace WebApi.Models.ApiResult
{
    /// <summary>
    /// 获取系统运行信息
    /// </summary>
    public class SystemConfigurationResult
    {
        /// <summary>
        /// 获取描述应用程序所运行的操作系统的字符串。
        /// </summary>
        public string OSDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FrameworkDescription { get; set; }
        public string OSArchitecture { get; set; }
        public string ProcessArchitecture { get; set; }
        public string SystemVersion { get; set; }
        public string RuntimeDirectory { get; set; }
        public bool Is64BitOperatingSystem { get; set; }
        public long WorkingSet { get; set; }
        public string UserDomainName { get; set; }
        public string UserName { get; set; }
        public bool UserInteractive { get; set; }
        public string MachineName { get; set; }
        public string OSVersion { get; set; }
        public string CurrentDirectory { get; set; }

        public string SystemDirectory { get; set; }
        public string[] LogicalDrives { get; set; }
        public List<LogicalDrivesInfo> DrivesList { get; set; } = new List<LogicalDrivesInfo>();
    }
    /// <summary>
    /// 
    /// </summary>
    public class LogicalDrivesInfo
    {
        public string Name { get; set; }
        public DriveType DriveType { get; set; }
        public long TotalSize { get; set; }
        public long TotalFreeSpace { get; set; }
        public string TotalSizeFriendlyFileSize { get; set; }
        public string TotalFreeSpaceFriendlyFileSize { get; set; } 
        public string AvailableFreeSpaceFriendlyFileSize { get; set; }
        public long AvailableFreeSpace { get; set; }
    }
}
