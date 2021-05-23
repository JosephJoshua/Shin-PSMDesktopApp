using DevExpress.Xpf.Core;

namespace PSMDesktopApp.Views
{
    public partial class LoginView : ThemedWindow
    {
        public LoginView()
        {
            InitializeComponent();
            Email.Focus();
        }
    }
}
