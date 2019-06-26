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
    public sealed class MembersViewModel : Screen
    {
        private readonly SimpleContainer _container;

        private readonly IWindowManager _windowManager;
        private readonly IInternetConnectionHelper _internetConnectionHelper;
        private readonly IMemberEndpoint _memberEndpoint;

        private bool _isLoading = false;

        private BindingList<MemberModel> _members;
        private MemberModel _selectedMember;

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

        public BindingList<MemberModel> Members
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

        public bool CanAddMember
        {
            get => !IsLoading && _internetConnectionHelper.HasInternetConnection;
        }

        public bool CanEditMember
        {
            get => !IsLoading && SelectedMember != null && _internetConnectionHelper.HasInternetConnection;
        }

        public bool CanDeleteMember
        {
            get => !IsLoading && SelectedMember != null && _internetConnectionHelper.HasInternetConnection;
        }

        public MembersViewModel(IWindowManager windowManager, IInternetConnectionHelper internetConnectionHelper, IMemberEndpoint memberEndpoint)
        {
            DisplayName = "Members";

            _windowManager = windowManager;
            _internetConnectionHelper = internetConnectionHelper;
            _memberEndpoint = memberEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadMembers();
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
            if (IsLoading || !_internetConnectionHelper.HasInternetConnection) return;

            IsLoading = true;
            List<MemberModel> memberList = await _memberEndpoint.GetAll();

            IsLoading = false;
            Members = new BindingList<MemberModel>(memberList);
        }
    }
}
