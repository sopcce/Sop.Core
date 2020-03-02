using Dapper;
using MySql.Data.MySqlClient;
using Serilog;
using Sop.FileServer.Base;
using Sop.FileServer.Common;
using Sop.FileServer.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sop.FileServer
{
    public class Program
    {
        
        public static IDbConnection conn = new MySqlConnection("server=127.0.0.1;port=3306;database=sopcce;uid=root;pwd=123456;Charset=utf8mb4;SslMode = none;");
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("log.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
            Log.Information("Hello, Serilog!");
            Log.CloseAndFlush();

            var app = new BaseApplication { Name = "论坛贴吧数据分析系统" };
            Register(app);
            app.Execute(args.Length == 0 ? null : args[0]);


           
            Console.ForegroundColor=ConsoleColor.Blue;
            Console.WriteLine("*********************************");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.ReadKey();

        } 
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        static void Register(IApplication app)
        {
            app.RegisterTask<TaskDemo>("TaskDemo", "TaskDemo");
            app.RegisterTask<TaskService>("TaskService", "TaskService");

        }

    }
}
