using Newtonsoft.Json;
using PSMDesktopApp.Library.Helpers;
using PSMDesktopApp.Library.JsonConverters;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PSMDesktopApp.Library.Api
{
    public class ApiHelper : IApiHelper
    {
        public ILoggedInUserModel LoggedInUser { get; set; }

        private readonly ISettingsHelper _settingsHelper;

        private HttpClient _apiClient;

        public HttpClient ApiClient => _apiClient;

        public ApiHelper(ILoggedInUserModel loggedInUser, ISettingsHelper settings)
        {
            _settingsHelper = settings;

            InitializeClient();

            LoggedInUser = loggedInUser;
        }

        private void InitializeClient()
        {
            var jsonFormatter = HttpClientDefaults.MediaTypeFormatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsonFormatter.SerializerSettings = new JsonSerializerSettings 
            { 
                Converters = new List<JsonConverter> { new CustomDateTimeConverter() },
            };

            string apiUrl = CombineURL(_settingsHelper.Settings.ApiUrl, _settingsHelper.Settings.ApiRequestPrefix);

            if (apiUrl.Last() != '/')
            {
                apiUrl += '/';
            }

            _apiClient = new HttpClient
            {
                BaseAddress = new Uri(apiUrl),
                Timeout = TimeSpan.FromSeconds(30),
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

            using (HttpResponseMessage response = await _apiClient.PostAsync("login", new StringContent(jsonReq, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await _apiClient.GetAsync("users/current"))
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
                    throw await ApiException.FromHttpResponse(response);
                }
            }
        }

        private string CombineURL(string url1, string url2)
        {
            if (url1.Length == 0) return url2;
            if (url2.Length == 0) return url1;

            url1 = url1.TrimEnd('/', '\\');
            url2 = url2.TrimStart('/', '\\');

            return string.Format("{0}/{1}", url1, url2);
        }
    }
}
