using Newtonsoft.Json;
using System.IO;

namespace PSMDesktopApp.Library.Helpers
{
    public class Settings
    {
        public string ApiUrl { get; set; }

        public string ApiRequestPrefix { get; set; }

        public string ReportPath { get; set; }

        public string NamaToko { get; set; }

        public string NoHpToko { get; set; }

        public string AlamatToko { get; set; }
    }

    public class SettingsHelper : ISettingsHelper
    {
        public Settings Settings { get; private set; } = new Settings();

        private const string FilePath = "settings.json";

        public SettingsHelper()
        {
            Init();
        }

        public void ReadSettingsFromFile()
        {
            string content = File.ReadAllText(FilePath);
            Settings = JsonConvert.DeserializeObject<Settings>(content);
        }

        public void SaveSettingsToFile()
        {
            string data = JsonConvert.SerializeObject(Settings);
            File.WriteAllText(FilePath, data);
        }

        private void Init()
        {
            if (!File.Exists(FilePath))
            {
                Settings.ApiUrl = "https://jointcell.online";
                Settings.ApiRequestPrefix = "api";
                Settings.ReportPath = "Reports/ServiceInvoice.rpt";
                Settings.NamaToko = "Galerie Mobile";
                Settings.NoHpToko = "082398200020";
                Settings.AlamatToko = "Jl. Pendidikan\nSorong, Papua Barat";

                SaveSettingsToFile();
            }

            ReadSettingsFromFile();
        }
    }
}
