using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IApiHelper _apiHelper;
        private readonly IWindowManager _windowManager;

        private readonly TechniciansViewModel _techniciansViewModel;
        private readonly SalesViewModel _salesViewModel;
        private readonly ServicesViewModel _servicesViewModel;
        private readonly SparepartReportViewModel _sparepartReportViewModel;
        private readonly ProfitReportViewModel _profitReportViewModel;
        private readonly TechnicianReportViewModel _technicianReportViewModel;

        private bool _loggedIn = false;
        
        public ShellViewModel(IApiHelper apiHelper, IWindowManager windowManager,
            TechniciansViewModel techniciansViewModel, SalesViewModel salesViewModel,
            ServicesViewModel servicesViewModel, SparepartReportViewModel sparepartReportViewModel, 
            ProfitReportViewModel profitReportViewModel, TechnicianReportViewModel technicianReportViewModel)
        {
            _apiHelper = apiHelper;
            _windowManager = windowManager;

            _techniciansViewModel = techniciansViewModel;
            _salesViewModel = salesViewModel;
            _servicesViewModel = servicesViewModel;
            _sparepartReportViewModel = sparepartReportViewModel;
            _profitReportViewModel = profitReportViewModel;
            _technicianReportViewModel = technicianReportViewModel;
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken)
        {
            if (!_loggedIn) return Task.FromResult(true);

            bool canClose = DXMessageBox.Show("Apakah anda yakin ingin keluar?", "Servisan Manager", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
            return Task.FromResult(canClose);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await Task.Delay(1000);

            if (await _windowManager.ShowDialogAsync(IoC.Get<LoginViewModel>()) == false)
            {
                await TryCloseAsync();
            }
            else
            {
                UserRole role = _apiHelper.LoggedInUser.role;

                _loggedIn = true;

                if (role == UserRole.Admin)
                {
                    Items.Add(_techniciansViewModel);
                    Items.Add(_salesViewModel);
                    Items.Add(_servicesViewModel);
                    Items.Add(_sparepartReportViewModel);
                    Items.Add(_profitReportViewModel);
                    Items.Add(_technicianReportViewModel);
                }
                else if (role == UserRole.CustomerService)
                {
                    Items.Add(_servicesViewModel);
                }
            }
        }
    }
}
