using Newtonsoft.Json.Converters;
using PSMDesktopApp.Library.Helpers;

namespace PSMDesktopApp.Library.JsonConverters
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = Constants.DateTimeFormat;
        }
    }
}
