using System.ComponentModel;
using DevExpress.Xpf.Core;
using PSMDesktopApp.ViewModels;

namespace PSMDesktopApp.Views
{
    public partial class ShellView : ThemedWindow
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DataContext != null)
            {
                ShellViewModel vm = (ShellViewModel)DataContext;
                vm.OnClose(e);
            }
        }
    }
}
