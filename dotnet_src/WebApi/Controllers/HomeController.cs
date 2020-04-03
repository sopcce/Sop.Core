using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sop.Core.Api;
using WebApi.Models.ApiResult;


namespace WebApi.Controllers
{

    public class HomeController : ApiBaseController
    {



        [HttpGet]
        public Task<ApiResult<PageResult<OrderListResult>>> GetList(int page = 1, int limit = 5)
        {
            var apiResult = new ApiResult<PageResult<OrderListResult>>();

            var data = new List<OrderListResult>();
            for (int i = 0; i < 1000; i++)
            {
                var cc = new Random().Next(i * -100, i * 100);
                data.Add(new OrderListResult()
                {
                    order_no = Guid.NewGuid().ToString(),
                    timestamp = DateTime.Now.AddMinutes(cc).ConvertTime(),
                    price = float.Parse($"{cc}.{i}"),
                    status = "支付",
                    username = $"测试人员-{i}",
                });
            }
            apiResult.Data = new PageResult<OrderListResult>()
            {
                Total = 1000,
                Items = data.Skip((page - 1) * limit).Take(limit).ToList(),

            };
            return Task.FromResult(apiResult);

        }


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
                    drivesInfo.TotalSize = item.TotalSize;
                    drivesInfo.TotalFreeSpace = item.TotalFreeSpace;

                    


                } 
            }
            apiResult.Data = info;
            return Task.FromResult(apiResult);

        }

    }
}
