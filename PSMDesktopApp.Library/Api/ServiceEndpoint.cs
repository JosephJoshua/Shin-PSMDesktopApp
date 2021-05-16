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

        public async Task<List<ServiceModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Service").ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ServiceModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<ServiceModel> GetByNomorNota(int nomorNota)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Service/" + nomorNota).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    ServiceModel result = await response.Content.ReadAsAsync<ServiceModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Insert(ServiceModel service)
        {
            await _apiHelper.ApiClient.PostAsJsonAsync("/api/Service", service).ConfigureAwait(false);
        }

        public async Task Update(ServiceModel service)
        {
            await _apiHelper.ApiClient.PutAsJsonAsync("/api/Service", service).ConfigureAwait(false);
        }

        public async Task Delete(int nomorNota)
        {
            await _apiHelper.ApiClient.DeleteAsync("/api/Service/" + nomorNota).ConfigureAwait(false);
        }
    }
}
