using PSMDesktopApp.Library.Helpers;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/token", data))
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

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/user"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    LoggedInUser.Id = result.Id;
                    LoggedInUser.Username = result.Username;
                    LoggedInUser.EmailAddress = result.EmailAddress;
                    LoggedInUser.Role = result.Role;
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
