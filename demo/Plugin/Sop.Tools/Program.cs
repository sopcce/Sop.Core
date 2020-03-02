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
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
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
                .UseUrls("http://*:5000")
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
               .UseStartup<Startup>();
             

            return host;
        }
    }
 
}
