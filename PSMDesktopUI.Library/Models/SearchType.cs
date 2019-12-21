using System.ComponentModel;

namespace PSMDesktopUI.Library.Models
{
    public enum SearchType
    {
        [Description("Nomor Nota")]
        NomorNota,

        [Description("Nama Pelanggan")]
        NamaPelanggan,

        [Description("Nomor Hp")]
        NomorHp,

        [Description("Status")]
        Status,
    }
}
