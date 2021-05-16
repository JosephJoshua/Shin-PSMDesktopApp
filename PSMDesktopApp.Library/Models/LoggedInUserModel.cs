namespace PSMDesktopApp.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }

        public string Id { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string Role { get; set; }
    }
}
