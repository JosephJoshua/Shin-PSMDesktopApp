using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopUI.Library.Models;

namespace PSMDesktopUI.Library.Api
{
    public interface ISparepartEndpoint
    {
        Task Delete(int id);
        Task<List<SparepartModel>> GetByService(int nomorNota);
        Task Insert(SparepartModel sparepart);
    }
}