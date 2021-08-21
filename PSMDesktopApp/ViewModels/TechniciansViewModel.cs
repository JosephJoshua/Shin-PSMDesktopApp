using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSMDesktopApp.ViewModels
{
    public sealed class TechniciansViewModel : Screen
    {
        private readonly ILog _logger;

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

        public bool CanAddTechnician => !IsLoading;

        public bool CanDeleteTechnician => !IsLoading && SelectedTechnician != null;

        public TechniciansViewModel(IWindowManager windowManager, ITechnicianEndpoint technicianEndpoint)
        {
            DisplayName = "Teknisi";

            _logger = LogManager.GetLog(typeof(TechniciansViewModel));
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
            if (await _windowManager.ShowDialogAsync(IoC.Get<AddTechnicianViewModel>()) == true)
            {
                await LoadTechnicians();
            }
        }

        public async Task DeleteTechnician()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus teknisi ini?", "Teknisi", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _technicianEndpoint.Delete(SelectedTechnician.Id);
                    await LoadTechnicians();
                }
                catch (ApiException ex)
                {
                    // Check the error message sent by the server.
                    if (!string.IsNullOrWhiteSpace(ex.Details) && ex.Details.ToLower().Contains("violates foreign key constraint"))
                    {
                        DXMessageBox.Show("Tidak dapat menghapus teknisi ini karena masih terdapat servisan dengan teknisi ini.", "Sales", MessageBoxButton.OK);
                        return;
                    }

                    _logger.Error(ex);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }

        public async Task LoadTechnicians()
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll((SearchText ?? "").Trim());
                Technicians = new BindableCollection<TechnicianModel>(technicianList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
