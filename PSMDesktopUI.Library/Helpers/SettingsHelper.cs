using System.Collections.Generic;
using System.IO;

namespace PSMDesktopUI.Library.Helpers
{
    public class SettingsHelper : ISettingsHelper
    {
        private const string FilePath = "settings.txt";

        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public SettingsHelper()
        {
            Init();
        }

        private void Init()
        {
            if (File.Exists(FilePath))
            {
                GetSettings();
            }
            else
            {
                // Create default settings file if it doesn't exist
                TextWriter writer = new StreamWriter(FilePath);

                writer.WriteLine(@"apiUrl: http://localhost:3030/");
                writer.WriteLine(@"reportPath: Reports/ServiceInvoice.rpt");

                writer.Close();

                GetSettings();
            }
        }

        private void GetSettings()
        {
            foreach (string line in File.ReadAllLines(FilePath))
            {
                string key = line.Split(':')[0];
                string val = line.Substring(key.Length + 1);

                _settings.Add(key, val);
            }
        }

        public string Get(string key)
        {
            if (_settings.TryGetValue(key, out string val))
            {
                return val;
            }

            return "";
        }
    }
}
