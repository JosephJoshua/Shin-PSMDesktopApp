using Caliburn.Micro;
using DevExpress.Xpf.Core;
using Newtonsoft.Json;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopApp.ViewModels
{
    public class EditServiceLimitedViewModel : Screen
    {
        private readonly ILog _logger;

        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private ServiceModel _oldService;
        private int _nomorNota;

        private int _technicianId;

        private string _kerusakan;
        private string _noHp;

        private ServiceStatus _serviceStatuses;
        private ServiceStatus _selectedStatus;

        private bool _sudahKonfirmasi;
        private string _isiKonfirmasi;
        private DateTime? _tanggalKonfirmasi;

        private double _dp;
        private double _tambahanBiaya;

        private BindingList<TechnicianModel> _technicians;
        private TechnicianModel _selectedTechnician;

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
                NotifyOfPropertyChange(() => CanSave);
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

        public BindingList<TechnicianModel> Technicians
        {
            get => _technicians;

            set
            {
                _technicians = value;
                NotifyOfPropertyChange(() => Technicians);
            }
        }

        public TechnicianModel SelectedTechnician
        {
            get => _selectedTechnician;

            set
            {
                _selectedTechnician = value;
                NotifyOfPropertyChange(() => SelectedTechnician);
            }
        }

        public bool CanSave => !string.IsNullOrEmpty(Kerusakan);

        public EditServiceLimitedViewModel(IServiceEndpoint serviceEndpoint, ITechnicianEndpoint technicianEndpoint)
        {
            _logger = LogManager.GetLog(typeof(EditServiceLimitedViewModel));
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadTechnicians();
        }

        public async Task LoadTechnicians()
        {
            try
            {
                List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
                Technicians = new BindingList<TechnicianModel>(technicianList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            if (Technicians.Count > 0)
            {
                SelectedTechnician = Technicians.Where(t => t.Id == _technicianId).FirstOrDefault() ?? Technicians[0];
            }
        }

        public void SetFieldValues(ServiceModel service)
        {
            // Clone service object without changes being reflected
            string json = JsonConvert.SerializeObject(service);
            _oldService = JsonConvert.DeserializeObject<ServiceModel>(json);

            NomorNota = service.NomorNota;
            Kerusakan = service.Kerusakan;
            NoHp = service.NoHp;
            SudahKonfirmasi = service.TanggalKonfirmasi != null;
            IsiKonfirmasi = service.IsiKonfirmasi;
            TanggalKonfirmasi = service.TanggalKonfirmasi;
            Dp = (double)service.Dp;
            TambahanBiaya = (double)service.TambahanBiaya;

            _technicianId = service.TechnicianId;

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
                DXMessageBox.Show("Tidak bisa mengubah servisan dari 'Jadi (Sudah diambil)' menjadi 'Tidak jadi'", "Edit servisan");
                return false;
            }

            if (wasSudahDiambil && belumDiambil)
            {
                DXMessageBox.Show("Tidak bisa mengubah servisan dari 'Sudah diambil' menjadi 'Belum diambil'", "Edit servisan");
                return false;
            }

            if (tidakJadi && (_oldService.Biaya != 0 || TambahanBiaya != 0))
            {
                if (DXMessageBox.Show(
                    "Biaya dan tambahan biaya harus 0 jika servisan ini ingin dibatalkan. Apakah anda ingin mengubahnya menjadi 0 secara otomatis?", 
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
            _oldService.TechnicianId = SelectedTechnician.Id;

            try
            {
                await _serviceEndpoint.Update(_oldService, NomorNota);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }

            return true;
        }

        public async Task Save()
        {
            if (await UpdateService())
            {
                await TryCloseAsync(true);
            }
        }

        public async Task Cancel()
        {
            await TryCloseAsync(false);
        }
    }
}
