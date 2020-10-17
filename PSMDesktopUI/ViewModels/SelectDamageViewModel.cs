using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PSMDesktopUI.ViewModels
{
    public class SelectDamageViewModel : Screen
    {
        private readonly IDamageEndpoint _damageEndpoint;

        private BindingList<DamageModel> _damages;
        private DamageModel _selectedDamage;

        private bool _isLoading = false;
        private string _searchText;

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
                NotifyOfPropertyChange(() => HasSelectedDamage);
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

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public bool HasSelectedDamage
        {
            get => SelectedDamage != null;
        }

        public SelectDamageViewModel(IDamageEndpoint damageEndpoint)
        {
            _damageEndpoint = damageEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadDamages();
        }

        public void Submit()
        {
            Console.WriteLine(SelectedDamage.Kerusakan);
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadDamages();
            }
        }

        public async Task LoadDamages()
        {
            IsLoading = true;

            List<DamageModel> damageList = await _damageEndpoint.GetAll();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                damageList = damageList.Where(d => d.Kerusakan.ToLower().Contains(SearchText.ToLower())).ToList();
            }

            Damages = new BindingList<DamageModel>(damageList);
            IsLoading = false;
        }
    }
}
