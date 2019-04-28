using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sop.Data;
using System;
using System.Net;

namespace Sop.Tools
{
    public class Program
    {


        public static void Main(string[] args)
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    logging.SetMinimumLevel(LogLevel.Warning);
                })
                .UseKestrel(options =>
                { 
                    options.Limits.MaxRequestBodySize = null;

                })
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
               .UseStartup<Startup>();

            var address = IPAddress.Broadcast;
            Console.WriteLine("IP address : " + address);

            return host;
        }
    }
}
