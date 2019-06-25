using System;

namespace PSMDesktopUI.Library.Helpers
{
    public interface IInternetConnectionHelper
    {
        bool HasInternetConnection { get; }

        event EventHandler InternetConnectionAvailabilityChanged;

        void Init();
    }
}