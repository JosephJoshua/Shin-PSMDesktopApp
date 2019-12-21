using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopUI.Library.Models;

namespace PSMDesktopUI.Library.Api
{
    public interface ISalesEndpoint
    {
        Task Delete(int id);
        Task<List<SalesModel>> GetAll();
        Task Insert(SalesModel sales);
    }
}