using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sop.FileServer.Base
{
    public interface IApplication
    {
       
        string Name { get; set; }
       
       
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task"></param>
        void Execute(string task);

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="name"></param>
        void RegisterTask<T>(string key, string name) where T : IExecutable;
    }
}
