using System;

namespace PSMDataManager.Library.Models
{
    public class ServiceModel
    {
        public int NomorNota { get; set; }

        public DateTime Tanggal { get; set; }

        public string NamaPelanggan { get; set; }

        public string NoHp { get; set; }

        public string TipeHp { get; set; }

        public string Imei { get; set; }

        public int DamageId { get; set; }

        public string YangBelumDicek { get; set; }

        public string Kelengkapan { get; set; }

        public string Warna { get; set; }

        public string KataSandiPola { get; set; }

        public int TechnicianId { get; set; }

        public string StatusServisan { get; set; }

        public DateTime TanggalKonfirmasi { get; set; }

        public string IsiKonfirmasi { get; set; }

        public int Biaya { get; set; }

        public int Discount { get; set; }

        public int Dp { get; set; }

        public int TambahanBiaya { get; set; }

        public int TotalBiaya { get; set; }

        public int HargaSparepart { get; set; }

        public int Sisa { get; set; }

        public int LabaRugi { get; set; }

        public DateTime TanggalPengambilan { get; set; }
    }
}
