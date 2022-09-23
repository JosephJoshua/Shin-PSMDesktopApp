using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopApp.Library.Models;

namespace PSMDesktopApp.Library.Api
{
    public interface IServiceEndpoint
    {

        Task<List<ServiceModel>> GetAll(string searchText = "", SearchType searchType = SearchType.NamaPelanggan, DateTime? minDate = null, DateTime? maxDate = null);

        Task<List<ProfitResultModel>> GetLabaRugiReport(DateTime? minDate = null, DateTime? maxDate = null);

        Task<List<TechnicianResultModel>> GetTeknisiReport(int idTeknisi, DateTime? minDate = null, DateTime? maxDate = null);

        Task<ServiceModel> GetByNomorNota(int nomorNota); 

        Task<int> Insert(ServiceModel service);

        Task Update(ServiceModel service, int nomorNota);

        Task Delete(int id);
    }
}