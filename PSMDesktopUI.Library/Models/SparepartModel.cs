using System;

namespace PSMDesktopUI.Library.Models
{
    public class SparepartModel
    {
        public int Id { get; set; }

        public int NomorNota { get; set; }

        public string Nama { get; set; }

        public decimal Harga { get; set; }

        public DateTime TanggalPembelian { get; set; }
    }
}
