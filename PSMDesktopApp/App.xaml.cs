using System.Windows;
using System.Windows.Threading;

namespace PSMDesktopApp
{
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            #if DEBUG

            e.Handled = false;

            #else

            e.Handled = true;

            string errorMessage = "";

            if (e.Exception.InnerException == null)
            {
                errorMessage = string.Format("An application error has occured: {0}", e.Exception.ToString());
            }
            else
            {
                errorMessage = string.Format("An application error has occured: {0} Inner exception: {1}", e.Exception.ToString(), e.Exception.InnerException.ToString());
            }

            MessageBox.Show(errorMessage, "Application error", MessageBoxButton.OK, MessageBoxImage.Error);

            #endif
        }
    }
}
