using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Sop.Data;
using System;
using System.Net;

namespace Sop.Tools
{
    public class Program
    {


        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("log/log.txt",
                    rollingInterval: RollingInterval.Day,                  
                    rollOnFileSizeLimit: true)
                .CreateLogger();
            try
            {
                var host = CreateWebHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<SopContext>();
                        // using ContosoUniversity.Data; 
                        DbInitializer.Initialize(context);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred creating the DB.");
                    }
                }
                host.Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            } 
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    logging.AddConsole();
                //    logging.AddDebug();
                //    logging.AddEventSourceLogger();
                //    logging.SetMinimumLevel(LogLevel.Warning);
                //})
                //.UseKestrel(options =>
                //{
                //    options.Limits.MaxRequestBodySize = null;

                //})
                .UseUrls("http://*:5000 ")
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
               .UseStartup<Startup>();
             

            return host;
        }
    }
    internal class CurrentDirectoryHelpers
    {
        internal const string AspNetCoreModuleDll = "aspnetcorev2_inprocess.dll";

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [System.Runtime.InteropServices.DllImport(AspNetCoreModuleDll)]
        private static extern int http_get_application_properties(ref IISConfigurationData iiConfigData);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct IISConfigurationData
        {
            public IntPtr pNativeApplication;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            public string pwzFullApplicationPath;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            public string pwzVirtualApplicationPath;
            public bool fWindowsAuthEnabled;
            public bool fBasicAuthEnabled;
            public bool fAnonymousAuthEnable;
        }

        public static void SetCurrentDirectory()
        {
            try
            {
                // Check if physical path was provided by ANCM
                var sitePhysicalPath = Environment.GetEnvironmentVariable("ASPNETCORE_IIS_PHYSICAL_PATH");
                if (string.IsNullOrEmpty(sitePhysicalPath))
                {
                    // Skip if not running ANCM InProcess
                    if (GetModuleHandle(AspNetCoreModuleDll) == IntPtr.Zero)
                    {
                        return;
                    }

                    IISConfigurationData configurationData = default(IISConfigurationData);
                    if (http_get_application_properties(ref configurationData) != 0)
                    {
                        return;
                    }

                    sitePhysicalPath = configurationData.pwzFullApplicationPath;
                }

                Environment.CurrentDirectory = sitePhysicalPath;
            }
            catch
            {
                // ignore
            }
        }
    }
}
