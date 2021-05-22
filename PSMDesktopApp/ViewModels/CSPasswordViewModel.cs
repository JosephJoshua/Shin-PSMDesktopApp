using Caliburn.Micro;
using PSMDesktopApp.Library.Helpers;
using System.Configuration;

namespace PSMDesktopApp.ViewModels
{
    public class CSPasswordViewModel : Screen
    {
        private readonly IStringEncryptionHelper _encryptionHelper;

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;

                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanSubmit);
            }
        }

        public bool CanSubmit
        {
            get => !string.IsNullOrEmpty(Password);
        }

        public CSPasswordViewModel(IStringEncryptionHelper encryptionHelper)
        {
            _encryptionHelper = encryptionHelper;
        }

        public void Submit()
        {
            string hashedPassword = ConfigurationManager.AppSettings["cspassword"];
            bool isCorrect = _encryptionHelper.VerifyHashedPassword(hashedPassword, Password);

            TryClose(isCorrect);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
