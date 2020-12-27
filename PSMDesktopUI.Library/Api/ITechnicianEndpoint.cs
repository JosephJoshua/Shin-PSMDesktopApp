using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopUI.Library.Models;

namespace PSMDesktopUI.Library.Api
{
    public interface ITechnicianEndpoint
    {
        Task<List<TechnicianModel>> GetAll();

        Task<TechnicianModel> GetById(int id);

        Task Insert(TechnicianModel technician);

        Task Delete(int id);
    }
}