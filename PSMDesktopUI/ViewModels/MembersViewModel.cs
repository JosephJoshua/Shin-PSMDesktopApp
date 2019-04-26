using Caliburn.Micro;
using PSMDesktopUI.Library.Models;
using System.ComponentModel;

namespace PSMDesktopUI.ViewModels
{
    public sealed class MembersViewModel : Screen
    {
        private BindingList<Member> _members;
        private Member _selectedMember;

        public BindingList<Member> Members
        {
            get => _members;

            set
            {
                _members = value;
                NotifyOfPropertyChange(() => Members);
            }
        }

        public Member SelectedMember
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

        public MembersViewModel()
        {
            DisplayName = "Members";
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

        public void RefreshGrid()
        {

        }
    }
}
