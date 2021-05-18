using Newtonsoft.Json;
using PSMDesktopApp.Library.Helpers;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PSMDesktopApp.Library.Api
{
    public class ApiHelper : IApiHelper
    {
        public ILoggedInUserModel LoggedInUser { get; set; }

        private readonly ISettingsHelper _settings;

        private HttpClient _apiClient { get; set; }

        public HttpClient ApiClient
        {
            get => _apiClient;
        }

        public ApiHelper(ILoggedInUserModel loggedInUser, ISettingsHelper settings)
        {
            _settings = settings;

            InitializeClient();

            LoggedInUser = loggedInUser;
        }

        private void InitializeClient()
        {
            string api = _settings.Get("apiUrl");

            _apiClient = new HttpClient
            {
                BaseAddress = new Uri(api)
            };

            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string email, string password)
        {
            string jsonReq = JsonConvert.SerializeObject(new
            {
                email,
                password,
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/api/login", new StringContent(jsonReq, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/users/current"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    LoggedInUser.id = result.id;
                    LoggedInUser.username = result.username;
                    LoggedInUser.email = result.email;
                    LoggedInUser.role = result.role;
                    LoggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
