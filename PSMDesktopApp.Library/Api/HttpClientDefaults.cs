using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;

namespace PSMDesktopApp.Library.Api
{
    public class HttpClientDefaults
    {
        public static MediaTypeFormatterCollection MediaTypeFormatters
        {
            get
            {
                PropertyInfo p = typeof(HttpContentExtensions).GetProperty("DefaultMediaTypeFormatterCollection",
                    BindingFlags.NonPublic | BindingFlags.Static);

                return (MediaTypeFormatterCollection)p.GetValue(null, null);
            }
        }
    }
}
