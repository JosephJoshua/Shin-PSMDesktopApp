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
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Technician"))
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
    }
}
