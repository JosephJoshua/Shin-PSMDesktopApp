using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopUI.Library.Api
{
    public class SparepartEndpoint : ISparepartEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public SparepartEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<SparepartModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Sparepart").ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<SparepartModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<SparepartModel>> GetByService(int nomorNota)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Sparepart/" + nomorNota).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<SparepartModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Insert(SparepartModel sparepart)
        {
            await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sparepart", sparepart).ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            await _apiHelper.ApiClient.DeleteAsync("/api/Sparepart/" + id).ConfigureAwait(false);
        }
    }
}
