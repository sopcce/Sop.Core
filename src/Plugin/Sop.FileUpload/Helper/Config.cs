using Microsoft.Extensions.DependencyInjection;

namespace Sop.FileUpload.Models.Helper
{
    public static class Config
    {
        public static string EncryptKey { get; set; } = "123123";


        public static bool IsHosted { get; private set; }

        public static void SetIsHosted(this IServiceCollection services)
        {
            // you can grab any other info from your services collection
            // if you want.  This is an extension method that you call
            // from your Startup.ConfigureServices method

            IsHosted = true;
        }
    }
    
}