using System.Collections.Generic;
using System.IO;

namespace PSMDesktopApp.Library.Helpers
{
    public class SettingsHelper : ISettingsHelper
    {
        private string FilePath => "settings.txt";

        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public SettingsHelper()
        {
            Init();
        }

        private void Init()
        {
            if (!File.Exists(FilePath))
            {
                // Create default settings file if it doesn't exist
                TextWriter writer = new StreamWriter(FilePath);

                writer.WriteLine(@"apiUrl: http://localhost:3030/");
                writer.WriteLine(@"reportPath: Reports/ServiceInvoice.rpt");
                writer.WriteLine(@"noHpToko: 082398200020");
                writer.WriteLine(@"alamatToko: Jl. Pendidikan\nSorong, Papua Barat");

                writer.Close();
            }

            if (!GetSettings())
            {
                throw new System.Exception("Gagal membaca " + FilePath);
            }
        }

        private bool GetSettings()
        {
            foreach (string line in File.ReadAllLines(FilePath))
            {
                string[] keyValue = line.Split(':');
                if (keyValue.Length < 1)
                {
                    return false;
                }

                string key = keyValue[0];
                string val = line.Substring(key.Length + 1);

                if (val.Length < 1)
                {
                    return false;
                }
                else if (val[0] == ' ')
                {
                    val = val.Remove(0);
                }

                _settings.Add(key, val);
            }

            return true;
        }

        public string Get(string key)
        {
            return _settings.TryGetValue(key, out string val) ? val : "";
        }
    }
}
