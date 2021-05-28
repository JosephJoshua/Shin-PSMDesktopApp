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

        private bool _showAllColumns = false;
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

        public bool ShowAllColumns
        {
            get => _showAllColumns;

            set
            {
                _showAllColumns = value;
                NotifyOfPropertyChange(() => ShowAllColumns);
            }
        }

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
            get => IsAdmin && !IsLoading && SelectedService != null;
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

        public bool IsCustomerService
        {
            get => LoggedInUserRole == UserRole.CustomerService;
        }

        public ServicesViewModel(IApiHelper apiHelper, IWindowManager windowManager, IServiceEndpoint serviceEndpoint,
                                 ITechnicianEndpoint technicianEndpoint, ISalesEndpoint salesEndpoint,
                                 ISparepartEndpoint sparepartEndpoint)
        {
            DisplayName = "Servisan";

            _windowManager = windowManager;
            _apiHelper = apiHelper;
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
            _salesEndpoint = salesEndpoint;
            _sparepartEndpoint = sparepartEndpoint;

            DateTime today = DateTime.Today;

            // Set start and end date to the start and end of the month, respectively
            _startDate = new DateTime(today.Year, today.Month, 1);
            _endDate = _startDate.AddMonths(1).AddTicks(-1);
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
                if (addServiceVM.NomorNota != -1 &&
                    DXMessageBox.Show("Apakah anda ingin mencetak servisan ini?", "Tambah Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                        ServiceModel newService = await _serviceEndpoint.GetByNomorNota(addServiceVM.NomorNota);
                        PrintService(newService);
                }

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
                await LoadSparepart(nomorNota);
            }
        }

        public async Task EditService()
        {
            ServiceModel service = SelectedService;

            EditServiceViewModel editServiceVM = IoC.Get<EditServiceViewModel>();
            editServiceVM.SetFieldValues(service);

            if (_windowManager.ShowDialog(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task EditServiceLimited()
        {
            ServiceModel service = SelectedService;

            EditServiceLimitedViewModel editServiceVM = IoC.Get<EditServiceLimitedViewModel>();
            editServiceVM.SetFieldValues(service);

            if (_windowManager.ShowDialog(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task DeleteService()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus servisan ini?", "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _serviceEndpoint.Delete(SelectedService.NomorNota);
                await LoadServices();
            }
        }

        public async Task DeleteSparepart()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus sparepart ini?", "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int nomorNota = SelectedSparepart.NomorNota;

                await _sparepartEndpoint.Delete(SelectedSparepart.Id);
                await LoadSparepart(nomorNota);
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
            var serviceCollection = new BindableCollection<ServiceModel>(serviceList);

            if (Services?.SequenceEqual(serviceCollection) ?? false)
            {
                IsLoading = false;
                return;
            }

            Services = serviceCollection;
            IsLoading = false;

            OnRefresh?.Invoke();
        }

        public async Task LoadSparepart(int nomorNota)
        {
            int index = Services.FindIndex(s => s.NomorNota == nomorNota);

            Services[index] = await _serviceEndpoint.GetByNomorNota(nomorNota);
            Services[index].Spareparts = await _sparepartEndpoint.GetByNomorNota(nomorNota);
        }

        public void PrintSelectedService()
        {
            if (SelectedService == null) return;
            PrintService(SelectedService);
        }

        private void PrintService(ServiceModel service)
        {
            string kelengkapan = service.Kelengkapan?.Trim().Replace(" ", ", ");

            if (!string.IsNullOrWhiteSpace(kelengkapan) && kelengkapan.Length > 1)
            {
                kelengkapan = kelengkapan[0].ToString() + kelengkapan.Substring(1).ToLower();

                // Make "SIM" uppercase so it looks better; there's probably a more efficient way of doing this but I don't care.
                for (int i = 0; i < kelengkapan.Length; i++)
                {
                    if (kelengkapan[i].ToString().ToLower() == "s" && kelengkapan.Length - i - 1 >= 2 && kelengkapan[i + 1] == 'i' &&
                        kelengkapan[i + 2] == 'm')
                    {
                        kelengkapan = kelengkapan.Substring(0, i) + "SIM" + kelengkapan.Substring(i + 3);
                        break;
                    }
                }
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

            _windowManager.ShowDialog(invoicePreviewVM);
        }
    }
}
