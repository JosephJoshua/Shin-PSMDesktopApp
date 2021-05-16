namespace PSMDesktopApp.Library.Helpers
{
    public interface IStringEncryptionHelper
    {
        string HashPassword(string password);

        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
