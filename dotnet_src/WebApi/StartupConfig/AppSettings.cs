namespace WebApi.StartupConfig
{
    ///
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