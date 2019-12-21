using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Helpers;
using PSMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopUI.ViewModels
{
    public sealed class SalesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IInternetConnectionHelper _internetConnectionHelper;
        private readonly ISalesEndpoint _salesEndpoint;

        private bool _isLoading = false;

        private BindingList<SalesModel> _sales;
        private SalesModel _selectedSales;

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

        public BindingList<SalesModel> Sales
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

        public bool CanAddSales
        {
            get => !IsLoading && _internetConnectionHelper.HasInternetConnection;
        }

        public bool CanDeleteSales
        {
            get => !IsLoading && SelectedSales != null && _internetConnectionHelper.HasInternetConnection;
        }

        public SalesViewModel(IWindowManager windowManager, IInternetConnectionHelper internetConnectionHelper, ISalesEndpoint salesEndpoint)
        {
            DisplayName = "Sales";

            _windowManager = windowManager;
            _internetConnectionHelper = internetConnectionHelper;
            _salesEndpoint = salesEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadSales();
        }

        public async Task AddSales()
        {
            if (_windowManager.ShowDialog(IoC.Get<AddSalesViewModel>()) == true)
            {
                await LoadSales();
            }
        }

        public async Task DeleteSales()
        {
            if (DXMessageBox.Show("Are you sure you want to delete this sales?", "Sales", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _salesEndpoint.Delete(SelectedSales.Id);
                await LoadSales();
            }
        }

        public async Task LoadSales()
        {
            if (IsLoading || !_internetConnectionHelper.HasInternetConnection) return;

            IsLoading = true;
            List<SalesModel> salesList = await _salesEndpoint.GetAll();

            IsLoading = false;
            Sales = new BindingList<SalesModel>(salesList);
        }
    }
}
