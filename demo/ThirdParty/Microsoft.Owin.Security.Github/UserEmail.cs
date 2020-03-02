namespace Microsoft.Owin.Security.Github
{
    public class UserEmail
    {
        public string Email { get; set; }

        public bool Primary { get; set; }

        public bool Verified { get; set; }
    }
}
