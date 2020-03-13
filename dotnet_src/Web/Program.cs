using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

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