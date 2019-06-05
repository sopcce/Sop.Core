using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading;

namespace Sop.FileServer.Base
{
    public class BaseApplication : IApplication
    {

        private IDictionary<string, Type> _taskDic = new Dictionary<string, Type>();
        private IDictionary<string, string> _describes = new Dictionary<string, string>();
        private IDictionary<int, string> _deskey = new Dictionary<int, string>();


        /// <summary>
        /// 工具名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void Execute(string task)
        {
            if (!string.IsNullOrEmpty(task))
            {
                #region 单个任务
                if (_taskDic.ContainsKey(task.ToLower()))
                {
                    IExecutable exe = null;
                    try
                    {
                        Type type = _taskDic[task.ToLower()];
                        exe = System.Runtime.Serialization.FormatterServices
                              .GetUninitializedObject(type) as IExecutable;

                        Console.Title = string.Format("{0} - {1}", SystemName, _describes[task.ToLower()]);
                        exe.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (exe != null && exe is IDisposable) (exe as IDisposable).Dispose();
                    }
                }
                #endregion

                string[] tasks = task.Split('|');
                string tempTasks = string.Empty;
                for (int j = 0; j < tasks.Length; j++)
                {
                    int id = -1;
                    if (int.TryParse(tasks[j].ToLower(), out id))
                    {
                        //id = id > tasks.Length ? tasks.Length : id;
                        string value = string.Empty;
                        if (_deskey.TryGetValue(id, out value) && 
                            _taskDic.ContainsKey(value.ToLower()))
                        {
                            tempTasks = tempTasks + "|" + value.ToLower();
                        }
                    }
                    if (_taskDic.ContainsKey(tasks[j].ToLower()))
                    {
                        tempTasks = tempTasks + "|" + tasks[j].ToLower();
                    }
                }
                tempTasks = tempTasks.TrimStart('|');
                #region 执行多任务 
                if (tempTasks.Split('|').Length > 0)
                {
                    Executes(tempTasks);

                }
                #endregion

            }
            else
            {
                ShowTasks();
            }
            Console.WriteLine("任务结束..  请按任意键退出.");
            Console.ReadKey();

        }


        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public void RegisterTask<T>(string key, string name) where T : IExecutable
        {
            var type = typeof(T);
            key = key.ToLower();
            _taskDic.Add(key, type);
            _describes.Add(key, name);
            _deskey.Add(_deskey.Count + 1, key);
        }



        /// <summary>
        /// 显示任务
        /// </summary>
        protected virtual void ShowTasks()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(this.Name);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------------------------------");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("参数说明:");
            Console.WriteLine("多任务执行请用|分隔，如“任务A|任务B”");
            Console.WriteLine("多任务执行请用|分隔，如“1|2”");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------------------------------");

            Console.ForegroundColor = ConsoleColor.Green;

            int i = 1;
            foreach (var item in _describes)
            {
                Console.WriteLine($"--{i}-- {item.Key.ToLower()} : {item.Value} ");
                i++;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------------------------------");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("请选择:");

            Console.ForegroundColor = ConsoleColor.White;
            var task = Console.ReadLine();

            Execute(task);
        }


        /// <summary>
        /// 多任务执行
        /// </summary>
        /// <param name="taskStr"></param>
        public void Executes(string taskStr)
        {
            if (string.IsNullOrEmpty(taskStr))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("未发现配置任务");
                Console.ForegroundColor = ConsoleColor.Green;
                return;
            }
            string[] tasks = taskStr.Split('|');

            //初始化任务
            Thread[] threads = new Thread[tasks.Length];
            int i = 0;
            foreach (var task in tasks)
            {
                IExecutable exe = null;
                try
                {

                    var type = _taskDic[task.ToLower()] as Type;
                    exe = Activator.CreateInstance(type) as IExecutable;
                    if (exe != null)
                    {
                        threads[i] = new Thread(exe.Execute);
                        threads[i].Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n错误信息：" + ex.Message);
                }
                finally
                {
                    i++;
                    if (exe != null && exe is IDisposable) (exe as IDisposable).Dispose();
                }
            }
        }

    }

}
