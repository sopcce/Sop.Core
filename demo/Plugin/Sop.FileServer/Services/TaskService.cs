using Sop.FileServer.Base;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace Sop.FileServer.Services
{
    public class TaskService : IExecutable
    {
        public string Name
        {
            get
            {
                return "TaskService";
            }
        }
        public void Execute()
        {
            Console.WriteLine(string.Format("{0} 开始任务！", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            Console.WriteLine("本次任务执行完毕后自动关闭，请不要退出此程序");
            Console.WriteLine("Windows Service 运行中.....");
            Console.WriteLine();

            var servicesToRun = new ServiceBase[]
            {
                new Service1()
                
            };

            if (System.Environment.UserInteractive)
            {
                Console.WriteLine();
                MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (ServiceBase service in servicesToRun)
                {
                    Console.WriteLine("启动中 {0}... ", service.ServiceName);
                    onStartMethod.Invoke(service, new object[] { new string[] { } });
                    Console.WriteLine("已经启动");
                }
                Console.WriteLine();
                Console.WriteLine($"本次任务执行中！请不要关闭此窗口");
                Console.ReadKey();

                MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (ServiceBase service in servicesToRun)
                {
                    Console.WriteLine("关闭中 {0}... ", service.ServiceName);
                    onStopMethod.Invoke(service, null);
                    Console.WriteLine("已经关闭");
                }
            }
            else
            {
                ServiceBase.Run(servicesToRun);
            }

            Console.WriteLine();
            Console.WriteLine("本次任务执行完毕！30秒后自动关闭");
            Thread.Sleep(30000);
            Environment.Exit(0); //退出控制台


        }



    }
}
