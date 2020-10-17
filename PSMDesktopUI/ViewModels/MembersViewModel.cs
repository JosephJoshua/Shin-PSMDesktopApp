using Caliburn.Micro;
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
    public sealed class MembersViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IMemberEndpoint _memberEndpoint;

        private bool _isLoading = false;

        private BindableCollection<MemberModel> _members;
        private MemberModel _selectedMember;

        private string _searchText;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddMember);
                NotifyOfPropertyChange(() => CanEditMember);
                NotifyOfPropertyChange(() => CanDeleteMember);
            }
        }

        public BindableCollection<MemberModel> Members
        {
            get => _members;

            set
            {
                _members = value;
                NotifyOfPropertyChange(() => Members);
            }
        }

        public MemberModel SelectedMember
        {
            get => _selectedMember;

            set
            {
                _selectedMember = value;

                NotifyOfPropertyChange(() => SelectedMember);
                NotifyOfPropertyChange(() => CanEditMember);
                NotifyOfPropertyChange(() => CanDeleteMember);
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

        public bool CanAddMember
        {
            get => !IsLoading;
        }

        public bool CanEditMember
        {
            get => !IsLoading && SelectedMember != null;
        }

        public bool CanDeleteMember
        {
            get => !IsLoading && SelectedMember != null;
        }

        public MembersViewModel(IWindowManager windowManager, IMemberEndpoint memberEndpoint)
        {
            DisplayName = "Members";

            _windowManager = windowManager;
            _memberEndpoint = memberEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadMembers();
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadMembers();
            }
        }

        public async Task AddMember()
        {
            AddMemberViewModel addMemberVM = IoC.Get<AddMemberViewModel>();

            if (_windowManager.ShowDialog(addMemberVM) == true)
            {
                await LoadMembers();
            }
        }

        public async Task EditMember()
        {
            EditMemberViewModel editMemberVM = IoC.Get<EditMemberViewModel>();
            editMemberVM.SetFieldValues(SelectedMember);

            if (_windowManager.ShowDialog(editMemberVM) == true)
            {
                await LoadMembers();
            }
        }

        public async Task DeleteMember()
        {
            if (DXMessageBox.Show("Are you sure you want to delete this member?", "Members", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _memberEndpoint.Delete(SelectedMember.Id);
                await LoadMembers();
            }
        }

        public async Task LoadMembers()
        {
            if (IsLoading) return;

            IsLoading = true;

            List<MemberModel> memberList = await _memberEndpoint.GetAll();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                memberList = memberList.Where(m => m.Nama.ToLower().Contains(SearchText.ToLower())).ToList();
            }

            IsLoading = false;
            Members = new BindableCollection<MemberModel>(memberList);
        }
    }
}
