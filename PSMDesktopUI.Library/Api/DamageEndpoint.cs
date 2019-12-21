using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PSMDesktopUI.Library.Api
{
    public class DamageEndpoint : IDamageEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public DamageEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<DamageModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Damage").ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<DamageModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Insert(DamageModel damage)
        {
            await _apiHelper.ApiClient.PostAsJsonAsync("/api/Damage", damage).ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            await _apiHelper.ApiClient.DeleteAsync("/api/Damage/" + id).ConfigureAwait(false);
        }
    }
}
