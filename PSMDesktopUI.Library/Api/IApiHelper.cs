using System.Threading.Tasks;
using PSMDesktopUI.Library.Models;

namespace PSMDesktopUI.Library.Api
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);

        Task GetLoggedInUserInfo(string token);
    }
}