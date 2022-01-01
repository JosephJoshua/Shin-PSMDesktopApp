using PSMDesktopApp.Library.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopApp.Library.Api
{
    public class TechnicianEndpoint : ITechnicianEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public TechnicianEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<TechnicianModel>> GetAll(string searchText = "")
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("teknisi/?q=" + WebUtility.UrlEncode(searchText))
                .ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<TechnicianModel>>();
                    return result;
                }
                else
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task<TechnicianModel> GetById(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("teknisi/" + id).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    TechnicianModel result = await response.Content.ReadAsAsync<TechnicianModel>();
                    return result;
                }
                else
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task Insert(TechnicianModel technician)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("teknisi/", technician).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task Delete(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("teknisi/" + id).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }
    }
}
