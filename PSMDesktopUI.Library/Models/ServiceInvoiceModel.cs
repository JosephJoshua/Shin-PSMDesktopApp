using System;

namespace PSMDesktopUI.Library.Models
{
    public class ServiceInvoiceModel
    {
        public string NomorNota { get; set; }

        public string NamaPelanggan { get; set; }

        public string NoHp { get; set; }

        public string TipeHp { get; set; }

        public string Imei { get; set; }

        public string Kerusakan { get; set; }

        public decimal TotalBiaya { get; set; }

        public decimal Dp { get; set; }

        public decimal Sisa { get; set; }

        public string Kelengkapan { get; set; }

        public string YangBelumDicek { get; set; }

        public string Tanggal { get; set; }
    }
}
