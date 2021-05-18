using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopApp.Library.Api
{
    public class SalesEndpoint : ISalesEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public SalesEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<SalesModel>> GetAll(string searchText = "")
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(WebUtility.UrlEncode("/api/sales?q=" + searchText))
                    .ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)   
                {
                    var result = await response.Content.ReadAsAsync<List<SalesModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<SalesModel> GetById(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/sales/" + id).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    SalesModel result = await response.Content.ReadAsAsync<SalesModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Insert(SalesModel sales)
        {
            await _apiHelper.ApiClient.PostAsJsonAsync("/api/sales", sales).ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            await _apiHelper.ApiClient.DeleteAsync("/api/sales/" + id).ConfigureAwait(false);
        }
    }
}
