using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public sealed class MembersViewModel : Screen
    {
        private IMemberEndpoint _memberEndpoint;

        private BindingList<MemberModel> _members;
        private MemberModel _selectedMember;

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
            }
        }

        public bool CanEditMember
        {
            get => SelectedMember != null;
        }

        public bool CanDeleteMember
        {
            get => SelectedMember != null;
        }

        public MembersViewModel(IMemberEndpoint memberEndpoint)
        {
            DisplayName = "Members";
            _memberEndpoint = memberEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadMembers();
        }

        public void AddMember()
        {
            
        }

        public void EditMember()
        {

        }

        public void DeleteMember()
        {

        }

        public async Task LoadMembers()
        {
            List<MemberModel> memberList = await _memberEndpoint.GetAll();
            Members = new BindingList<MemberModel>(memberList);
        }
    }
}
