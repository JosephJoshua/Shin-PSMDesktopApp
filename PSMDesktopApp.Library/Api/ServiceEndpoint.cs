using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopApp.Library.Api
{
    public class ServiceEndpoint : IServiceEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public ServiceEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<ServiceModel>> GetAll(string searchText = "", SearchType searchType = SearchType.NamaPelanggan, DateTime? minDate = null, DateTime? maxDate = null)
        {
            var queryParams = new List<KeyValuePair<string, string>> 
            { 
                new KeyValuePair<string, string>("q", searchText),
                new KeyValuePair<string, string>("by", GetColumnFromSearchType(searchType))
            };

            const string dtFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";

            // We have to use the null conditional operator so we can use it as a normal DateTime, as opposed to a nullable one.
            if (minDate != null) queryParams.Add(new KeyValuePair<string, string>("min_date", minDate?.ToString(dtFormat)));
            if (maxDate != null) queryParams.Add(new KeyValuePair<string, string>("max_date", maxDate?.ToString(dtFormat)));

            string query = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
            string url = "/api/servisan?" + query;

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ServiceModel>>();
                    return result;
                }
                else
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task<ServiceModel> GetByNomorNota(int nomorNota)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/servisan/" + nomorNota).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    ServiceModel result = await response.Content.ReadAsAsync<ServiceModel>();
                    return result;
                }
                else
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task Insert(ServiceModel service)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/servisan", service).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task Update(ServiceModel service)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync("/api/servisan", service).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task Delete(int nomorNota)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("/api/servisan/" + nomorNota).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        private string GetColumnFromSearchType(SearchType searchType)
        {
            switch (searchType)
            {
                case SearchType.NamaPelanggan:
                    return "nama_pelanggan";
                case SearchType.NomorHp:
                    return "nomor_hp";
                case SearchType.NomorNota:
                    return "nomor_nota";
                case SearchType.Status:
                    return "status";
                case SearchType.TipeHp:
                    return "tipe_hp";
                default:
                    throw new Exception("Unhandled search type");
            }
        }
    }
}
