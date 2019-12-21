using System;

namespace PSMDesktopUI.Library.Models
{
    public class ProfitResultModel
    {
        public int NomorNota { get; set; }

        public DateTime TanggalPengambilan { get; set; }

        public string TipeHp { get; set; }

        public string Kerusakan { get; set; }

        public decimal Biaya { get; set; }

        public decimal HargaSparepart { get; set; }

        public decimal LabaRugi { get; set; }
    }
}
