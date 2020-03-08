using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
       
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .CaptureStartupErrors(true)
                           .ConfigureLogging(logging =>
                            {
                                logging.ClearProviders();
                                logging.AddConsole();
                           })
                          .UseSetting("detailedErrors", "true") 
                          .UseStartup<Startup>();
        }
    }
}