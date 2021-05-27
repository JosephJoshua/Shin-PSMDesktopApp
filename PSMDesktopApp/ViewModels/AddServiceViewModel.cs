using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PSMDesktopApp.ViewModels
{
    public class AddServiceViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        private readonly ISalesEndpoint _salesEndpoint;
        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _showTextFields = false;
        private bool _showSalesGrid = true;
        private bool _isSalesLoading = false;

        private BindingList<SalesModel> _sales;
        private SalesModel _selectedSales;

        private string _namaPelanggan;
        private string _noHp;
        private string _tipeHp;
        private string _imei;
        private string _kerusakan;
        private string _yangBelumDicek;
        private string _warna;
        private string _kataSandiPola;
        private string _kondisiHp;
        private string _isiKonfirmasi;

        private int _salesId;

        private double _biaya;
        private int _diskon;
        private double _dp;
        private double _tambahanBiaya;

        private bool _isBatteryChecked = false;
        private bool _isSimChecked = false;
        private bool _isMemoryChecked = false;
        private bool _isCondomChecked = false;

        private DateTime? _tanggalKonfirmasi = DateTime.Now;
        private bool _sudahKonfirmasi = false;

        private BindingList<TechnicianModel> _technicians;
        private ServiceStatus _serviceStatuses;

        private TechnicianModel _selectedTechnician;
        private ServiceStatus _selectedStatus;

        public bool ShowTextFields
        {
            get => _showTextFields;

            set
            {
                _showTextFields = value;
                NotifyOfPropertyChange(() => ShowTextFields);
            }
        }

        public bool ShowSalesGrid
        {
            get => _showSalesGrid;

            set
            {
                _showSalesGrid = value;
                NotifyOfPropertyChange(() => ShowSalesGrid);
            }
        }

        public bool IsSalesLoading
        {
            get => _isSalesLoading;

            set
            {
                _isSalesLoading = value;
                NotifyOfPropertyChange(() => IsSalesLoading);
            }
        }

        public BindingList<SalesModel> Sales
        {
            get => _sales;

            set
            {
                _sales = value;
                NotifyOfPropertyChange(() => Sales);
            }
        }

        public SalesModel SelectedSales
        {
            get => _selectedSales;

            set
            {
                _selectedSales = value;
                NotifyOfPropertyChange(() => SelectedSales);
                NotifyOfPropertyChange(() => HasSelectedSales);
            }
        }

        public bool HasSelectedSales
        {
            get => SelectedSales != null;
        }

        public string NamaPelanggan
        {
            get => _namaPelanggan;

            set
            {
                _namaPelanggan = value;

                NotifyOfPropertyChange(() => NamaPelanggan);
                NotifyOfPropertyChange(() => CanAdd);
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

        public string TipeHp
        {
            get => _tipeHp;

            set
            {
                _tipeHp = value;

                NotifyOfPropertyChange(() => TipeHp);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string Imei
        {
            get => _imei;

            set
            {
                _imei = value;
                NotifyOfPropertyChange(() => Imei);
            }
        }

        public string Kerusakan
        {
            get => _kerusakan;
            
            set
            {
                _kerusakan = value;

                NotifyOfPropertyChange(() => Kerusakan);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string YangBelumDicek
        {
            get => _yangBelumDicek;

            set
            {
                _yangBelumDicek = value;
                NotifyOfPropertyChange(() => YangBelumDicek);
            }
        }

        public string Warna
        {
            get => _warna;

            set
            {
                _warna = value;
                NotifyOfPropertyChange(() => Warna);
            }
        }

        public string KataSandiPola
        {
            get => _kataSandiPola;

            set
            {
                _kataSandiPola = value;
                NotifyOfPropertyChange(() => KataSandiPola);
            }
        }

        public string KondisiHp
        {
            get => _kondisiHp;

            set
            {
                _kondisiHp = value;
                NotifyOfPropertyChange(() => KondisiHp);
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

        public int SalesId
        {
            get => _salesId;

            set
            {
                _salesId = value;
                NotifyOfPropertyChange(() => SalesId);
            }
        }

        public double Biaya
        {
            get => _biaya;

            set
            {
                _biaya = value;

                NotifyOfPropertyChange(() => Biaya);
                NotifyOfPropertyChange(() => TotalBiaya);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public int Diskon
        {
            get => _diskon;

            set
            {
                _diskon = value;

                NotifyOfPropertyChange(() => Diskon);
                NotifyOfPropertyChange(() => TotalBiaya);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public double Dp
        {
            get => _dp;

            set
            {
                _dp = value;

                NotifyOfPropertyChange(() => Dp);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public double TambahanBiaya
        {
            get => _tambahanBiaya;
            
            set
            {
                _tambahanBiaya = value;

                NotifyOfPropertyChange(() => TambahanBiaya);
                NotifyOfPropertyChange(() => TotalBiaya);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public double TotalBiaya
        {
            get => (100.0 - Diskon) / 100.0 * Biaya + TambahanBiaya;
        }

        public double Sisa
        {
            get => TotalBiaya - Dp;
        }

        public bool IsBatteryChecked
        {
            get => _isBatteryChecked;

            set
            {
                _isBatteryChecked = value;
                NotifyOfPropertyChange(() => IsBatteryChecked);
            }
        }

        public bool IsSimChecked
        {
            get => _isSimChecked;

            set
            {
                _isSimChecked = value;
                NotifyOfPropertyChange(() => IsSimChecked);
            }
        }

        public bool IsMemoryChecked
        {
            get => _isMemoryChecked;

            set
            {
                _isMemoryChecked = value;
                NotifyOfPropertyChange(() => IsMemoryChecked);
            }
        }

        public bool IsCondomChecked
        {
            get => _isCondomChecked;

            set
            {
                _isCondomChecked = value;
                NotifyOfPropertyChange(() => IsCondomChecked);
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

        public bool SudahKonfirmasi
        {
            get => _sudahKonfirmasi;

            set
            {
                _sudahKonfirmasi = value;
                NotifyOfPropertyChange(() => SudahKonfirmasi);
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

        public ServiceStatus ServiceStatuses
        {
            get => _serviceStatuses;

            set
            {
                _serviceStatuses = value;
                NotifyOfPropertyChange(() => ServiceStatuses);
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

        public ServiceStatus SelectedStatus
        {
            get => _selectedStatus;

            set
            {
                _selectedStatus = value;
                NotifyOfPropertyChange(() => SelectedStatus);
            }
        }

        public bool CanAdd
        {
            get => !string.IsNullOrWhiteSpace(NamaPelanggan) && !string.IsNullOrWhiteSpace(TipeHp) && !string.IsNullOrEmpty(Kerusakan);
        }

        public AddServiceViewModel(IWindowManager windowManager, ISalesEndpoint salesEndpoint,
                                   ITechnicianEndpoint technicianEndpoint, IServiceEndpoint serviceEndpoint)
        {
            _windowManager = windowManager;

            _salesEndpoint = salesEndpoint;
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadSales();
            await GetTechnicians();

            if (Technicians.Count > 0)
            {
                SelectedTechnician = Technicians[0];
            }

            NotifyOfPropertyChange(() => SelectedSales);
        }

        public async Task GetTechnicians()
        {
            List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
            Technicians = new BindingList<TechnicianModel>(technicianList);
        }

        public void ConfirmSales()
        {
            ShowSalesGrid = false;
            ShowTextFields = true;
            
            SalesId = SelectedSales.Id;
        }

        public async Task Add()
        {
            if (await AddService())
            {
                TryClose(true);
            }
        }

        public void Cancel()
        {
            TryClose(false);
        }

        public async Task LoadSales()
        {
            if (IsSalesLoading) return;

            IsSalesLoading = true;
            List<SalesModel> salesList = await _salesEndpoint.GetAll();

            IsSalesLoading = false;
            Sales = new BindingList<SalesModel>(salesList);
        }

        public async Task<bool> AddService()
        {
            bool tidakJadi = SelectedStatus == ServiceStatus.TidakJadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil;

            if (tidakJadi && (Biaya != 0 || TambahanBiaya != 0))
            {
                DXMessageBox.Show(
                    "Biaya dan tambahan biaya harus 0 jika servisan dibatalkan. Tolong ubah biaya dan tambahan biaya menjadi 0",
                    "Tambah servisan"
                );

                return false;
            }

            string kelengkapan = "";

            if (IsBatteryChecked)
            {
                kelengkapan += "Baterai ";
            }

            if (IsSimChecked)
            {
                kelengkapan += "SIM ";
            }

            if (IsMemoryChecked)
            {
                kelengkapan += "Memori ";
            }

            if (IsCondomChecked)
            {
                kelengkapan += "Condom ";
            }

            // The DevExpress DateEdit control will set the Kind to Unspecified if the date is selected from
            // the DateEdit dropdown. To counteract this, we must set it to Local manually before passing it into
            // the ServiceEndpoint to allow for proper timezone formatting.
            // https://supportcenter.devexpress.com/ticket/details/b145416/dateedit-datetime-s-kind-is-unspecified-when-a-date-is-selected-from-the-dropdown-calendar
            DateTime? tanggalKonfirmasi = new DateTime(TanggalKonfirmasi?.Ticks ?? 0, DateTimeKind.Local);

            ServiceModel service = new ServiceModel
            {
                NamaPelanggan = NamaPelanggan,
                NoHp = NoHp,
                TipeHp = TipeHp,
                Imei = Imei,
                Kerusakan = Kerusakan,
                KondisiHp = KondisiHp,
                YangBelumDicek = YangBelumDicek,
                Kelengkapan = kelengkapan,
                Warna = Warna,
                KataSandiPola = KataSandiPola,
                TechnicianId = SelectedTechnician.Id,
                SalesId = SalesId,
                StatusServisan = SelectedStatus.Description(),
                TanggalKonfirmasi = SudahKonfirmasi ? tanggalKonfirmasi : null,
                IsiKonfirmasi = SudahKonfirmasi ? IsiKonfirmasi : "",
                Biaya = (decimal)Biaya,
                Diskon = Diskon,
                Dp = (decimal)Dp,
                TambahanBiaya = (decimal)TambahanBiaya,
            };

            await _serviceEndpoint.Insert(service);
            return true;
        }
    }
}
