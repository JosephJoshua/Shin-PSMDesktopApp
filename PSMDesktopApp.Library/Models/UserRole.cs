using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PSMDesktopApp.Library.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRole
    {
        [EnumMember(Value = "admin")]
        Admin,

        [EnumMember(Value = "customer_service")]
        CustomerService,

        [EnumMember(Value = "buyer")]
        Buyer,
    }
}
