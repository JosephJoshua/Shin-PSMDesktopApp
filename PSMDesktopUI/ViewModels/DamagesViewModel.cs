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
    public sealed class DamagesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IInternetConnectionHelper _internetConnectionHelper;
        private readonly IDamageEndpoint _damageEndpoint;

        private bool _isLoading = false;

        private BindingList<DamageModel> _damages;
        private DamageModel _selectedDamage;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddDamage);
                NotifyOfPropertyChange(() => CanDeleteDamage);
            }
        }

        public BindingList<DamageModel> Damages
        {
            get => _damages;

            set
            {
                _damages = value;
                NotifyOfPropertyChange(() => Damages);
            }
        }

        public DamageModel SelectedDamage
        {
            get => _selectedDamage;

            set
            {
                _selectedDamage = value;

                NotifyOfPropertyChange(() => SelectedDamage);
                NotifyOfPropertyChange(() => CanAddDamage);
                NotifyOfPropertyChange(() => CanDeleteDamage);
            }
        }

        public bool CanAddDamage
        {
            get => !IsLoading && _internetConnectionHelper.HasInternetConnection;
        }

        public bool CanDeleteDamage
        {
            get => !IsLoading && SelectedDamage != null && _internetConnectionHelper.HasInternetConnection;
        }

        public DamagesViewModel(IWindowManager windowManager, IInternetConnectionHelper internetConnectionHelper, IDamageEndpoint damageEndpoint)
        {
            DisplayName = "Damages";

            _windowManager = windowManager;
            _internetConnectionHelper = internetConnectionHelper;
            _damageEndpoint = damageEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadDamages();
        }

        public async Task AddDamage()
        {
            if (_windowManager.ShowDialog(IoC.Get<AddDamageViewModel>()) == true)
            {
                await LoadDamages();
            }
        }

        public async Task DeleteDamage()
        {
            if (DXMessageBox.Show("Are you sure you want to delete this damage?", "Damages", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _damageEndpoint.Delete(SelectedDamage.Id);
                await LoadDamages();
            }
        }

        public async Task LoadDamages()
        {
            if (IsLoading || !_internetConnectionHelper.HasInternetConnection) return;

            IsLoading = true;
            List<DamageModel> damageList = await _damageEndpoint.GetAll();

            IsLoading = false;
            Damages = new BindingList<DamageModel>(damageList);
        }
    }
}
