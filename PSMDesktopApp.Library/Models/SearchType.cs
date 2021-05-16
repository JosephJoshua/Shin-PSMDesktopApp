using System.ComponentModel;

namespace PSMDesktopApp.Library.Models
{
    public enum SearchType
    {
        [Description("Nomor Nota")]
        NomorNota,

        [Description("Nama Pelanggan")]
        NamaPelanggan,

        [Description("Nomor Hp")]
        NomorHp,

        [Description("Tipe Hp")]
        TipeHp,

        [Description("Status")]
        Status,
    }
}
