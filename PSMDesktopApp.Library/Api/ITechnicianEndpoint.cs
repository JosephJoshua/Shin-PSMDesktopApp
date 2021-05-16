using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopApp.Library.Models;

namespace PSMDesktopApp.Library.Api
{
    public interface ITechnicianEndpoint
    {
        Task<List<TechnicianModel>> GetAll();

        Task<TechnicianModel> GetById(int id);

        Task Insert(TechnicianModel technician);

        Task Delete(int id);
    }
}