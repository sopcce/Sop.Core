namespace Sop.Domain.VModel
{
    public class AuthenticateModel
    {
        public string Username { get; internal set; }
        public string Token { get; internal set; }
        public string Password { get; set; }
    }
}