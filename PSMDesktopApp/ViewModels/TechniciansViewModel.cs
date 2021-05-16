using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSMDesktopApp.ViewModels
{
    public sealed class TechniciansViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _isLoading = false;

        private BindableCollection<TechnicianModel> _technicians;
        private TechnicianModel _selectedTechnician;

        private string _searchText;

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

        public BindableCollection<TechnicianModel> Technicians
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

        public string SearchText
        {
            get => _searchText;

            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
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

        public TechniciansViewModel(IWindowManager windowManager, ITechnicianEndpoint technicianEndpoint)
        {
            DisplayName = "Technicians";

            _windowManager = windowManager;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadTechnicians();
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadTechnicians();
            }
        }

        public async Task AddTechnician()
        {
            if (_windowManager.ShowDialog(IoC.Get<AddTechnicianViewModel>()) == true)
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

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                technicianList = technicianList.Where(t => t.Nama.ToLower().Contains(SearchText.ToLower())).ToList();
            }

            IsLoading = false;
            Technicians = new BindableCollection<TechnicianModel>(technicianList);
        }
    }
}
