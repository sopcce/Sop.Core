using Sop.Framework.Environment;
using Sop.Framework.SystemLog;
using Sop.Framework.Utility;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(RunningEnvironment), "Initialize")]

namespace Sop.Framework.Environment
{
    /// <summary>
    /// 默认运行环境实现
    /// </summary>
    public class RunningEnvironment
    {
        private const string WebConfigPath = "~/web.config";
        private const string BinPath = "~/bin";
        private const string RefreshHtmlPath = "~/refresh.html";

        public static void Initialize()
        {
            IsLog4netConfig();
        }
        public static bool IsLog4netConfig()
        {
            try
            {
                //string a = "asd";
                //int b = Convert.ToInt32(a);
            }
            catch (Exception ex)
            {
                Log.Instance().Write("RunningEnvironment:" + ex.Message, ex, typeof(RunningEnvironment), LogLevel.Info);

            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsFullTrust()
        {
            return AppDomain.CurrentDomain.IsHomogenous && AppDomain.CurrentDomain.IsFullyTrusted;
        }


        public static void RestartAppDomain()
        {
            if (IsFullTrust())
            {
                HttpRuntime.UnloadAppDomain();
            }
            else
            {
                bool success = TryWriteBinFolder() || TryWriteWebConfig();

                if (!success)
                {
                    throw new ApplicationException(string.Format("需要启动站点，在非FullTrust环境下必须给\"{0}\"或者\"~/{1}\"写入的权限", BinPath, WebConfigPath));
                }
            }

            //通过http请求使站点启动
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                // Don't redirect posts...
                if (httpContext.Request.RequestType == "GET")
                {
                    httpContext.Response.Redirect(httpContext.Request.RawUrl, true /*endResponse*/);
                }
                else
                {
                    httpContext.Response.ContentType = "text/html";
                    httpContext.Response.WriteFile(RefreshHtmlPath);
                    httpContext.Response.End();
                }
            }
        }


        public static bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                System.IO.File.SetLastWriteTimeUtc(FileUtility.GetDiskFilePath(WebConfigPath), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool TryWriteBinFolder()
        {
            try
            {
                var binMarker = FileUtility.GetDiskFilePath(BinPath + "HostRestart");
                Directory.CreateDirectory(binMarker);
                using (var stream = File.CreateText(Path.Combine(binMarker, "log.txt")))
                {
                    stream.WriteLine("Restart on '{0}'", DateTime.UtcNow);
                    stream.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool IsAssemblyLoaded(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => new AssemblyName(assembly.FullName).Name == name);
        }

    }
}