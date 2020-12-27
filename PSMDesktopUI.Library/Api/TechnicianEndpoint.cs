using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopUI.Library.Api
{
    public class TechnicianEndpoint : ITechnicianEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public TechnicianEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<TechnicianModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Technician").ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<TechnicianModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<TechnicianModel> GetById(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Technician/" + id).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    TechnicianModel result = await response.Content.ReadAsAsync<TechnicianModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Insert(TechnicianModel technician)
        {
            await _apiHelper.ApiClient.PostAsJsonAsync("/api/Technician", technician).ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            await _apiHelper.ApiClient.DeleteAsync("/api/Technician/" + id).ConfigureAwait(false);
        }
    }
}
