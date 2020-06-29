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
        /// <summary>
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public JwtToken JwtToken { get; set; }
        
    }

    /// <summary>
    /// JwtToken
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }
    }
}