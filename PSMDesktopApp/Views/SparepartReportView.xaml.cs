using System.Windows.Controls;

namespace PSMDesktopApp.Views
{
    public partial class SparepartReportView : UserControl
    {
        public SparepartReportView()
        {
            InitializeComponent();
        }

        private void SetInitialGridWidth()
        {
            double lcWidth = MainLayoutControl.ActualWidth;
            GridLayoutGroup.Width = lcWidth * 0.8d;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetInitialGridWidth();
        }
    }
}
