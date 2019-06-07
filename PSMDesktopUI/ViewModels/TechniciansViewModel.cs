using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopUI.ViewModels
{
    public sealed class TechniciansViewModel : Screen
    {
        private readonly SimpleContainer _container;

        private readonly IWindowManager _windowManager;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _isLoading = false;

        private BindingList<TechnicianModel> _technicians;
        private TechnicianModel _selectedTechnician;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddTechnician);
                NotifyOfPropertyChange(() => CanDeleteTechnician);
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
                NotifyOfPropertyChange(() => CanAddTechnician);
                NotifyOfPropertyChange(() => CanDeleteTechnician);
            }
        }

        public bool CanAddTechnician
        {
            get => !IsLoading;
        }

        public bool CanDeleteTechnician
        {
            get => !IsLoading && SelectedTechnician != null;
        }

        public TechniciansViewModel(SimpleContainer container, IWindowManager windowManager, ITechnicianEndpoint technicianEndpoint)
        {
            DisplayName = "Technicians";

            _container = container;

            _windowManager = windowManager;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadTechnicians();
        }

        public async Task AddTechnician()
        {
            if (_windowManager.ShowDialog(_container.GetInstance<AddTechnicianViewModel>()) == true)
            {
                await LoadTechnicians();
            }
        }

        public async Task DeleteTechnician()
        {
            if (DXMessageBox.Show("Are you sure you want to delete this technician?", "Technicians", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _technicianEndpoint.Delete(SelectedTechnician.Id);
                await LoadTechnicians();
            }
        }

        public async Task LoadTechnicians()
        {
            if (IsLoading) return;

            IsLoading = true;
            List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();

            IsLoading = false;
            Technicians = new BindingList<TechnicianModel>(technicianList);
        }
    }
}
