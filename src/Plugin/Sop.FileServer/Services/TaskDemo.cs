using Sop.FileServer.Base;
using System;
using System.Threading;

namespace Sop.FileServer.Services
{
    public class TaskDemo : IExecutable
    {
        public string Name
        {
            get
            {
                return "TaskDemo";
            }
        }
        public void Execute()
        {
            Console.WriteLine(string.Format("{0} 开始任务！", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            Console.WriteLine("本次任务执行完毕后自动关闭，请不要退出此程序");
            Console.WriteLine();
            


            Console.WriteLine();
            Console.WriteLine("本次任务执行完毕！30秒后自动关闭");
            Thread.Sleep(30000);
            Environment.Exit(0); //退出控制台


        }


         
    }
}
