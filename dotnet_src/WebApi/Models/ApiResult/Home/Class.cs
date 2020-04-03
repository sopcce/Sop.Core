using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ApiResult
{
    public class SystemConfigurationResult
    {
        public string OSDescription { get; set; }
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
