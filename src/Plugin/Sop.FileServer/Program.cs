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
        //server=127.0.0.1;port=3306;database=sopcce;uid=root;pwd=123456;Charset=utf8mb4;SslMode = none;
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

            //var app = new BaseApplication { Name = "论坛贴吧数据分析系统" };
            //Register(app);
            //app.Execute(args.Length == 0 ? null : args[0]);


            //var proxy = new ProxyValidator();
            //var asdasd = proxy.IsAvailable(new WebProxy("34.80.63.3", 1024));
            //asdasd = proxy.IsAvailable(new WebProxy("117.93.138.15", 53281));
            //asdasd = proxy.IsAvailable(new WebProxy("49.84.151.169	", 53281));
            //DB();
            //GetCity("0");
            GetCity1("0");
            Console.ForegroundColor=ConsoleColor.Blue;
            Console.WriteLine("*********************************");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.ReadKey();

        }
        
        private static string GetCity1(string parentId)
        {

            string url = $"http://www.mca.gov.cn/article/sj/xzqh/2019/201901-06/201902061025.html";
            var html = HttpHelper.HttpGet(url);
            if (html == null)
            {
                return null;

            }
            return null;
        }
        private static string GetCity(string parentId)
        {
             
            string url = $"http://api.yunrunyuqing.com/api/place?parentId={parentId}";
            var html = HttpHelper.HttpGet(url);
            if (html == null)
            {
                return null;

            }
            var json = html.FromJson<Info>();
            if (json == null)
            {
                return null;

            }
            //对JSON数据存储到数据库
            var data = json.data;
            if (data == null)
            {
                return null;

            }
            if (data != null & data.Count > 0)
            {
                foreach (var datainfo in data)
                {
                    var info = conn.QueryFirstOrDefault<string>("SELECT * FROM `sop_citys` where Name=@name ", new { name = datainfo.Name });
                    if (!string.IsNullOrWhiteSpace(info))
                    {
                        var id = conn.Execute("update sop_citys set Isnew=0,psid=@psid,sid=@sid where Name=@name  ", new
                        {
                            sid = datainfo.ID,
                            psid = datainfo.ParentID,
                            name = datainfo.Name

                        });
                        Console.WriteLine(id > 0 ? "" + datainfo.Name : "失败了:" + datainfo.Name);
                    }
                    else
                    {
                        var id = conn.Execute(" INSERT INTO `sop_citys`(`Code`, `Name`, `Description`, `ParentCode`, `ParentCodeList`, `ChildCount`, `Depth`, `Enabled`, `DateCreated`, `Icon`, `IsNew`, `psid`, `sid`) VALUES (0,@name, NULL, 0, \',86,\', 0, @Layer, 1, @date, NULL, NULL,@psid,@sid); ", new
                        {
                            sid = datainfo.ID,
                            psid = datainfo.ParentID,
                            name = datainfo.Name,
                            Layer = datainfo.Layer,
                            date = DateTime.Now
                        });
                        Console.WriteLine(id > 0 ? "" + datainfo.Name : "失败了:" + datainfo.Name);


                    }
                     GetCity(datainfo.ID);
                }
            }
            return null;
        }

        public static void DB()
        {
            var info = conn.Query<SopCitysInfo>("SELECT * FROM `sop_citys`").ToList();
            var count = info.Count();
            foreach (var dataCitysInfo in info)
            {
                var code = dataCitysInfo.Code;
                if (code.ToString().Substring(2, 4) == "0000")
                {

                    var id = conn.Execute("update sop_citys set ParentCode=86,ParentCodeList=',86,',Depth=1 where id=@id ", new
                    {
                        pcode = code.ToString().Substring(0, 2),
                        id = dataCitysInfo.Id
                    });
                    Console.WriteLine(id > 0 ? "" + dataCitysInfo.Id : "失败了" + dataCitysInfo.Id);
                }
                else
                {
                    if (code.ToString().Substring(4, 2) == "00")
                    {

                        var id = conn.Execute("update sop_citys set ParentCode=@pcode,ParentCodeList=@pcode2,Depth=2 where id=@id ", new
                        {
                            pcode = code.ToString().Substring(0, 2) + "0000",
                            pcode2 = ",86," + code.ToString().Substring(0, 2) + "0000,",
                            id = dataCitysInfo.Id
                        });
                        Console.WriteLine(id > 0 ? "" + dataCitysInfo.Id : "失败了" + dataCitysInfo.Id);
                    }
                    else
                    {
                        var id = conn.Execute("update sop_citys set ParentCode=@pcode,ParentCodeList=@pcode2,Depth=3 where id=@id ", new
                        {
                            pcode = code.ToString().Substring(0, 4) + "00",
                            pcode2 = ",86," + code.ToString().Substring(0, 2) + "0000," + code.ToString().Substring(0, 4) + "00,",
                            id = dataCitysInfo.Id
                        });
                        Console.WriteLine(id > 0 ? "" + dataCitysInfo.Id : "失败了" + dataCitysInfo.Id);


                    }

                }
                //修正Childcount


            }


        }
        [Serializable]
        public class SopCitysInfo
        {


            ///<Summary>
            /// Id 
            ///</Summary>
            public virtual long Id { get; set; }
            ///<Summary>
            /// Code 
            ///</Summary>
            public virtual long Code { get; set; }
            ///<Summary>
            /// Name 
            ///</Summary>
            public virtual string Name { get; set; }
            ///<Summary>
            /// Description 
            ///</Summary>
            public virtual string Description { get; set; }
            ///<Summary>
            /// ParentCode 
            ///</Summary>
            public virtual long Parentcode { get; set; }
            ///<Summary>
            /// ParentCodeList 
            ///</Summary>
            public virtual string Parentcodelist { get; set; }
            ///<Summary>
            /// ChildCount 
            ///</Summary>
            public virtual int Childcount { get; set; }
            ///<Summary>
            /// Depth 
            ///</Summary>
            public virtual int Depth { get; set; }
            ///<Summary>
            /// Enabled 
            ///</Summary>
            public virtual int Enabled { get; set; }
            ///<Summary>
            /// DateCreated 
            ///</Summary>
            public virtual DateTime Datecreated { get; set; }
            ///<Summary>
            /// Icon 
            ///</Summary>
            public virtual string Icon { get; set; }
            ///<Summary>
            /// IsNew 
            ///</Summary>
            public virtual string Isnew { get; set; }
            public virtual string psid { get; set; }
            public virtual string sid { get; set; }

        }
        public class Info
        {
            public int status { get; set; }
            public string msg { get; set; }
            public List<Data> data { get; set; }

        }
        public class Data
        {
            public string ID { get; set; }
            public int Layer { get; set; }

            public string Name { get; set; }
            public string ParentID { get; set; }
            public string Word { get; set; }

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
