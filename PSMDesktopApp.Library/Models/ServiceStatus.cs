using System.ComponentModel;

namespace PSMDesktopApp.Library.Models
{
    public enum ServiceStatus
    {
        [Description("Sedang dikerjakan")]
        SedangDikerjakan,

        [Description("Jadi (Belum diambil)")]
        JadiBelumDiambil,

        [Description("Jadi (Sudah diambil)")]
        JadiSudahDiambil,

        [Description("Tidak jadi (Belum diambil)")]
        TidakJadiBelumDiambil,

        [Description("Tidak jadi (Sudah diambil)")]
        TidakJadiSudahDiambil,

        [Description("Pending")]
        Pending,
    }
}
