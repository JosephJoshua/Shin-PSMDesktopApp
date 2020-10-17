using Caliburn.Micro;

namespace PSMDesktopUI.ViewModels
{
    public class CSPasswordViewModel : Screen
    {
        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public void Submit()
        {
            TryClose(Password == "sorong2020");
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
