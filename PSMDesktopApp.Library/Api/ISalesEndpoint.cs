using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopApp.Library.Models;

namespace PSMDesktopApp.Library.Api
{
    public interface ISalesEndpoint
    {
        Task Delete(int id);
        Task<List<SalesModel>> GetAll();
        Task<SalesModel> GetById(int id);
        Task Insert(SalesModel sales);
    }
}