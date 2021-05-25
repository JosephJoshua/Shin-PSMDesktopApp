using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace PSMDesktopApp.Library.Models
{
    public class ServiceModel : INotifyPropertyChanged, IEquatable<ServiceModel>
    {
        [JsonProperty(PropertyName = "nomor_nota")]
        public int NomorNota { get; set; }

        [JsonProperty(PropertyName = "tanggal")]
        public DateTime Tanggal { get; set; }

        [JsonProperty(PropertyName = "nama_pelanggan")]
        public string NamaPelanggan { get; set; }

        [JsonProperty(PropertyName = "no_hp")]
        public string NoHp { get; set; }

        [JsonProperty(PropertyName = "tipe_hp")]
        public string TipeHp { get; set; }

        [JsonProperty(PropertyName = "imei")]
        public string Imei { get; set; }

        [JsonProperty(PropertyName = "kerusakan")]
        public string Kerusakan { get; set; }

        [JsonProperty(PropertyName = "kondisi_hp")]
        public string KondisiHp { get; set; }

        [JsonProperty(PropertyName = "yang_blm_dicek")]
        public string YangBelumDicek { get; set; }

        [JsonProperty(PropertyName = "kelengkapan")]
        public string Kelengkapan { get; set; }

        [JsonProperty(PropertyName = "warna")]
        public string Warna { get; set; }

        [JsonProperty(PropertyName = "kata_sandi_pola")]
        public string KataSandiPola { get; set; }

        [JsonProperty(PropertyName = "id_teknisi")]
        public int TechnicianId { get; set; }

        [JsonProperty(PropertyName = "id_sales")]
        public int SalesId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string StatusServisan { get; set; }

        [JsonProperty(PropertyName = "tanggal_konfirmasi")]
        public DateTime? TanggalKonfirmasi { get; set; }

        [JsonProperty(PropertyName = "isi_konfirmasi")]
        public string IsiKonfirmasi { get; set; }

        [JsonProperty(PropertyName = "biaya")]
        public decimal Biaya { get; set; }

        [JsonProperty(PropertyName = "diskon")]
        public int Diskon { get; set; }

        [JsonProperty(PropertyName = "dp")]
        public decimal Dp { get; set; }

        [JsonProperty(PropertyName = "tambahan_biaya")]
        public decimal TambahanBiaya { get; set; }

        [JsonProperty(PropertyName = "total_biaya")]
        public decimal TotalBiaya { get; set; }

        [JsonProperty(PropertyName = "harga_sparepart")]
        public decimal HargaSparepart { get; set; }

        [JsonProperty(PropertyName = "sisa")]
        public decimal Sisa { get; set; }

        [JsonProperty(PropertyName = "laba_rugi")]
        public decimal LabaRugi { get; set; }

        [JsonProperty(PropertyName = "tanggal_pengambilan")]
        public DateTime? TanggalPengambilan { get; set; }

        [JsonIgnore]
        public ICollection<SparepartModel> Spareparts
        {
            get => _spareparts;
            set
            {
                _spareparts = value;
                NotifyPropertyChanged(nameof(Spareparts));
            }
        }

        private ICollection<SparepartModel> _spareparts;

        // This is required so that the Master-Detail grid will update when the Spareparts property gets set.
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public bool Equals(ServiceModel other)
        {
            if (this == null && other == null) return true;

            return other != null && NomorNota == other.NomorNota && Tanggal == other.Tanggal && NamaPelanggan == other.NamaPelanggan &&
                   NoHp == other.NoHp && TipeHp == other.TipeHp && Imei == other.Imei && Kerusakan == other.Kerusakan &&
                   KondisiHp == other.KondisiHp && YangBelumDicek == other.YangBelumDicek && Kelengkapan == other.Kelengkapan &&
                   Warna == other.Warna && KataSandiPola == other.KataSandiPola && TechnicianId == other.TechnicianId &&
                   SalesId == other.SalesId && StatusServisan == other.StatusServisan && TanggalKonfirmasi == other.TanggalKonfirmasi &&
                   IsiKonfirmasi == other.IsiKonfirmasi && Biaya == other.Biaya && Dp == other.Dp && TambahanBiaya == other.TambahanBiaya &&
                   TotalBiaya == other.TotalBiaya && HargaSparepart == other.HargaSparepart && Sisa == other.Sisa && 
                   LabaRugi == other.LabaRugi && TanggalPengambilan == other.TanggalPengambilan;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ServiceModel);
        }

        public override int GetHashCode()
        {
            return NomorNota.GetHashCode();
        }
    }
}
