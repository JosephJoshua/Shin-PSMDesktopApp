using System.Threading.Tasks;
using PSMDesktopUI.Models;

namespace PSMDesktopUI.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}