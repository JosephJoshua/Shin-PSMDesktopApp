using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using DevExpress.Xpf.Grid;
using System;
using DevExpress.Data.Extensions;
using System.Linq;
using PSMDesktopApp.Library.Helpers;

namespace PSMDesktopApp.ViewModels
{
    public sealed class ServicesViewModel : Screen
    {
        private readonly ILog _logger;

        private readonly IWindowManager _windowManager;
        private readonly IApiHelper _apiHelper;
        private readonly IConnectionHelper _connectionHelper;

        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private bool _isLoading = false;
        private bool _onlyShowWIP = true;

        private BindableCollection<ServiceModel> _services;
        private ServiceModel _selectedService;

        private SparepartModel _selectedSparepart;

        private string _searchText;
        private SearchType _searchTypes;
        private SearchType _selectedSearchType;

        private DateTime _startDate;
        private DateTime _endDate;

        private UserRole LoggedInUserRole => _apiHelper.LoggedInUser.role;

        private bool _isFirstLoad = true;

        public delegate void BeforeRefreshEventHandler();
        public event BeforeRefreshEventHandler BeforeRefresh;

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

        public bool OnlyShowWIP
        {
            get => _onlyShowWIP;

            set
            {
                _onlyShowWIP = value;
                NotifyOfPropertyChange(() => OnlyShowWIP);

                DateTime today = DateTime.Today;

                if (_isFirstLoad) return;

                if (value)
                {
                    StartDate = new DateTime(today.Year, 1, 1);
                    EndDate = StartDate.AddMonths(12).AddTicks(-1);
                }
                else
                {
                    StartDate = new DateTime(today.Year, today.Month, 1);
                    EndDate = _startDate.AddMonths(1).AddTicks(-1);
                }

                _ = LoadServices();
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
                NotifyOfPropertyChange(() => CanAddSparepart);
                NotifyOfPropertyChange(() => CanEditService);
                NotifyOfPropertyChange(() => CanDeleteService);
                NotifyOfPropertyChange(() => CanPrintService);
                NotifyOfPropertyChange(() => ShowInfo);
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

        public DateTime StartDate
        {
            get => _startDate;

            set
            {
                _startDate = value;
                NotifyOfPropertyChange(() => StartDate);
            }
        }
        
        public DateTime EndDate
        {
            get => _endDate;

            set
            {
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);
            }
        }

        public bool CanAddService => !IsBuyer && !IsLoading;

        public bool CanAddSparepart => !IsCustomerService && !IsLoading && (SelectedService != null || SelectedSparepart != null);

        public bool CanEditService => !IsBuyer && !IsLoading && SelectedService != null;

        public bool CanDeleteService => IsAdmin && !IsLoading && SelectedService != null;

        public bool CanDeleteSparepart => !IsCustomerService && !IsLoading && SelectedSparepart != null;

        public bool CanPrintService => !IsBuyer && !IsLoading && SelectedService != null;

        public bool ShowInfo => SelectedService != null;

        public bool IsAdmin => LoggedInUserRole == UserRole.Admin;

        public bool IsCustomerService => LoggedInUserRole == UserRole.CustomerService;

        public bool IsBuyer => LoggedInUserRole == UserRole.Buyer;

        public ServicesViewModel(IApiHelper apiHelper, IWindowManager windowManager, IConnectionHelper connectionHelper,
                                 IServiceEndpoint serviceEndpoint, ISparepartEndpoint sparepartEndpoint)
        {
            DisplayName = "Servisan";

            _logger = LogManager.GetLog(typeof(ServicesViewModel));
            _windowManager = windowManager;
            _apiHelper = apiHelper;
            _connectionHelper = connectionHelper;
            _serviceEndpoint = serviceEndpoint;
            _sparepartEndpoint = sparepartEndpoint;

            DateTime today = DateTime.Today;

            StartDate = new DateTime(today.Year, 1, 1);
            EndDate = StartDate.AddMonths(12).AddTicks(-1);
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
                // We could use LoadSparepart(service.NomorNota) but doing this eliminates the need for searching through the Services list.
                try 
                {
                    service.Spareparts = await _sparepartEndpoint.GetByNomorNota(service.NomorNota);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
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

            if (await _windowManager.ShowDialogAsync(addServiceVM) == true)
            {
                if (addServiceVM.NomorNota != -1 &&
                    DXMessageBox.Show("Apakah anda ingin mencetak servisan ini?", "Tambah Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        ServiceModel newService = await _serviceEndpoint.GetByNomorNota(addServiceVM.NomorNota);
                        await PrintService(newService);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                }

                await LoadServices();
            }
        }

        public async Task AddSparepart()
        {
            int nomorNota = SelectedService != null ? SelectedService.NomorNota : SelectedSparepart.NomorNota;

            AddSparepartViewModel addSparepartVM = IoC.Get<AddSparepartViewModel>();
            addSparepartVM.SetNomorNota(nomorNota);

            if (await _windowManager.ShowDialogAsync(addSparepartVM) == true)
            {
                await LoadSparepart(nomorNota);
            }
        }

        public async Task EditService()
        {
            ServiceModel service = SelectedService;

            EditServiceViewModel editServiceVM = IoC.Get<EditServiceViewModel>();
            editServiceVM.SetFieldValues(service);

            if (await _windowManager.ShowDialogAsync(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task EditServiceLimited()
        {
            ServiceModel service = SelectedService;

            EditServiceLimitedViewModel editServiceVM = IoC.Get<EditServiceLimitedViewModel>();
            editServiceVM.SetFieldValues(service);

            if (await _windowManager.ShowDialogAsync(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task DeleteService()
        {
            if (DXMessageBox.Show(
                    "Apakah anda yakin ingin menghapus servisan ini? Sparepart-sparepart yang ditambah dibawah servisan ini akan dihapus juga.",
                    "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes &&
                DXMessageBox.Show(
                    "Apakah anda YAKIN ingin menghapus servisan ini? Sparepart-sparepart yang ditambah dibawah servisan ini akan dihapus juga.",
                    "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes &&
                DXMessageBox.Show(
                    "Apakah anda SANGAT YAKIN ingin menghapus servisan ini? Sparepart-sparepart yang ditambah dibawah servisan ini akan dihapus juga.",
                    "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _serviceEndpoint.Delete(SelectedService.NomorNota);
                    await LoadServices();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }

        public async Task DeleteSparepart()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus sparepart ini?", "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int nomorNota = SelectedSparepart.NomorNota;

                try
                {
                    await _sparepartEndpoint.Delete(SelectedSparepart.Id);
                    await LoadSparepart(nomorNota);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }

        public async Task LoadServices()
        {
            if (IsLoading || (!_isFirstLoad && !_connectionHelper.WasConnectionSuccessful)) return;

            BeforeRefresh?.Invoke();

            IsLoading = true;

            bool shouldForceStatus = OnlyShowWIP && string.IsNullOrWhiteSpace(SearchText);

            SearchType searchType = shouldForceStatus ? SearchType.Status : SelectedSearchType;
            string searchText = shouldForceStatus ? "Sedang dikerjakan" : (SearchText ?? "").Trim();

            // Make sure the start date's time is set to the start of the day (at 00:00:00).
            StartDate = StartDate.Date;

            // Make sure the end date's time is set to the end of the day (at 23:59:59).
            EndDate = EndDate.Date.AddDays(1).AddTicks(-1);

            try 
            {
                List<ServiceModel> serviceList = await _serviceEndpoint.GetAll(searchText, searchType, StartDate, EndDate);
                var serviceCollection = new BindableCollection<ServiceModel>(serviceList);

                if (Services?.SequenceEqual(serviceCollection) ?? false)
                {
                    IsLoading = false;
                    return;
                }

                Services = serviceCollection;
                IsLoading = false;

                OnRefresh?.Invoke();

                _isFirstLoad = false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                IsLoading = false;

                return;
            }
        }

        public async Task LoadSparepart(int nomorNota)
        {
            int index = Services.FindIndex(s => s.NomorNota == nomorNota);

            try
            {
                Services[index] = await _serviceEndpoint.GetByNomorNota(nomorNota);
                Services[index].Spareparts = await _sparepartEndpoint.GetByNomorNota(nomorNota);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public async Task PrintSelectedService()
        {
            if (SelectedService == null) return;
            await PrintService(SelectedService);
        }

        private async Task PrintService(ServiceModel service)
        {
            string kelengkapan = service.Kelengkapan?.Trim().Replace(" ", ", ");

            if (!string.IsNullOrWhiteSpace(kelengkapan) && kelengkapan.Length > 1)
            {
                kelengkapan = kelengkapan[0].ToString() + kelengkapan.Substring(1).ToLower();
                kelengkapan = kelengkapan.Replace("sim", "SIM").Replace("Sim", "SIM");
            }

            ServiceInvoicePreviewViewModel invoicePreviewVM = IoC.Get<ServiceInvoicePreviewViewModel>();
            invoicePreviewVM.SetInvoiceModel(new ServiceInvoiceModel
            {
                NomorNota = service.NomorNota.ToString(),
                NamaPelanggan = service.NamaPelanggan,
                NoHp = service.NoHp,
                TipeHp = service.TipeHp,
                Imei = service.Imei,
                Kerusakan = service.Kerusakan,
                TotalBiaya = service.TotalBiaya,
                Dp = service.Dp,
                Sisa = service.Sisa,
                Kelengkapan = kelengkapan,
                KondisiHp = service.KondisiHp,
                YangBelumDicek = service.YangBelumDicek,
                Tanggal = service.Tanggal.ToString(DateTimeFormatInfo.CurrentInfo.LongDatePattern),
            });

            await _windowManager.ShowDialogAsync(invoicePreviewVM);
        }
    }
}
