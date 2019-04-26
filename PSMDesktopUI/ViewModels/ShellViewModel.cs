using Caliburn.Micro;

namespace PSMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private IWindowManager _windowManager;

        private LoginViewModel _loginViewModel;
        private MembersViewModel _membersViewModel;
        
        public ShellViewModel(IWindowManager windowManager, LoginViewModel loginViewModel, MembersViewModel membersViewModel)
        {
            _windowManager = windowManager;

            _loginViewModel = loginViewModel;
            _membersViewModel = membersViewModel;

            Items.Add(_membersViewModel);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);

            _windowManager.ShowDialog(_loginViewModel);
        }
    }
}
