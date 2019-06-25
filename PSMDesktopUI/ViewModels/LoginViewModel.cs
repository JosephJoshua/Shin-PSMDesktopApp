﻿using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Helpers;
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

        private readonly IApiHelper _apiHelper;
        private readonly IInternetConnectionHelper _internetConnectionHelper;

        public string Username
        {
            get => _username;

            set
            {
                _username = value;

                NotifyOfPropertyChange(() => Username);
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
                NotifyOfPropertyChange(() => IsErrorMessageVisibile);
            }
        }

        public bool IsErrorMessageVisibile
        {
            get => !string.IsNullOrEmpty(ErrorMessage);
        }

        public bool CanLogin
        {
            get => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        public LoginViewModel(IApiHelper apiHelper, IInternetConnectionHelper internetConnectionHelper)
        {
            _apiHelper = apiHelper;
            _internetConnectionHelper = internetConnectionHelper;
        }

        public async Task Login()
        {
            if (!_internetConnectionHelper.HasInternetConnection)
            {
                ErrorMessage = "No internet connection";
            }

            Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);

            try
            {
                ErrorMessage = string.Empty;

                var result = await _apiHelper.Authenticate(Username, Password);
                await _apiHelper.GetLoggedInUserInfo(result.access_token);

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
