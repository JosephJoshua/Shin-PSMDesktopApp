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
    public sealed class SalesViewModel : Screen
    {
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

        public bool CanAddSales
        {
            get => !IsLoading;
        }

        public bool CanDeleteSales
        {
            get => !IsLoading && SelectedSales != null;
        }

        public SalesViewModel(IWindowManager windowManager, ISalesEndpoint salesEndpoint)
        {
            DisplayName = "Sales";

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
            if (IsLoading) return;

            IsLoading = true;
            List<SalesModel> salesList = await _salesEndpoint.GetAll((SearchText ?? "").Trim());

            IsLoading = false;
            Sales = new BindableCollection<SalesModel>(salesList);
        }
    }
}
