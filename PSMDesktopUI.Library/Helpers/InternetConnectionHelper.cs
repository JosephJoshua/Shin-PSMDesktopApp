using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace PSMDesktopUI.Library.Helpers
{
    public class InternetConnectionHelper : IInternetConnectionHelper
    {
        private bool _hasInternetConnection;

        public event EventHandler InternetConnectionAvailabilityChanged;

        public bool HasInternetConnection
        {
            get => _hasInternetConnection;

            private set
            {
                _hasInternetConnection = value;
                OnInternetConnectionAvailabilityChanged(EventArgs.Empty);
            }
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public void Init()
        {
            HasInternetConnection = InternetGetConnectedState(out int desc, 0);
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        protected void OnInternetConnectionAvailabilityChanged(EventArgs e)
        {
            InternetConnectionAvailabilityChanged?.Invoke(this, e);
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            HasInternetConnection = e.IsAvailable;
        }
    }
}
