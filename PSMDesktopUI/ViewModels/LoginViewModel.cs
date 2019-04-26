using Caliburn.Micro;
using PSMDesktopUI.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username;
        private string _password;

        private string _errorMessage;

        private IApiHelper _apiHelper;

        public string Username
        {
            get { return _username; }

            set
            {
                _username = value;

                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get { return _password; }

            set
            {
                _password = value;

                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }

            set
            {
                _errorMessage = value;

                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorMessageVisibile);
            }
        }

        public bool IsErrorMessageVisibile
        {
            get { return !string.IsNullOrEmpty(ErrorMessage); }
        }

        public bool CanLogin
        {
            get { return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password); }
        }

        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task Login()
        {
            Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);

            try
            {
                ErrorMessage = string.Empty;

                var result = await _apiHelper.Authenticate(Username, Password);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
            }
        }
    }
}
