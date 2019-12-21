using System;
using System.Collections.Generic;

namespace PSMDesktopUI.Library.Models
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

        public int SalesId { get; set; }

        public string StatusServisan { get; set; }

        public DateTime TanggalKonfirmasi { get; set; }

        public string IsiKonfirmasi { get; set; }

        public decimal Biaya { get; set; }

        public int Discount { get; set; }

        public decimal Dp { get; set; }

        public decimal TambahanBiaya { get; set; }

        public decimal TotalBiaya { get; set; }

        public decimal HargaSparepart { get; set; }

        public decimal Sisa { get; set; }

        public decimal LabaRugi { get; set; }

        public DateTime TanggalPengambilan { get; set; }

        public ICollection<SparepartModel> Spareparts { get; set; }
    }
}
