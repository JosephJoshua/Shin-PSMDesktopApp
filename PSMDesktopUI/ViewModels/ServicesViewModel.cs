using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Globalization;
using DevExpress.Xpf.Grid;

namespace PSMDesktopUI.ViewModels
{
    public sealed class ServicesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IApiHelper _apiHelper;

        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;
        private readonly ISalesEndpoint _salesEndpoint;
        private readonly IDamageEndpoint _damageEndpoint;
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private bool _isLoading = false;

        private BindableCollection<ServiceModel> _services;
        private ServiceModel _selectedService;

        private SparepartModel _selectedSparepart;

        private string _searchText;
        private SearchType _searchTypes;
        private SearchType _selectedSearchType;

        public delegate void OnRefreshEventHandler();
        public event OnRefreshEventHandler OnRefresh;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddService);
                NotifyOfPropertyChange(() => CanAddSparepart);
                NotifyOfPropertyChange(() => CanEditService);
                NotifyOfPropertyChange(() => CanDeleteService);
                NotifyOfPropertyChange(() => CanPrintService);
            }
        }

        public BindableCollection<ServiceModel> Services
        {
            get => _services;

            set
            {
                _services = value;
                NotifyOfPropertyChange(() => Services);
            }
        }

        public ServiceModel SelectedService
        {
            get => _selectedService;

            set
            {
                _selectedService = value;

                NotifyOfPropertyChange(() => SelectedService);
                NotifyOfPropertyChange(() => SelectedServiceTechnician);
                NotifyOfPropertyChange(() => SelectedServiceDamage);
                NotifyOfPropertyChange(() => SelectedServiceSales);
                NotifyOfPropertyChange(() => CanAddSparepart);
                NotifyOfPropertyChange(() => CanEditService);
                NotifyOfPropertyChange(() => CanDeleteService);
                NotifyOfPropertyChange(() => CanPrintService);
                NotifyOfPropertyChange(() => ShowInfo);
            }
        }

        public string SelectedServiceTechnician
        {
            get
            {
                if (SelectedService == null) return null;

                return _technicianEndpoint.GetAll().Result.Where((t) => t.Id == SelectedService.TechnicianId).FirstOrDefault().Nama;
            }
        }

        public string SelectedServiceSales
        {
            get
            {
                if (SelectedService == null) return null;

                return _salesEndpoint.GetAll().Result.Where((t) => t.Id == SelectedService.SalesId).FirstOrDefault().Nama;
            }
        }

        public string SelectedServiceDamage
        {
            get
            {
                if (SelectedService == null) return null;

                return _damageEndpoint.GetAll().Result.Where((t) => t.Id == SelectedService.DamageId).FirstOrDefault().Kerusakan;
            }
        }

        public SparepartModel SelectedSparepart
        {
            get => _selectedSparepart;

            set
            {
                _selectedSparepart = value;

                NotifyOfPropertyChange(() => SelectedSparepart);
                NotifyOfPropertyChange(() => CanAddSparepart);
                NotifyOfPropertyChange(() => CanDeleteSparepart);
            }
        }

        public string SearchText
        {
            get => _searchText;

            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public SearchType SearchTypes
        {
            get => _searchTypes;

            set
            {
                _searchTypes = value;
                NotifyOfPropertyChange(() => SearchTypes);
            }
        }

        public SearchType SelectedSearchType
        {
            get => _selectedSearchType;

            set
            {
                _selectedSearchType = value;
                NotifyOfPropertyChange(() => SelectedSearchType);
            }
        }

        public bool CanAddService
        {
            get => !IsLoading;
        }

        public bool CanAddSparepart
        {
            get => !IsLoading && (SelectedService != null || SelectedSparepart != null);
        }

        public bool CanEditService
        {
            get => !IsLoading && SelectedService != null;
        }

        public bool CanDeleteService
        {
            get => !IsLoading && SelectedService != null;
        }

        public bool CanDeleteSparepart
        {
            get => !IsLoading && SelectedSparepart != null;
        }
        
        public bool CanPrintService
        {
            get => !IsLoading && SelectedService != null;
        }

        public bool ShowInfo
        {
            get => SelectedService != null;
        }

        public bool IsCustomerService
        {
            get => _apiHelper.LoggedInUser.Role.ToLower() == "Customer Service".ToLower();
        }

        public ServicesViewModel(IApiHelper apiHelper, IWindowManager windowManager, IServiceEndpoint serviceEndpoint,
                                 ITechnicianEndpoint technicianEndpoint, ISalesEndpoint salesEndpoint,
                                 IDamageEndpoint damageEndpoint, ISparepartEndpoint sparepartEndpoint)
        {
            DisplayName = "Services";

            _windowManager = windowManager;
            _apiHelper = apiHelper;
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
            _salesEndpoint = salesEndpoint;
            _damageEndpoint = damageEndpoint;
            _sparepartEndpoint = sparepartEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadServices();
        }

        public async Task OnMasterRowExpanding(RowAllowEventArgs args)
        {
            ServiceModel service = (ServiceModel)args.Row;
            
            if (service.Spareparts == null)
            {
                service.Spareparts = await _sparepartEndpoint.GetByService(service.NomorNota);
            }
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadServices();
            }
        }

        public async Task AddService()
        {
            AddServiceViewModel addServiceVM = IoC.Get<AddServiceViewModel>();

            if (_windowManager.ShowDialog(addServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task AddSparepart()
        {
            int nomorNota = SelectedService != null ? SelectedService.NomorNota : SelectedSparepart.NomorNota;

            AddSparepartViewModel addSparepartVM = IoC.Get<AddSparepartViewModel>();
            addSparepartVM.SetNomorNota(nomorNota);

            if (_windowManager.ShowDialog(addSparepartVM) == true)
            {
                await SyncServiceValues(nomorNota);
                await LoadServices();
            }
        }

        public async Task EditService()
        {
            if (IsCustomerService && !AskForCSPassword()) return;

            ServiceModel service = SelectedService;

            EditServiceViewModel editServiceVM = IoC.Get<EditServiceViewModel>();
            await editServiceVM.SetFieldValues(service);

            if (_windowManager.ShowDialog(editServiceVM) == true)
            {
                await SyncServiceValues(service.NomorNota);
                await LoadServices();
            }
        }

        public async Task EditStatus()
        {
            ServiceModel service = SelectedService;

            EditStatusViewModel editStatusVM = IoC.Get<EditStatusViewModel>();
            editStatusVM.SetFieldValues(service);

            if (_windowManager.ShowDialog(editStatusVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task DeleteService()
        {
            if (IsCustomerService && !AskForCSPassword()) return;

            if (DXMessageBox.Show("Are you sure you want to delete this service?", "Services", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _serviceEndpoint.Delete(SelectedService.NomorNota);
                await LoadServices();
            }
        }

        public async Task DeleteSparepart()
        {
            if (DXMessageBox.Show("Are you sure you want to delete this sparepart?", "Services", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _sparepartEndpoint.Delete(SelectedSparepart.Id);
                await SyncServiceValues(SelectedSparepart.NomorNota);
                await LoadServices();
            }
        }

        public async Task LoadServices()
        {
            if (IsLoading) return;

            IsLoading = true;

            List<ServiceModel> serviceList = await _serviceEndpoint.GetAll();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string searchText = SearchText.ToLower();

                switch (SelectedSearchType)
                {
                    case SearchType.NomorNota:
                        serviceList = serviceList.Where(s => s.NomorNota.ToString().ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.NamaPelanggan:
                        serviceList = serviceList.Where(s => s.NamaPelanggan.ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.NomorHp:
                        serviceList = serviceList.Where(s => s.NoHp.ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.TipeHp:
                        serviceList = serviceList.Where(s => s.TipeHp.ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.Status:
                        serviceList = serviceList.Where(s => s.StatusServisan.ToString().ToLower().Contains(searchText)).ToList();
                        break;
                }
            }

            Services = new BindableCollection<ServiceModel>(serviceList);
            IsLoading = false;

            OnRefresh?.Invoke();
        }

        public async Task PrintService()
        {
            if (SelectedService == null) return;

            string kerusakan = (await _damageEndpoint.GetAll()).Find(d => d.Id == SelectedService.DamageId).Kerusakan;
            string kelengkapan = SelectedService.Kelengkapan.Trim().Replace(" ", ", ").ToLower();
            kelengkapan = kelengkapan[0].ToString().ToUpper() + kelengkapan.Substring(1);

            ServiceInvoicePreviewViewModel invoicePreviewVM = IoC.Get<ServiceInvoicePreviewViewModel>();
            invoicePreviewVM.SetInvoiceModel(new ServiceInvoiceModel
            {
                NomorNota = SelectedService.NomorNota.ToString(),
                NamaPelanggan = SelectedService.NamaPelanggan,
                NoHp = SelectedService.NoHp,
                TipeHp = SelectedService.TipeHp,
                Imei = SelectedService.Imei ?? "",
                Kerusakan = kerusakan,
                TotalBiaya = SelectedService.TotalBiaya,
                Dp = SelectedService.Dp,
                Sisa = SelectedService.Sisa,
                Kelengkapan = kelengkapan,
                KondisiHp = SelectedService.KondisiHp,
                YangBelumDicek = SelectedService.YangBelumDicek,
                Tanggal = SelectedService.Tanggal.ToString("f", DateTimeFormatInfo.InvariantInfo),
            });

            _windowManager.ShowDialog(invoicePreviewVM);
        }

        public async Task SyncServiceValues(int nomorNota)
        {
            ServiceModel service = (await _serviceEndpoint.GetAll()).Where((s) => s.NomorNota == nomorNota).FirstOrDefault();

            decimal hargaSparepart = 0;

            foreach (SparepartModel s in await _sparepartEndpoint.GetByService(nomorNota))
            {
                hargaSparepart += s.Harga;
            }

            decimal labaRugi = service.TotalBiaya - hargaSparepart;

            ServiceModel newService = new ServiceModel
            {
                NomorNota = service.NomorNota,
                NamaPelanggan = service.NamaPelanggan,
                NoHp = service.NoHp,
                TipeHp = service.TipeHp,
                Imei = service.Imei,
                DamageId = service.DamageId,
                KondisiHp = service.KondisiHp,
                YangBelumDicek = service.YangBelumDicek,
                Kelengkapan = service.Kelengkapan,
                Warna = service.Warna,
                KataSandiPola = service.KataSandiPola,
                TechnicianId = service.TechnicianId,
                StatusServisan = service.StatusServisan,
                TanggalKonfirmasi = service.TanggalKonfirmasi,
                IsiKonfirmasi = service.IsiKonfirmasi,
                Biaya = service.Biaya,
                Discount = service.Discount,
                Dp = service.Dp,
                TambahanBiaya = service.TambahanBiaya,
                HargaSparepart = hargaSparepart,
                LabaRugi = labaRugi,
                TanggalPengambilan = service.TanggalPengambilan,
            };

            await _serviceEndpoint.Update(newService);
        }

        private bool AskForCSPassword()
        {
            return _windowManager.ShowDialog(IoC.Get<CSPasswordViewModel>()) == true;
        }
    }
}
