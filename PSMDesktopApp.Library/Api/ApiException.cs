using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PSMDesktopApp.Library.Api
{
    public class ApiException : Exception
    {
        public string Details { get; }

        public ApiException() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, Exception inner) : base(message, inner) { }

        public ApiException(string message, string details) : this(message)
        {
            Details = details;
        }

        public ApiException(string message, Exception inner, string details) : this(message, inner)
        {
            Details = details;
        }

        public static async Task<Exception> FromHttpResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return new Exception("Attempting to create an ApiException from a successful request");

            // To avoid InvalidDataContract and UnsupportedMediaType exceptions when reading an
            // response body returned by gin-gonic or gin-jwt.
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var content = await response.Content.ReadAsAsync<HttpErrorContent>();
                string message = $"{ response.ReasonPhrase }{ (content.Message != null ? ":" : "") } { content.Message ?? "" }";

                if (content.Error == null)
                {
                    return new ApiException(message);
                }

                return new ApiException(message, content.Error);
            }
            else if (response.Content.Headers.ContentType.MediaType == "text/plain")
            {
                string message = await response.Content.ReadAsStringAsync();
                return new ApiException(message);
            }

            // Don't think this will ever be reached...
            return new ApiException(response.ReasonPhrase);
        }

        private class HttpErrorContent
        {
            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }

            [JsonProperty(PropertyName = "error")]
            public string Error { get; set; }
        }
    }
}
