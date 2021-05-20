using System;
using Newtonsoft.Json;

namespace PSMDesktopApp.Library.Models
{
    public class SparepartModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "nomor_nota")]
        public int NomorNota { get; set; }

        [JsonProperty(PropertyName = "nama")]
        public string Nama { get; set; }

        [JsonProperty(PropertyName = "harga")]
        public decimal Harga { get; set; }

        [JsonProperty(PropertyName = "tanggal_pengambilan")]
        public DateTime TanggalPembelian { get; set; }
    }
}
