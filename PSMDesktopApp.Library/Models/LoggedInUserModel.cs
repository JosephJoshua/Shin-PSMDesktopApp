namespace PSMDesktopApp.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }

        public string id { get; set; }

        public string username { get; set; }

        public string email { get; set; }

        public UserRole role { get; set; }
    }
}
