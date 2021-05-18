namespace PSMDesktopApp.Library.Models
{
    public interface ILoggedInUserModel
    {
        string email { get; set; }

        string id { get; set; }

        UserRole role { get; set; }

        string Token { get; set; }

        string username { get; set; }
    }
}