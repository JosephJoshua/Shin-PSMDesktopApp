using DevExpress.Xpf.Grid;
using PSMDesktopApp.ViewModels;
using System.Windows.Controls;

namespace PSMDesktopApp.Views
{
    public partial class ServicesView : UserControl
    {
        private bool _isFirstLoad = true;

        private int _serviceFocusedRowHandle;
        private bool _wasFocusedRowExpanded;

        public ServicesView()
        {
            InitializeComponent();
        }

        private void BeforeRefresh()
        {
            _serviceFocusedRowHandle = MasterView.FocusedRowHandle;
            _wasFocusedRowExpanded = ServicesGrid.IsMasterRowExpanded(_serviceFocusedRowHandle);
        }

        private void OnRefresh()
        {
            TableView tableView = ServicesGrid.View as TableView;
            tableView.BestFitColumns();

            foreach (GridColumn column in ServicesGrid.Columns)
            {
                // I know that hardcoding "NomorNota" in here isn't a good idea but it will do for now.
                if (column.FieldName == "NomorNota") continue;

                column.Width = column.Width.Value + 20;
            }

            MasterView.FocusedRowHandle = _serviceFocusedRowHandle;

            if (_wasFocusedRowExpanded && MasterView.GetSelectedRows().Count > 0)
            {
                ServicesGrid.ExpandMasterRow(MasterView.FocusedRowHandle);
            }
        }

        private void SetInitialGridWidth()
        {
            double lcWidth = MainLayoutControl.ActualWidth;
            GridLayoutGroup.Width = lcWidth * 0.7d;
        }

        private void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ServicesViewModel vm = (ServicesViewModel)DataContext;

            vm.BeforeRefresh += BeforeRefresh;
            vm.OnRefresh += OnRefresh;

            if (_isFirstLoad)
            {
                SetInitialGridWidth();
            }

            _isFirstLoad = true;
        }
    }
}
