using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System.Collections;

namespace PSMDesktopUI.Views
{
    public partial class AddServiceView : ThemedWindow
    {
        public AddServiceView()
        {
            InitializeComponent();
        }

        private void SalesGrid_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            if (e.NewItemsSource == null || ((IList)e.NewItemsSource).Count == 0) return;

            GridControl grid = sender as GridControl;
            grid.View.FocusedRowHandle = 0;
        }

        private void MemberGrid_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            if (e.NewItemsSource == null || ((IList)e.NewItemsSource).Count == 0) return;

            GridControl grid = sender as GridControl;
            grid.View.FocusedRowHandle = 0;
        }
    }
}
