using Caliburn.Micro;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IWindowManager _windowManager;

        private readonly MembersViewModel _membersViewModel;
        private readonly TechniciansViewModel _techniciansViewModel;
        
        public ShellViewModel(IWindowManager windowManager, MembersViewModel membersViewModel, TechniciansViewModel techniciansViewModel)
        {
            _windowManager = windowManager;

            _membersViewModel = membersViewModel;
            _techniciansViewModel = techniciansViewModel;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await Task.Delay(1000);

            if (_windowManager.ShowDialog(IoC.Get<LoginViewModel>()) == false)
            {
                TryClose();
            }
            else
            {
                Items.Add(_membersViewModel);
                Items.Add(_techniciansViewModel);
            }
        }
    }
}
