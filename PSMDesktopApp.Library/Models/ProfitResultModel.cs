using Newtonsoft.Json;
using System;

namespace PSMDesktopApp.Library.Models
{
    public class ProfitResultModel
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

        [JsonProperty(PropertyName = "harga_sparepart")]
        public decimal HargaSparepart { get; set; }

        [JsonProperty(PropertyName = "laba_rugi")]
        public decimal LabaRugi { get; set; }
    }
}
