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
    public sealed class SalesViewModel : Screen
    {
        private readonly ILog _logger;

        private readonly IWindowManager _windowManager;
        private readonly ISalesEndpoint _salesEndpoint;

        private bool _isLoading = false;

        private BindableCollection<SalesModel> _sales;
        private SalesModel _selectedSales;

        private string _searchText;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddSales);
                NotifyOfPropertyChange(() => CanDeleteSales);
            }
        }

        public BindableCollection<SalesModel> Sales
        {
            get => _sales;

            set
            {
                _sales = value;
                NotifyOfPropertyChange(() => Sales);
            }
        }

        public SalesModel SelectedSales
        {
            get => _selectedSales;

            set
            {
                _selectedSales = value;

                NotifyOfPropertyChange(() => SelectedSales);
                NotifyOfPropertyChange(() => CanAddSales);
                NotifyOfPropertyChange(() => CanDeleteSales);
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

        public bool CanAddSales => !IsLoading;

        public bool CanDeleteSales => !IsLoading && SelectedSales != null;

        public SalesViewModel(IWindowManager windowManager, ISalesEndpoint salesEndpoint)
        {
            DisplayName = "Sales";

            _logger = LogManager.GetLog(typeof(SalesViewModel));
            _windowManager = windowManager;
            _salesEndpoint = salesEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadSales();
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadSales();
            }
        }

        public async Task AddSales()
        {
            if (await _windowManager.ShowDialogAsync(IoC.Get<AddSalesViewModel>()) == true)
            {
                await LoadSales();
            }
        }

        public async Task DeleteSales()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus sales ini?", "Sales", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _salesEndpoint.Delete(SelectedSales.Id);
                    await LoadSales();
                }
                catch (ApiException ex)
                {
                    // Check the error message sent by the server.
                    if (!string.IsNullOrWhiteSpace(ex.Details) && ex.Details.ToLower().Contains("violates foreign key constraint"))
                    {
                        DXMessageBox.Show("Tidak dapat menghapus sales ini karena masih terdapat servisan dengan sales ini.", "Sales", MessageBoxButton.OK);
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

        public async Task LoadSales()
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                List<SalesModel> salesList = await _salesEndpoint.GetAll((SearchText ?? "").Trim());
                Sales = new BindableCollection<SalesModel>(salesList);
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
