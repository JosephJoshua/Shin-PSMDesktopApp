using PSMDesktopApp.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) => LogUnhandledException(s, e.ExceptionObject as Exception);

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(s, e.Exception);
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(s, e.Exception);
                e.SetObserved();
            };
        }

        private void LogUnhandledException(object sender, Exception exception)
        {
            var exceptionLogger = new NLogLogger(sender.GetType());
            exceptionLogger.Error(exception);
        }
    }
}
