using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PSMDesktopApp.ViewModels
{
    public class EditServiceLimitedViewModel : Screen
    {
        private readonly IServiceEndpoint _serviceEndpoint;

        private ServiceModel _oldService;
        private int _nomorNota;

        private ServiceStatus _serviceStatuses;
        private ServiceStatus _selectedStatus;

        private bool _sudahKonfirmasi;
        private string _isiKonfirmasi;
        private DateTime? _tanggalKonfirmasi;

        public int NomorNota
        {
            get => _nomorNota;

            set
            {
                _nomorNota = value;
                NotifyOfPropertyChange(() => NomorNota);
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

        public EditServiceLimitedViewModel(IServiceEndpoint serviceEndpoint)
        {
            _serviceEndpoint = serviceEndpoint;
        }

        public void SetFieldValues(ServiceModel service)
        {
            _oldService = service;

            NomorNota = service.NomorNota;
            SudahKonfirmasi = service.TanggalKonfirmasi != null;
            IsiKonfirmasi = service.IsiKonfirmasi;
            TanggalKonfirmasi = service.TanggalKonfirmasi;

            SelectedStatus = Enum.GetValues(ServiceStatuses.GetType()).Cast<ServiceStatus>().Where((e) => e.Description() == service.StatusServisan).FirstOrDefault();
        }

        public async Task<bool> UpdateService()
        {
            ServiceStatus oldStatus = Enum.GetValues(ServiceStatuses.GetType()).Cast<ServiceStatus>().Where(e => e.Description() ==
                _oldService.StatusServisan).FirstOrDefault();

            if (oldStatus == ServiceStatus.JadiSudahDiambil && (SelectedStatus == ServiceStatus.TidakJadiBelumDiambil ||
                SelectedStatus == ServiceStatus.TidakJadiSudahDiambil))
            {
                DXMessageBox.Show("Tidak bisa ubah servisan dari 'Jadi (Sudah diambil)' menjadi 'Tidak jadi'", "Edit servisan");
                return false;
            }

            if ((oldStatus == ServiceStatus.JadiSudahDiambil || oldStatus == ServiceStatus.TidakJadiSudahDiambil) &&
                (SelectedStatus == ServiceStatus.JadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiBelumDiambil))
            {
                DXMessageBox.Show("Tidak bisa ubah servisan dari 'Sudah diambil' menjadi 'Belum diambil'", "Edit servisan");
                return false;
            }

            if (SelectedStatus == ServiceStatus.TidakJadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil)
            {
                DXMessageBox.Show("Biaya harus 0 jika servisan dibatalkan", "Edit servisan");
                return false;
            }

            // The DevExpress DateEdit control will set the Kind to Unspecified if the date is selected from
            // the DateEdit dropdown. To counteract this, we must set it to Local manually before passing it into
            // the ServiceEndpoint to allow for proper timezone formatting.
            // https://supportcenter.devexpress.com/ticket/details/b145416/dateedit-datetime-s-kind-is-unspecified-when-a-date-is-selected-from-the-dropdown-calendar
            DateTime? tanggalKonfirmasi = new DateTime(TanggalKonfirmasi?.Ticks ?? 0, DateTimeKind.Local);

            _oldService.StatusServisan = SelectedStatus.Description();
            _oldService.IsiKonfirmasi = SudahKonfirmasi ? IsiKonfirmasi : "";
            _oldService.TanggalKonfirmasi = SudahKonfirmasi ? tanggalKonfirmasi : null;

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
