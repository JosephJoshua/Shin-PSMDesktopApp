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

        public static async Task<ApiException> FromHttpResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsAsync<HttpErrorContent>();

            // If content.Message is null, just put the reason phrase as the message.
            string message = $"{ response.ReasonPhrase }{ (content.Message != null ? ":" : "") } { content.Message ?? "" }";

            if (content.Error == null)
            {
                return new ApiException(message);
            }

            return new ApiException(message, content.Error);
        }

        private class HttpErrorContent
        {
            [JsonProperty(PropertyName = "message", Required = Required.Always)]
            public string Message { get; set; }

            [JsonProperty(PropertyName = "error")]
            public string Error { get; set; }
        }
    }
}
