using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Helpers;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class AddServiceViewModel : Screen
    {
        private readonly IInternetConnectionHelper _internetConnectionHelper;

        private readonly IMemberEndpoint _memberEndpoint;
        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly IDamageEndpoint _damageEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _showTextFields = false;
        private bool _showMemberGrid = false;
        private bool _memberIsCustomer;

        private bool _isLoadingMembers = false;

        private BindingList<MemberModel> _members;
        private MemberModel _selectedMember;

        private string _namaPelanggan;
        private string _noHp;
        private string _tipeHp;
        private string _imei;
        private string _yangBelumDicek;
        private string _warna;
        private string _kataSandiPola;
        private string _isiKonfirmasi;

        private double _biaya;
        private int _discount;
        private double _dp;
        private double _tambahanBiaya;

        private bool _isBatteryChecked = false;
        private bool _isSimChecked = false;
        private bool _isMemoryChecked = false;
        private bool _isCondomChecked = false;

        private DateTime _tanggalKonfirmasi = DateTime.Now;
        private bool _sudahKonfirmasi = false;

        private BindingList<TechnicianModel> _technicians;
        private BindingList<DamageModel> _damages;
        private ServiceStatus _serviceStatuses;

        private TechnicianModel _selectedTechnician;
        private DamageModel _selectedDamage;
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

        public bool ShowMemberGrid
        {
            get => _showMemberGrid;

            set
            {
                _showMemberGrid = value;
                NotifyOfPropertyChange(() => ShowMemberGrid);
            }
        }

        public bool ShowMemberButtons
        {
            get => !ShowTextFields && !ShowMemberGrid;
        }

        public bool MemberIsCustomer
        {
            get => _memberIsCustomer;

            set
            {
                _memberIsCustomer = value;
                NotifyOfPropertyChange(() => MemberIsCustomer);
            }
        }

        public bool IsLoadingMembers
        {
            get => _isLoadingMembers;

            set
            {
                _isLoadingMembers = value;
                NotifyOfPropertyChange(() => IsLoadingMembers);
            }
        }

        public BindingList<MemberModel> Members
        {
            get => _members;
            
            set
            {
                _members = value;
                NotifyOfPropertyChange(() => Members);
            }
        }

        public MemberModel SelectedMember
        {
            get => _selectedMember;

            set
            {
                _selectedMember = value;

                NotifyOfPropertyChange(() => SelectedMember);
                NotifyOfPropertyChange(() => HasSelectedMember);
            }
        }

        public bool HasSelectedMember
        {
            get => SelectedMember != null;
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

        public string IsiKonfirmasi
        {
            get => _isiKonfirmasi;

            set
            {
                _isiKonfirmasi = value;
                NotifyOfPropertyChange(() => IsiKonfirmasi);
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

        public double LabaRugi
        {
            get => 0; // TODO: get => TotalBiaya - Pengeluaran
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

        public BindingList<DamageModel> Damages
        {
            get => _damages;

            set
            {
                _damages = value;
                NotifyOfPropertyChange(() => Damages);
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

        public DamageModel SelectedDamage
        {
            get => _selectedDamage;

            set
            {
                _selectedDamage = value;
                NotifyOfPropertyChange(() => SelectedDamage);
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
            get => !string.IsNullOrWhiteSpace(NamaPelanggan) && !string.IsNullOrWhiteSpace(TipeHp);
        }

        public bool CanPrint
        {
            get => !string.IsNullOrWhiteSpace(NamaPelanggan) && !string.IsNullOrWhiteSpace(TipeHp);
        }

        public AddServiceViewModel(IInternetConnectionHelper internetConnectionHelper, IMemberEndpoint memberEndpoint, 
                                   ITechnicianEndpoint technicianEndpoint, IDamageEndpoint damageEndpoint, IServiceEndpoint serviceEndpoint)
        {
            _internetConnectionHelper = internetConnectionHelper;

            _memberEndpoint = memberEndpoint;
            _serviceEndpoint = serviceEndpoint;
            _damageEndpoint = damageEndpoint;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await GetTechnicians();

            if (Technicians.Count > 0)
            {
                SelectedTechnician = Technicians[0];
            }

            await GetDamages();

            if (Damages.Count > 0)
            {
                SelectedDamage = Damages[0];
            }
        }

        public async Task GetTechnicians()
        {
            List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
            Technicians = new BindingList<TechnicianModel>(technicianList);
        }

        public async Task GetDamages()
        {
            List<DamageModel> damageList = await _damageEndpoint.GetAll();
            Damages = new BindingList<DamageModel>(damageList);
        }

        public async Task SelectMember()
        {
            ShowTextFields = false;
            ShowMemberGrid = true;

            NotifyOfPropertyChange(() => ShowMemberButtons);

            await LoadMembers();
        }

        public void SelectNonMember()
        {
            ShowMemberGrid = false;
            ShowTextFields = true;

            NotifyOfPropertyChange(() => ShowMemberButtons);
        }

        public void ConfirmMember()
        {
            ShowMemberGrid = false;
            ShowTextFields = true;

            NotifyOfPropertyChange(() => ShowMemberButtons);

            MemberIsCustomer = true;

            NamaPelanggan = SelectedMember.Nama;
            NoHp = SelectedMember.NoHp ?? "";
        }

        public async Task Add()
        {
            await AddService();
            TryClose(true);
        }

        public async Task Print()
        {
            await AddService();
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        public async Task LoadMembers()
        {
            if (IsLoadingMembers || !_internetConnectionHelper.HasInternetConnection) return;

            IsLoadingMembers = true;
            List<MemberModel> memberList = await _memberEndpoint.GetAll();

            IsLoadingMembers = false;
            Members = new BindingList<MemberModel>(memberList);
        }

        public async Task AddService()
        {
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

            ServiceModel service = new ServiceModel
            {
                NamaPelanggan = NamaPelanggan,
                NoHp = NoHp,
                TipeHp = TipeHp,
                Imei = Imei,
                DamageId = SelectedDamage.Id,
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
                HargaSparepart = 0,
                LabaRugi = (decimal)LabaRugi,
                TanggalPengambilan = DateTime.MinValue,
            };

            await _serviceEndpoint.Insert(service);
        }
    }
}
