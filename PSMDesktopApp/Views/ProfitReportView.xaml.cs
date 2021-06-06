using System.Windows.Controls;

namespace PSMDesktopApp.Views
{
    public partial class ProfitReportView : UserControl
    {
        public ProfitReportView()
        {
            InitializeComponent();
        }

        private void SetInitialGridWidth()
        {
            double lcWidth = MainLayoutControl.ActualWidth;
            GridLayoutGroup.Width = lcWidth * 0.7d;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetInitialGridWidth();
        }
    }
}
