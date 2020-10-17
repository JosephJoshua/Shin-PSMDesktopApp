using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System.Collections;

namespace PSMDesktopUI.Views
{
    public partial class SelectDamageView : ThemedWindow
    {
        public SelectDamageView()
        {
            InitializeComponent();
        }

        private void GridControl_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            if (e.NewItemsSource == null || ((IList)e.NewItemsSource).Count == 0) return;

            GridControl grid = sender as GridControl;
            grid.View.FocusedRowHandle = 0;
        }
    }
}
