using Caliburn.Micro;

namespace PSMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private IWindowManager _windowManager;

        private LoginViewModel _loginViewModel;
        
        public ShellViewModel(IWindowManager windowManager, LoginViewModel loginViewModel)
        {
            _windowManager = windowManager;
            _loginViewModel = loginViewModel;
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);

            _windowManager.ShowDialog(_loginViewModel);
        }
    }
}
