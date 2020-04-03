using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace Sop.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        private static AppSettings _instance = null;
        private static readonly object Padlock = new object();
        public static AppSettings Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppSettings();
                    }

                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        static AppSettings()
        {
            Configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
        /// <summary>
        /// 
        /// </summary>
        public string Get_JwtToken_Issuer
        {
            get
            { 
                return Configuration["AppSettings:JwtToken:Issuer"] ?? "null_null_null-null";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Get_JwtToken_SecretKey
        {
            get
            { 
                return Configuration["AppSettings:JwtToken:SecretKey"] ?? "null_null_null-null";
            }
        }



        /// <summary>
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public JwtToken JwtToken { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }
    }
}