namespace PSMDesktopApp.Library.Helpers
{
    public interface ISettingsHelper
    {
        Settings Settings { get; }

        void ReadSettingsFromFile();

        void SaveSettingsToFile();
    }
}