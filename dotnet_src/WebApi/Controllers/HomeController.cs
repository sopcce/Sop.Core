using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApi.Models.ApiResult;


namespace WebApi.Controllers
{

    public class HomeController : ApiBaseController
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult<PageResult<OrderListResult>>> GetList(int page = 1, int limit = 10)
        {
            var apiResult = new ApiResult<PageResult<OrderListResult>>();

            var data = new List<OrderListResult>();
            for (int i = 0; i < 1000; i++)
            {
                var cc = new Random().Next(i * -100, i * 100);
                data.Add(new OrderListResult()
                {
                    OrderNo = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.Now.AddMinutes(cc).ConvertTime(),
                    Price = decimal.Parse($"{cc}.{i}"),
                    Status = "支付",
                    Username = $"测试人员-{i}",
                });
            }
            apiResult.Data = new PageResult<OrderListResult>()
            {
                Total = 1000,
                Items = data.Skip((page - 1) * limit).Take(limit).ToList(),

            };
            return Task.FromResult(apiResult);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult<SystemConfigurationResult>> GetSysInfo()
        {
            var apiResult = new ApiResult<SystemConfigurationResult>();
            var info = new SystemConfigurationResult();

            info.OSDescription = RuntimeInformation.OSDescription;
            info.FrameworkDescription = RuntimeInformation.FrameworkDescription;
            info.OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
            info.ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
            info.SystemVersion = RuntimeEnvironment.GetSystemVersion();
            info.RuntimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            info.Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
            info.WorkingSet = Environment.WorkingSet;
            info.UserDomainName = Environment.UserDomainName;
            info.UserName = Environment.UserName;
            info.UserInteractive = Environment.UserInteractive;
            info.MachineName = Environment.MachineName;
            info.OSVersion = Environment.OSVersion.VersionString;
            info.CurrentDirectory = Environment.CurrentDirectory;
            info.SystemDirectory = Environment.SystemDirectory;
            info.LogicalDrives = Environment.GetLogicalDrives();



            var allDirves = DriveInfo.GetDrives();
            //检索计算机上的所有逻辑驱动器名称
            foreach (DriveInfo item in allDirves)
            {
                var drivesInfo = new LogicalDrivesInfo();
                drivesInfo.Name = item.Name;
                drivesInfo.DriveType = item.DriveType;
                if (item.IsReady)
                {
                    drivesInfo.TotalSize = item.TotalSize;
                    drivesInfo.TotalSizeFriendlyFileSize = item.TotalSize.ToFriendlyFileSize();
                    drivesInfo.TotalFreeSpace = item.TotalFreeSpace;
                    drivesInfo.TotalFreeSpaceFriendlyFileSize = item.TotalFreeSpace.ToFriendlyFileSize();
                    drivesInfo.AvailableFreeSpace = item.AvailableFreeSpace;
                    drivesInfo.AvailableFreeSpaceFriendlyFileSize = item.AvailableFreeSpace.ToFriendlyFileSize();  

                }
                info.DrivesList.Add(drivesInfo);
            }
            apiResult.Data = info;
            return Task.FromResult(apiResult);

        }

      
         

}
}
