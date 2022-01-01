using System;

namespace PSMDesktopApp.Library.Helpers
{
    public interface IConnectionHelper
    {
        Action OnConnectionFailed { set; }

        bool WasConnectionSuccessful { get; }

        bool CanConnectToApi();
    }
}