using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSMDesktopApp.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _email;
        private string _password;

        private string _errorMessage;

        private readonly IApiHelper _apiHelper;

        public string Email
        {
            get => _email;

            set
            {
                _email = value;

                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get => _password;

            set
            {
                _password = value;

                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;

            set
            {
                _errorMessage = value;

                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorMessageVisible);
            }
        }

        public bool IsErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        public bool CanLogin => !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);

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

                var result = await _apiHelper.Authenticate(Email, Password);
                await _apiHelper.GetLoggedInUserInfo(result.token);

                Application.Current.Dispatcher.Invoke(() => TryClose(true));
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
