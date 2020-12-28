using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopUI.ViewModels
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

        public void Recalculate()
        {
            _windowManager.ShowDialog(IoC.Get<RecalculateViewModel>());
        }

        public void OnClose(CancelEventArgs args)
        {
            if (!_loggedIn) return;

            MessageBoxResult result = DXMessageBox.Show("Do you want to recalculate automatic values?", "", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                Recalculate();
            }
            else if (result == MessageBoxResult.Cancel)
            {
                args.Cancel = true;
            }
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
                string role = _apiHelper.LoggedInUser.Role;

                _loggedIn = true;

                if (role == "Customer Service")
                {
                    Items.Add(_servicesViewModel);
                }
                else if (role == "Admin")
                {
                    Items.Add(_techniciansViewModel);
                    Items.Add(_salesViewModel);
                    Items.Add(_servicesViewModel);
                    Items.Add(_sparepartReportViewModel);
                    Items.Add(_profitReportViewModel);
                    Items.Add(_technicianReportViewModel);
                }
                else if (role == "Buyer")
                {
                    Items.Add(_servicesViewModel);
                    Items.Add(_sparepartReportViewModel);
                }
            }
        }
    }
}
