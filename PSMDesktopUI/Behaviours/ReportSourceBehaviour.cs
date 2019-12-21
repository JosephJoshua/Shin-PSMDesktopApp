using SAPBusinessObjects.WPF.Viewer;
using System.Windows;

namespace PSMDesktopUI.Behaviours
{
    public static class ReportSourceBehaviour
    {
        public static readonly DependencyProperty ReportSourceProperty =
            DependencyProperty.RegisterAttached(
                "ReportSource",
                typeof(object),
                typeof(ReportSourceBehaviour),
                new PropertyMetadata(ReportSourceChanged));

        private static void ReportSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CrystalReportsViewer crviewer)
            {
                crviewer.ViewerCore.ReportSource = e.NewValue;
            }
        }

        public static void SetReportSource(DependencyObject target, object value)
        {
            target.SetValue(ReportSourceProperty, value);
        }

        public static object GetReportSource(DependencyObject target)
        {
            return target.GetValue(ReportSourceProperty);
        }
    }
}
