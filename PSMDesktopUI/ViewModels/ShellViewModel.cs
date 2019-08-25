using Caliburn.Micro;
using PSMDesktopUI.Library.Helpers;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IWindowManager _windowManager;
        private readonly IInternetConnectionHelper _internetConnectionHelper;

        private readonly MembersViewModel _membersViewModel;
        private readonly TechniciansViewModel _techniciansViewModel;
        private readonly ServicesViewModel _servicesViewModel;
        
        public ShellViewModel(IWindowManager windowManager, IInternetConnectionHelper internetConnectionHelper,
            MembersViewModel membersViewModel, TechniciansViewModel techniciansViewModel, ServicesViewModel servicesViewModel)
        {
            _windowManager = windowManager;
            _internetConnectionHelper = internetConnectionHelper;

            _membersViewModel = membersViewModel;
            _techniciansViewModel = techniciansViewModel;
            _servicesViewModel = servicesViewModel;

            _internetConnectionHelper.Init();
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
                Items.Add(_servicesViewModel);
            }
        }
    }
}
