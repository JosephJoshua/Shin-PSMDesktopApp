using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopApp.Library.Models;

namespace PSMDesktopApp.Library.Api
{
    public interface IServiceEndpoint
    {
        Task Delete(int id);
        Task<List<ServiceModel>> GetAll();
        Task<ServiceModel> GetByNomorNota(int nomorNota);
        Task Insert(ServiceModel service);
        Task Update(ServiceModel service);
    }
}