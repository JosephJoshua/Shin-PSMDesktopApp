using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopApp.ViewModels
{
    public class EditServiceLimitedViewModel : Screen
    {
        private readonly IServiceEndpoint _serviceEndpoint;

        private ServiceModel _oldService;
        private int _nomorNota;

        private string _kerusakan;
        private string _noHp;

        private ServiceStatus _serviceStatuses;
        private ServiceStatus _selectedStatus;

        private bool _sudahKonfirmasi;
        private string _isiKonfirmasi;
        private DateTime? _tanggalKonfirmasi;

        private double _dp;
        private double _tambahanBiaya;

        public int NomorNota
        {
            get => _nomorNota;

            set
            {
                _nomorNota = value;
                NotifyOfPropertyChange(() => NomorNota);
            }
        }

        public string Kerusakan
        {
            get => _kerusakan;

            set
            {
                _kerusakan = value;
                NotifyOfPropertyChange(() => Kerusakan);
            }
        }

        public string NoHp
        {
            get => _noHp;

            set
            {
                _noHp = value;
                NotifyOfPropertyChange(() => NoHp);
            }
        }

        public ServiceStatus ServiceStatuses
        {
            get => _serviceStatuses;

            set
            {
                _serviceStatuses = value;
                NotifyOfPropertyChange(() => ServiceStatuses);
            }
        }

        public ServiceStatus SelectedStatus
        {
            get => _selectedStatus;

            set
            {
                _selectedStatus = value;
                NotifyOfPropertyChange(() => SelectedStatus);
            }
        }

        public bool SudahKonfirmasi
        {
            get => _sudahKonfirmasi;

            set
            {
                _sudahKonfirmasi = value;
                NotifyOfPropertyChange(() => SudahKonfirmasi);

                if (SudahKonfirmasi && TanggalKonfirmasi == null)
                {
                    TanggalKonfirmasi = DateTime.Now;
                }
            }
        }

        public string IsiKonfirmasi
        {
            get => _isiKonfirmasi;

            set
            {
                _isiKonfirmasi = value;
                NotifyOfPropertyChange(() => IsiKonfirmasi);
            }
        }

        public DateTime? TanggalKonfirmasi
        {
            get => _tanggalKonfirmasi;

            set
            {
                _tanggalKonfirmasi = value;
                NotifyOfPropertyChange(() => TanggalKonfirmasi);
            }
        }

        public double Dp
        {
            get => _dp;

            set
            {
                _dp = value;
                NotifyOfPropertyChange(() => Dp);
            }
        }

        public double TambahanBiaya
        {
            get => _tambahanBiaya;

            set
            {
                _tambahanBiaya = value;
                NotifyOfPropertyChange(() => TambahanBiaya);
            }
        }

        public EditServiceLimitedViewModel(IServiceEndpoint serviceEndpoint)
        {
            _serviceEndpoint = serviceEndpoint;
        }

        public void SetFieldValues(ServiceModel service)
        {
            _oldService = service;

            NomorNota = service.NomorNota;
            Kerusakan = service.Kerusakan;
            NoHp = service.NoHp;
            SudahKonfirmasi = service.TanggalKonfirmasi != null;
            IsiKonfirmasi = service.IsiKonfirmasi;
            TanggalKonfirmasi = service.TanggalKonfirmasi;
            Dp = (double)service.Dp;
            TambahanBiaya = (double)service.TambahanBiaya;

            SelectedStatus = Enum.GetValues(ServiceStatuses.GetType()).Cast<ServiceStatus>().Where((e) => e.Description() == service.StatusServisan).FirstOrDefault();
        }

        public async Task<bool> UpdateService()
        {
            ServiceStatus oldStatus = Enum.GetValues(ServiceStatuses.GetType()).Cast<ServiceStatus>().Where(e => e.Description() ==
                _oldService.StatusServisan).FirstOrDefault();

            bool wasSudahDiambil = oldStatus == ServiceStatus.JadiSudahDiambil || oldStatus == ServiceStatus.TidakJadiSudahDiambil;
            bool belumDiambil = SelectedStatus == ServiceStatus.JadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiBelumDiambil;
            bool tidakJadi = SelectedStatus == ServiceStatus.TidakJadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil;

            if (oldStatus == ServiceStatus.JadiSudahDiambil && tidakJadi)
            {
                DXMessageBox.Show("Tidak bisa ubah servisan dari 'Jadi (Sudah diambil)' menjadi 'Tidak jadi'", "Edit servisan");
                return false;
            }

            if (wasSudahDiambil && belumDiambil)
            {
                DXMessageBox.Show("Tidak bisa ubah servisan dari 'Sudah diambil' menjadi 'Belum diambil'", "Edit servisan");
                return false;
            }

            if (tidakJadi && (_oldService.Biaya != 0 || TambahanBiaya != 0))
            {
                if (DXMessageBox.Show(
                    "Biaya dan tambahan biaya harus 0 jika servisan dibatalkan. Apakah anda ingin mengubahnya menjadi 0 secara otomatis?", 
                    "Edit servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _oldService.Biaya = 0;
                    TambahanBiaya = 0;
                }
                else
                {
                    return false;
                }
            }

            // The DevExpress DateEdit control will set the Kind to Unspecified if the date is selected from
            // the DateEdit dropdown. To counteract this, we must set it to Local manually before passing it into
            // the ServiceEndpoint to allow for proper timezone formatting.
            // https://supportcenter.devexpress.com/ticket/details/b145416/dateedit-datetime-s-kind-is-unspecified-when-a-date-is-selected-from-the-dropdown-calendar
            DateTime? tanggalKonfirmasi = new DateTime(TanggalKonfirmasi?.Ticks ?? 0, DateTimeKind.Local);

            _oldService.Kerusakan = Kerusakan;
            _oldService.NoHp = NoHp;
            _oldService.StatusServisan = SelectedStatus.Description();
            _oldService.IsiKonfirmasi = SudahKonfirmasi ? IsiKonfirmasi : "";
            _oldService.TanggalKonfirmasi = SudahKonfirmasi ? tanggalKonfirmasi : null;
            _oldService.Dp = (decimal)Dp;
            _oldService.TambahanBiaya = (decimal)TambahanBiaya;

            await _serviceEndpoint.Update(_oldService, NomorNota);
            return true;
        }

        public async Task Save()
        {
            if (await UpdateService())
            {
                TryClose(true);
            }
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
