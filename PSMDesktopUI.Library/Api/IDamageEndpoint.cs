using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopUI.Library.Models;

namespace PSMDesktopUI.Library.Api
{
    public interface IDamageEndpoint
    {
        Task<List<DamageModel>> GetAll();
    }
}