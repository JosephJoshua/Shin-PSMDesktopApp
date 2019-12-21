using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Helpers;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IApiHelper _apiHelper;
        private readonly IWindowManager _windowManager;
        private readonly IInternetConnectionHelper _internetConnectionHelper;

        private readonly MembersViewModel _membersViewModel;
        private readonly TechniciansViewModel _techniciansViewModel;
        private readonly SalesViewModel _salesViewModel;
        private readonly DamagesViewModel _damagesViewModel;
        private readonly ServicesViewModel _servicesViewModel;
        private readonly SparepartReportViewModel _sparepartReportViewModel;
        private readonly ProfitReportViewModel _profitReportViewModel;
        private readonly TechnicianReportViewModel _technicianReportViewModel;

        private bool _loggedIn = false;

        public string InternetConnectionMessage
        {
            get => _internetConnectionHelper.HasInternetConnection ? "Connected to the internet" : "No internet connection";
        }
        
        public ShellViewModel(IApiHelper apiHelper, IWindowManager windowManager, IInternetConnectionHelper internetConnectionHelper,
            MembersViewModel membersViewModel, TechniciansViewModel techniciansViewModel, SalesViewModel salesViewModel,
            DamagesViewModel damagesViewModel, ServicesViewModel servicesViewModel,
            SparepartReportViewModel sparepartReportViewModel, ProfitReportViewModel profitReportViewModel,
            TechnicianReportViewModel technicianReportViewModel)
        {
            _apiHelper = apiHelper;
            _windowManager = windowManager;
            _internetConnectionHelper = internetConnectionHelper;

            _membersViewModel = membersViewModel;
            _techniciansViewModel = techniciansViewModel;
            _salesViewModel = salesViewModel;
            _damagesViewModel = damagesViewModel;
            _servicesViewModel = servicesViewModel;
            _sparepartReportViewModel = sparepartReportViewModel;
            _profitReportViewModel = profitReportViewModel;
            _technicianReportViewModel = technicianReportViewModel;

            _internetConnectionHelper.Init();
            _internetConnectionHelper.InternetConnectionAvailabilityChanged += InternetConnectionAvailabilityChanged;
        }

        public void Recalculate()
        {
            _windowManager.ShowDialog(IoC.Get<RecalculateViewModel>());
        }

        public void OnClose()
        {
            if (!_loggedIn) return;

            if (DXMessageBox.Show("Do you want to recalculate automatic values?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Recalculate();
            }
        }

        private void InternetConnectionAvailabilityChanged(object sender, System.EventArgs e)
        {
            NotifyOfPropertyChange(() => InternetConnectionMessage);
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
                _loggedIn = true;

                if (_apiHelper.LoggedInUser.Role.ToLower() == "Customer Service".ToLower())
                {
                    Items.Add(_servicesViewModel);
                }
                else if (_apiHelper.LoggedInUser.Role.ToLower() == "Admin".ToLower())
                {
                    Items.Add(_membersViewModel);
                    Items.Add(_techniciansViewModel);
                    Items.Add(_salesViewModel);
                    Items.Add(_damagesViewModel);
                    Items.Add(_servicesViewModel);
                    Items.Add(_sparepartReportViewModel);
                    Items.Add(_profitReportViewModel);
                    Items.Add(_technicianReportViewModel);
                }
            }
        }
    }
}
