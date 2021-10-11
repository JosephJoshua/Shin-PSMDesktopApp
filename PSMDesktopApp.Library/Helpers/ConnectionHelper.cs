using System;
using System.Net;

namespace PSMDesktopApp.Library.Helpers
{
    public class ConnectionHelper : IConnectionHelper
    {
        private readonly ISettingsHelper _settingsHelper;
        private bool _wasConnectionSuccessful = true;

        public Action OnConnectionFailed { get; set; }

        public ConnectionHelper(ISettingsHelper settingsHelper)
        {
            _settingsHelper = settingsHelper;
        }

        public bool CanConnectToApi()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.OpenRead(_settingsHelper.Settings.ApiUrl).Close();
                    _wasConnectionSuccessful = true;
                    
                    return true;
                }
                catch (WebException)
                {
                    if (_wasConnectionSuccessful)
                    {
                        OnConnectionFailed?.Invoke();
                    }

                    _wasConnectionSuccessful = false;
                    return false;
                }
            }
        }
    }
}
