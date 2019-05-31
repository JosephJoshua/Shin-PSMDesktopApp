using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopUI.Library.Api
{
    public class MemberEndpoint : IMemberEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public MemberEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<MemberModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/member"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<MemberModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Insert(MemberModel member)
        {
            await _apiHelper.ApiClient.PostAsJsonAsync("/api/Member", member);
        }

        public async Task Update(MemberModel member)
        {
            await _apiHelper.ApiClient.PutAsJsonAsync("/api/Member", member);
        }

        public async Task Delete(int id)
        {
            await _apiHelper.ApiClient.DeleteAsync("/api/Member/" + id);
        }
    }
}
