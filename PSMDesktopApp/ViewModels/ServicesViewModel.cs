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

namespace PSMDesktopApp.ViewModels
{
    public sealed class ServicesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IApiHelper _apiHelper;

        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;
        private readonly ISalesEndpoint _salesEndpoint;
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private bool _isLoading = false;

        private BindableCollection<ServiceModel> _services;
        private ServiceModel _selectedService;

        private SparepartModel _selectedSparepart;

        private string _searchText;
        private SearchType _searchTypes;
        private SearchType _selectedSearchType;

        private DateTime _startDate;
        private DateTime _endDate;

        private UserRole LoggedInUserRole
        {
            get => _apiHelper.LoggedInUser.role;
        }

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

                return _technicianEndpoint.GetById(SelectedService.TechnicianId).Result.Nama;
            }
        }

        public string SelectedServiceSales
        {
            get
            {
                if (SelectedService == null) return null;

                return _salesEndpoint.GetById(SelectedService.SalesId).Result.Nama;
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
            get => IsAdmin && !IsLoading && SelectedSparepart != null;
        }
        
        public bool CanPrintService
        {
            get => !IsLoading && SelectedService != null;
        }

        public bool ShowInfo
        {
            get => SelectedService != null;
        }

        public bool IsAdmin
        {
            get => LoggedInUserRole == UserRole.Admin;
        }

        public ServicesViewModel(IApiHelper apiHelper, IWindowManager windowManager, IServiceEndpoint serviceEndpoint,
                                 ITechnicianEndpoint technicianEndpoint, ISalesEndpoint salesEndpoint,
                                 ISparepartEndpoint sparepartEndpoint)
        {
            DisplayName = "Services";

            _windowManager = windowManager;
            _apiHelper = apiHelper;
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
            _salesEndpoint = salesEndpoint;
            _sparepartEndpoint = sparepartEndpoint;

            DateTime today = DateTime.Today;

            // Set start and end date to the first and last date of the month, respectively.
            _startDate = new DateTime(today.Year, today.Month, 1);
            _endDate = _startDate.AddMonths(1).AddDays(-1);
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
                service.Spareparts = await _sparepartEndpoint.GetByNomorNota(service.NomorNota);
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
                await LoadServices();
            }
        }

        public async Task EditService()
        {
            if (LoggedInUserRole == UserRole.CustomerService && !AskForCSPassword()) return;

            ServiceModel service = SelectedService;

            EditServiceViewModel editServiceVM = IoC.Get<EditServiceViewModel>();
            editServiceVM.SetFieldValues(service);

            if (_windowManager.ShowDialog(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        // TODO: Edit status dan kerusakan
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
            if (LoggedInUserRole == UserRole.CustomerService && !AskForCSPassword()) return;

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
                await LoadServices();
            }
        }

        public async Task LoadServices()
        {
            if (IsLoading) return;

            IsLoading = true;
            string searchText = (SearchText ?? "").Trim();

            // Make sure the start date's time is set to the start of the day (at 00:00:00).
            StartDate = StartDate.Date;

            // Make sure the end date's time is set to the end of the day (at 23:59:59).
            EndDate = EndDate.Date.AddDays(1).AddTicks(-1);

            List<ServiceModel> serviceList = await _serviceEndpoint.GetAll(searchText, SelectedSearchType, StartDate, EndDate);

            Services = new BindableCollection<ServiceModel>(serviceList);
            IsLoading = false;

            OnRefresh?.Invoke();
        }

        public void PrintService()
        {
            if (SelectedService == null) return;

            string kelengkapan = SelectedService.Kelengkapan?.Trim().Replace(" ", ", ");

            if (kelengkapan != null) kelengkapan = kelengkapan[0].ToString() + kelengkapan.Substring(1).ToLower();

            ServiceInvoicePreviewViewModel invoicePreviewVM = IoC.Get<ServiceInvoicePreviewViewModel>();
            invoicePreviewVM.SetInvoiceModel(new ServiceInvoiceModel
            {
                NomorNota = SelectedService.NomorNota.ToString(),
                NamaPelanggan = SelectedService.NamaPelanggan,
                NoHp = SelectedService.NoHp ?? "",
                TipeHp = SelectedService.TipeHp,
                Imei = SelectedService.Imei ?? "",
                Kerusakan = SelectedService.Kerusakan,
                TotalBiaya = SelectedService.TotalBiaya,
                Dp = SelectedService.Dp,
                Sisa = SelectedService.Sisa,
                Kelengkapan = kelengkapan ?? "",
                KondisiHp = SelectedService.KondisiHp ?? "",
                YangBelumDicek = SelectedService.YangBelumDicek ?? "",
                Tanggal = SelectedService.Tanggal.ToString("f", DateTimeFormatInfo.InvariantInfo),
            });

            _windowManager.ShowDialog(invoicePreviewVM);
        }

        private bool AskForCSPassword()
        {
            return _windowManager.ShowDialog(IoC.Get<CSPasswordViewModel>()) == true;
        }
    }
}
