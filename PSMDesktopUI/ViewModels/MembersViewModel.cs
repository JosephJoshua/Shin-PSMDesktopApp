using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
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

        public MembersViewModel(SimpleContainer container, IWindowManager windowManager, IMemberEndpoint memberEndpoint)
        {
            DisplayName = "Members";

            _container = container;
            _windowManager = windowManager;
            _memberEndpoint = memberEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadMembers();
        }

        public async Task AddMember()
        {
            if (_windowManager.ShowDialog(_container.GetInstance<AddMemberViewModel>()) == true)
            {
                await LoadMembers();
            }
        }

        public void EditMember()
        {

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

            NotifyOfPropertyChange(() => CanAddMember);
            NotifyOfPropertyChange(() => CanEditMember);
            NotifyOfPropertyChange(() => CanDeleteMember);

            List<MemberModel> memberList = await _memberEndpoint.GetAll();

            IsLoading = false;

            NotifyOfPropertyChange(() => CanAddMember);
            NotifyOfPropertyChange(() => CanEditMember);
            NotifyOfPropertyChange(() => CanDeleteMember);

            Members = new BindingList<MemberModel>(memberList);
        }
    }
}
