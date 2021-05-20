using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopApp.Library.Api
{
    public class SparepartEndpoint : ISparepartEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public SparepartEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<SparepartModel>> GetAll(DateTime? minDate = null, DateTime? maxDate = null)
        {
            var queryParams = new List<KeyValuePair<string, string>>();
            const string dtFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";

            // We have to use the null conditional operator so we can use it as a normal DateTime, as opposed to a nullable one.
            if (minDate != null) queryParams.Add(new KeyValuePair<string, string>("min_date", minDate?.ToString(dtFormat)));
            if (maxDate != null) queryParams.Add(new KeyValuePair<string, string>("max_date", maxDate?.ToString(dtFormat)));

            string query = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
            string url = "/api/sparepart?" + query;

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<SparepartModel>>();
                    return result;
                }
                else
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task<List<SparepartModel>> GetByNomorNota(int nomorNota)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/sparepart/" + nomorNota).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<SparepartModel>>();
                    return result;
                }
                else
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task Insert(SparepartModel sparepart)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/sparepart", sparepart).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task Delete(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("/api/sparepart/" + id).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }
    }
}
