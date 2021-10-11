using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Helpers;
using PSMDesktopApp.Library.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PSMDesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IApiHelper _apiHelper;
        private readonly IConnectionHelper _connectionHelper;
        private readonly IWindowManager _windowManager;

        private readonly TechniciansViewModel _techniciansViewModel;
        private readonly SalesViewModel _salesViewModel;
        private readonly ServicesViewModel _servicesViewModel;
        private readonly SparepartReportViewModel _sparepartReportViewModel;
        private readonly ProfitReportViewModel _profitReportViewModel;
        private readonly TechnicianReportViewModel _technicianReportViewModel;

        private readonly DispatcherTimer _reconnectTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };

        private bool _loggedIn = false;
        private bool _wasConnectionSuccessful = true;

        public bool WasConnectionSuccessful
        {
            get => _wasConnectionSuccessful;

            set
            {
                _wasConnectionSuccessful = value;
                NotifyOfPropertyChange(() => WasConnectionSuccessful);
            }
        }

        public ShellViewModel(IApiHelper apiHelper, IConnectionHelper connectionHelper, IWindowManager windowManager,
            TechniciansViewModel techniciansViewModel, SalesViewModel salesViewModel,
            ServicesViewModel servicesViewModel, SparepartReportViewModel sparepartReportViewModel,
            ProfitReportViewModel profitReportViewModel, TechnicianReportViewModel technicianReportViewModel)
        {
            _apiHelper = apiHelper;
            _connectionHelper = connectionHelper;
            _windowManager = windowManager;

            _techniciansViewModel = techniciansViewModel;
            _salesViewModel = salesViewModel;
            _servicesViewModel = servicesViewModel;
            _sparepartReportViewModel = sparepartReportViewModel;
            _profitReportViewModel = profitReportViewModel;
            _technicianReportViewModel = technicianReportViewModel;

            _reconnectTimer.Tick += TryReconnect;
            _connectionHelper.OnConnectionFailed = () =>
            {
                WasConnectionSuccessful = false;
                _reconnectTimer.Start();
            };
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken)
        {
            if (!_loggedIn) return Task.FromResult(true);

            bool canClose = DXMessageBox.Show("Apakah anda yakin ingin keluar?", "Servisan Manager", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
            if (!_loggedIn || canClose)
            {
                _reconnectTimer.Stop();
            }

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

        private void TryReconnect(object sender, EventArgs e)
        {
            if (_connectionHelper.CanConnectToApi())
            {
                WasConnectionSuccessful = true;
                _reconnectTimer.Stop();
            }
        }
    }
}
