using System;
using System.Net;
using System.Net.NetworkInformation;

namespace PSMDesktopApp.Library.Helpers
{
    public class ConnectionHelper : IConnectionHelper
    {
        private readonly ISettingsHelper _settingsHelper;
        private bool _isNetworkAvailable;

        public bool WasConnectionSuccessful { get; set; } = true;

        public Action OnConnectionFailed { get; set; }

        public ConnectionHelper(ISettingsHelper settingsHelper)
        {
            _settingsHelper = settingsHelper;

            _isNetworkAvailable = IsNetworkAvailable();
            if (!_isNetworkAvailable)
            {
                WasConnectionSuccessful = false;
                OnConnectionFailed?.Invoke();
            }

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(OnNetworkAvailabilityChanged);
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(OnNetworkAddressChanged);
        }

        public bool CanConnectToApi()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.OpenRead(_settingsHelper.Settings.ApiUrl).Close();
                    WasConnectionSuccessful = true;

                    return true;
                }
                catch (WebException)
                {
                    if (!WasConnectionSuccessful)
                    {
                        OnConnectionFailed?.Invoke();
                    }

                    WasConnectionSuccessful = false;
                    return false;
                }
            }
        }

        private bool IsNetworkAvailable()
        {
            // We only want to recognize changes related to internet adapters
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface networkInterface in interfaces)
                {
                    // Only check certain internet adapters
                    if (networkInterface.OperationalStatus == OperationalStatus.Up && 
                       (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel) &&
                       (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                    {
                        IPv4InterfaceStatistics statistics = networkInterface.GetIPv4Statistics();

                        if ((statistics.BytesReceived > 0) &&
                            (statistics.BytesSent > 0))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void NotifyNetworkAvailabilityChange()
        {
            bool newStatus = IsNetworkAvailable();

            if (!newStatus && newStatus != _isNetworkAvailable)
            {
                WasConnectionSuccessful = false;
                OnConnectionFailed?.Invoke();
            }

            _isNetworkAvailable = newStatus;
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            NotifyNetworkAvailabilityChange();
        }

        private void OnNetworkAddressChanged(object sender, EventArgs e)
        {
            NotifyNetworkAvailabilityChange();
        }
    }
}
