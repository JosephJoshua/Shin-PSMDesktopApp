namespace PSMDesktopUI.Library.Models
{
    public interface ILoggedInUserModel
    {
        string EmailAddress { get; set; }
        string Id { get; set; }
        string Role { get; set; }
        string Token { get; set; }
        string Username { get; set; }
    }
}