using Newtonsoft.Json;

namespace PSMDesktopApp.Library.Models
{
    public class TechnicianModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "nama")]
        public string Nama { get; set; }
    }
}
