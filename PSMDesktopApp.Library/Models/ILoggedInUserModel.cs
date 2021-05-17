namespace PSMDesktopApp.Library.Models
{
    public interface ILoggedInUserModel
    {
        string email { get; set; }

        string id { get; set; }

        string role { get; set; }

        string Token { get; set; }

        string username { get; set; }
    }
}