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
        private const int ReconnectInterval = 30;

        private readonly IApiHelper _apiHelper;
        private readonly IConnectionHelper _connectionHelper;
        private readonly IWindowManager _windowManager;

        private readonly TechniciansViewModel _techniciansViewModel;
        private readonly SalesViewModel _salesViewModel;
        private readonly ServicesViewModel _servicesViewModel;
        private readonly SparepartReportViewModel _sparepartReportViewModel;
        private readonly ProfitReportViewModel _profitReportViewModel;
        private readonly TechnicianReportViewModel _technicianReportViewModel;

        private readonly DispatcherTimer _reconnectCountdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        private int _secondsBeforeReconnect = ReconnectInterval;
        private bool _loggedIn = false;

        public bool WasConnectionSuccessful => _connectionHelper.WasConnectionSuccessful;

        public int SecondsBeforeReconnect
        {
            get => _secondsBeforeReconnect;

            set
            {
                _secondsBeforeReconnect = value;
                NotifyOfPropertyChange(() => SecondsBeforeReconnect);
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

            _reconnectCountdownTimer.Tick += ReconnectTimerCountdown;
            _connectionHelper.OnConnectionFailed = () =>
            {
                _reconnectCountdownTimer.Start();
                NotifyOfPropertyChange(() => WasConnectionSuccessful);
            };
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken)
        {
            if (!_loggedIn) return Task.FromResult(true);

            bool canClose = DXMessageBox.Show("Apakah anda yakin ingin keluar?", "Servisan Manager", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
            if (!_loggedIn || canClose)
            {
                _reconnectCountdownTimer.Stop();
            }

            return Task.FromResult(canClose);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            // We can't show a dialog immediately after the window's loaded for some reason.
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
                else if (role == UserRole.Buyer)
                {
                    Items.Add(_servicesViewModel);
                    Items.Add(_sparepartReportViewModel);
                }
            }
        }

        public bool TryReconnect()
        {
            if (_connectionHelper.CanConnectToApi())
            {
                // We want to reset the SecondsBeforeReconnect variable after trying to reconnect.
                SecondsBeforeReconnect = ReconnectInterval;

                _reconnectCountdownTimer.Stop();
                NotifyOfPropertyChange(() => WasConnectionSuccessful);

                return true;
            }

            SecondsBeforeReconnect = ReconnectInterval;
            return false;
        }

        private void ReconnectTimerCountdown(object sender, EventArgs e)
        {
            --SecondsBeforeReconnect;

            // We don't want to restart the timer again if we successfully reconnected to the API.
            if (!(SecondsBeforeReconnect <= 0 && TryReconnect()))
            {
                _reconnectCountdownTimer.Start();
            }
        }
    }
}
