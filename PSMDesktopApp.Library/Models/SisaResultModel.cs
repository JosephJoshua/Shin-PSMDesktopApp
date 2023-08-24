using Newtonsoft.Json;
using System;

namespace PSMDesktopApp.Library.Models
{
    public class SisaResultModel 
    {
        [JsonProperty(PropertyName = "nomor_nota")]
        public int NomorNota { get; set; }

        [JsonProperty(PropertyName = "tanggal_pengambilan")]
        public DateTime TanggalPengambilan { get; set; }

        [JsonProperty(PropertyName = "tipe_hp")]
        public string TipeHp { get; set; }

        [JsonProperty(PropertyName = "kerusakan")]
        public string Kerusakan { get; set; }

        [JsonProperty(PropertyName = "biaya")]
        public decimal Biaya { get; set; }

        [JsonProperty(PropertyName = "dp")]
        public decimal Dp { get; set; }

        [JsonProperty(PropertyName = "sisa")]
        public decimal Sisa { get; set; }
    }
}
