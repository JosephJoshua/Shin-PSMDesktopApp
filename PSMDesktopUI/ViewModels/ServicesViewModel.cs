using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Helpers;
using PSMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace PSMDesktopUI.ViewModels
{
    public sealed class ServicesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IInternetConnectionHelper _internetConnectionHelper;
        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;
        private readonly IDamageEndpoint _damageEndpoint;

        private bool _isLoading = false;

        private BindingList<ServiceModel> _services;
        private ServiceModel _selectedService;

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
                NotifyOfPropertyChange(() => CanEditService);
                NotifyOfPropertyChange(() => CanDeleteService);
                NotifyOfPropertyChange(() => CanPrintService);
            }
        }

        public BindingList<ServiceModel> Services
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
                NotifyOfPropertyChange(() => CanEditService);
                NotifyOfPropertyChange(() => CanDeleteService);
                NotifyOfPropertyChange(() => CanPrintService);
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

        public string SelectedServiceDamage
        {
            get
            {
                if (SelectedService == null) return null;

                return _damageEndpoint.GetAll().Result.Where((t) => t.Id == SelectedService.DamageId).FirstOrDefault().Kerusakan;
            }
        }

        public bool CanAddService
        {
            get => !IsLoading && _internetConnectionHelper.HasInternetConnection;
        }

        public bool CanEditService
        {
            get => !IsLoading && SelectedService != null && _internetConnectionHelper.HasInternetConnection;
        }

        public bool CanDeleteService
        {
            get => !IsLoading && SelectedService != null && _internetConnectionHelper.HasInternetConnection;
        }
        
        public bool CanPrintService
        {
            get => !IsLoading && SelectedService != null;
        }

        public ServicesViewModel(IWindowManager windowManager, IInternetConnectionHelper internetConnectionHelper, IServiceEndpoint serviceEndpoint, ITechnicianEndpoint technicianEndpoint,
            IDamageEndpoint damageEndpoint)
        {
            DisplayName = "Services";

            _windowManager = windowManager;
            _internetConnectionHelper = internetConnectionHelper;
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
            _damageEndpoint = damageEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadServices();
        }

        public async Task AddService()
        {
            AddServiceViewModel addServiceVM = IoC.Get<AddServiceViewModel>();

            if (_windowManager.ShowDialog(addServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task EditService()
        {
            EditServiceViewModel editServiceVM = IoC.Get<EditServiceViewModel>();
            editServiceVM.SetFieldValues(SelectedService);

            if (_windowManager.ShowDialog(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task DeleteService()
        {
            if (DXMessageBox.Show("Are you sure you want to delete this service?", "Services", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _serviceEndpoint.Delete(SelectedService.NomorNota);
                await LoadServices();
            }
        }

        public async Task LoadServices()
        {
            if (IsLoading || !_internetConnectionHelper.HasInternetConnection) return;

            IsLoading = true;
            List<ServiceModel> serviceList = await _serviceEndpoint.GetAll();

            IsLoading = false;
            Services = new BindingList<ServiceModel>(serviceList);
            
            OnRefresh?.Invoke();
        }

        public void PrintService()
        {

        }
    }
}
