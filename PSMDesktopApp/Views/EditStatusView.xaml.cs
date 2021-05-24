using DevExpress.Xpf.Core;

namespace PSMDesktopApp.Views
{
    public partial class EditStatusView : ThemedWindow
    {
        public EditStatusView()
        {
            InitializeComponent();
            StatusComboBox.Focus();
        }
    }
}
