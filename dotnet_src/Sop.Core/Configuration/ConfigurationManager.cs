using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Sop.Core
{
    public static class ConfigurationManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IConfiguration Configuration;

        static ConfigurationManager()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static T GetSection<T>(string key) where T : class, new()
        {
            var sd = Configuration.GetSection(key);
            //var ser = new ServiceCollection();
            //ser.AddOptions();
            //ser.Configure<T>(n =>
            //{
            //    sd
            //});
            //ser.BuildServiceProvider()

            return new ServiceCollection()
                .AddOptions()
                .Configure<T>(n =>
                {
                    Configuration.GetSection(key);
                })
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
        }

        public static string GetSection(string key)
        {
            return Configuration.GetValue<string>(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AppSettings GetAppSettings()
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            return appSettingsSection.Get<AppSettings>();
        }

    }
}