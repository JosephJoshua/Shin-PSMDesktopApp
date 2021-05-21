using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopApp.Library.Models;

namespace PSMDesktopApp.Library.Api
{
    public interface IServiceEndpoint
    {
        Task Delete(int id);
        Task<List<ServiceModel>> GetAll(string searchText = "", SearchType searchType = SearchType.NamaPelanggan, DateTime? minDate = null, DateTime? maxDate = null);
        Task<ServiceModel> GetByNomorNota(int nomorNota); 
        Task Insert(ServiceModel service);
        Task Update(ServiceModel service, int nomorNota);
    }
}