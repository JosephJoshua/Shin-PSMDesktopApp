﻿using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSMDesktopUI.ViewModels
{
    public sealed class DamagesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IDamageEndpoint _damageEndpoint;

        private bool _isLoading = false;

        private BindableCollection<DamageModel> _damages;
        private DamageModel _selectedDamage;

        private string _searchText;

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

        public BindableCollection<DamageModel> Damages
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

        public string SearchText
        {
            get => _searchText;

            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public bool CanAddDamage
        {
            get => !IsLoading;
        }

        public bool CanDeleteDamage
        {
            get => !IsLoading && SelectedDamage != null;
        }

        public DamagesViewModel(IWindowManager windowManager, IDamageEndpoint damageEndpoint)
        {
            DisplayName = "Damages";

            _windowManager = windowManager;
            _damageEndpoint = damageEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadDamages();
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadDamages();
            }
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
            if (IsLoading) return;

            IsLoading = true;
            List<DamageModel> damageList = await _damageEndpoint.GetAll();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                damageList = damageList.Where(d => d.Kerusakan.ToLower().Contains(SearchText.ToLower())).ToList();
            }

            IsLoading = false;
            Damages = new BindableCollection<DamageModel>(damageList);
        }
    }
}
