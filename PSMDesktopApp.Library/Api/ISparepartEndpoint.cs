using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopApp.Library.Models;

namespace PSMDesktopApp.Library.Api
{
    public interface ISparepartEndpoint
    {
        Task Delete(int id);

        Task<List<SparepartModel>> GetAll();

        Task<List<SparepartModel>> GetByNomorNota(int nomorNota);

        Task Insert(SparepartModel sparepart);
    }
}