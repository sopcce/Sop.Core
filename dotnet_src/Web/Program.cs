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
                          .UseSetting("detailedErrors", "true")
                           // .UseUrls("https://localhost:5101", "http://localhost:5102")
                           // .UseKestrel(o =>
                           // {
                           //     o.Listen(IPAddress.Any, 5101); //HTTP port
                           //     o.Listen(IPAddress.Any, 5102); //HTTPS port
                           // })
                          .UseStartup<Startup>();
        }
    }
}