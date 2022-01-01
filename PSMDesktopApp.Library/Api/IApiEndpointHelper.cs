using System;

namespace PSMDesktopApp.Library.Api
{
    public interface IApiEndpointHelper
    {
        bool IsConnectionProblem(Exception e);
    }
}