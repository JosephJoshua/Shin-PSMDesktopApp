using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class EditServiceViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly IDamageEndpoint _damageEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private int _damageId = -1;
        private int _technicianId;

        private int _nomorNota;
        private string _namaPelanggan;
        private string _noHp;
        private string _tipeHp;
        private string _imei;
        private string _damage;
        private string _yangBelumDicek;
        private string _warna;
        private string _kataSandiPola;
        private string _kondisiHp;
        private string _isiKonfirmasi;

        private double _biaya;
        private int _discount;
        private double _dp;
        private double _tambahanBiaya;

        private bool _isBatteryChecked = false;
        private bool _isSimChecked = false;
        private bool _isMemoryChecked = false;
        private bool _isCondomChecked = false;

        private DateTime _tanggalKonfirmasi;
        private bool _sudahKonfirmasi = false;

        private BindingList<TechnicianModel> _technicians;
        private ServiceStatus _serviceStatuses;

        private TechnicianModel _selectedTechnician;
        private ServiceStatus _selectedStatus;

        public int NomorNota
        {
            get => _nomorNota;

            set
            {
                _nomorNota = value;
                NotifyOfPropertyChange(() => NomorNota);
            }
        }

        public string NamaPelanggan
        {
            get => _namaPelanggan;

            set
            {
                _namaPelanggan = value;

                NotifyOfPropertyChange(() => NamaPelanggan);
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

        public string TipeHp
        {
            get => _tipeHp;

            set
            {
                _tipeHp = value;

                NotifyOfPropertyChange(() => TipeHp);
                NotifyOfPropertyChange(() => CanSave);
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

        public string Damage
        {
            get => _damage;

            set
            {
                _damage = value;

                NotifyOfPropertyChange(() => Damage);
                NotifyOfPropertyChange(() => CanSave);
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

        public int DamageId
        {
            get => _damageId;

            set
            {
                _damageId = value;
                NotifyOfPropertyChange(() => DamageId);
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

        public int Discount
        {
            get => _discount;

            set
            {
                _discount = value;

                NotifyOfPropertyChange(() => Discount);
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
            get => (Biaya - (Biaya * ((double)Discount / 100))) + TambahanBiaya;
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

        public DateTime TanggalKonfirmasi
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

        public bool CanSave
        {
            get => !string.IsNullOrWhiteSpace(NamaPelanggan) && !string.IsNullOrWhiteSpace(TipeHp) && !string.IsNullOrWhiteSpace(Damage);
        }

        public EditServiceViewModel(IWindowManager windowManager, ITechnicianEndpoint technicianEndpoint,
                                    IDamageEndpoint damageEndpoint, IServiceEndpoint serviceEndpoint)
        {
            _windowManager = windowManager;
            _serviceEndpoint = serviceEndpoint;
            _damageEndpoint = damageEndpoint;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadComboboxes();
        }

        public async Task GetTechnicians()
        {
            List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
            Technicians = new BindingList<TechnicianModel>(technicianList);
        }

        public void SelectDamage()
        {
            SelectDamageViewModel selectDamageVM = IoC.Get<SelectDamageViewModel>();

            if (_windowManager.ShowDialog(selectDamageVM) == true)
            {
                DamageModel selectedDamage = selectDamageVM.SelectedDamage;

                Damage = selectedDamage.Kerusakan;
                DamageId = selectedDamage.Id;
            }
        }

        public async Task Save()
        {
            if (await UpdateService())
            {
                TryClose(true);
            }
        }

        public async Task Print()
        {
            await UpdateService();
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        public async Task LoadComboboxes()
        {
            await GetTechnicians();

            if (Technicians.Count > 0)
            {
                SelectedTechnician = Technicians.Where((d) => d.Id == _technicianId).FirstOrDefault() ?? Technicians[0];
            }
        }

        public async Task SetFieldValues(ServiceModel service)
        {
            NomorNota = service.NomorNota;
            NamaPelanggan = service.NamaPelanggan;
            NoHp = service.NoHp;
            TipeHp = service.TipeHp;
            Imei = service.Imei;
            KondisiHp = service.KondisiHp;
            YangBelumDicek = service.YangBelumDicek;
            Warna = service.Warna;
            KataSandiPola = service.KataSandiPola;
            SudahKonfirmasi = service.TanggalKonfirmasi != new DateTime(1753, 1, 1, 0, 0, 0);
            TanggalKonfirmasi = service.TanggalKonfirmasi;
            IsiKonfirmasi = service.IsiKonfirmasi;
            Biaya = (double)service.Biaya;
            Discount = service.Discount;
            Dp = (double)service.Dp;
            TambahanBiaya = (double)service.TambahanBiaya;
            DamageId = service.DamageId;

            // Get damage from id
            Damage = (await _damageEndpoint.GetAll()).Where(d => d.Id == DamageId).FirstOrDefault().Kerusakan;

            _technicianId = service.TechnicianId;

            SelectedStatus = Enum.GetValues(ServiceStatuses.GetType()).Cast<ServiceStatus>().Where((e) => e.Description() == service.StatusServisan).FirstOrDefault();

            if (service.Kelengkapan.Contains("Battery"))
            {
                IsBatteryChecked = true;
            }

            if (service.Kelengkapan.Contains("SIM"))
            {
                IsSimChecked = true;
            }

            if (service.Kelengkapan.Contains("Memory"))
            {
                IsMemoryChecked = true;
            }

            if (service.Kelengkapan.Contains("Condom"))
            {
                IsCondomChecked = true;
            }
        }

        public async Task<bool> UpdateService()
        {
            if ((SelectedStatus == ServiceStatus.TidakJadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil) && Biaya != 0)
            {
                DXMessageBox.Show("'Biaya' must be 0 if the service is cancelled. Please set 'Biaya' to be 0", "Edit service");
                return false;
            }

            string kelengkapan = "";

            if (IsBatteryChecked)
            {
                kelengkapan += "Battery ";
            }

            if (IsSimChecked)
            {
                kelengkapan += "SIM ";
            }

            if (IsMemoryChecked)
            {
                kelengkapan += "Memory ";
            }

            if (IsCondomChecked)
            {
                kelengkapan += "Condom ";
            }

            bool sudahDiambil = SelectedStatus == ServiceStatus.JadiSudahDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil;

            ServiceModel service = new ServiceModel
            {
                NomorNota = NomorNota,
                NamaPelanggan = NamaPelanggan,
                NoHp = NoHp,
                TipeHp = TipeHp,
                Imei = Imei,
                DamageId = DamageId,
                KondisiHp = KondisiHp,
                YangBelumDicek = YangBelumDicek,
                Kelengkapan = kelengkapan,
                Warna = Warna,
                KataSandiPola = KataSandiPola,
                TechnicianId = SelectedTechnician.Id,
                StatusServisan = SelectedStatus.Description(),
                TanggalKonfirmasi = SudahKonfirmasi ? TanggalKonfirmasi : DateTime.MinValue,
                IsiKonfirmasi = IsiKonfirmasi,
                Biaya = (decimal)Biaya,
                Discount = Discount,
                Dp = (decimal)Dp,
                TambahanBiaya = (decimal)TambahanBiaya,
                TanggalPengambilan = sudahDiambil ? DateTime.Now : DateTime.MinValue,
            };

            await _serviceEndpoint.Update(service);
            return true;
        }
    }
}
