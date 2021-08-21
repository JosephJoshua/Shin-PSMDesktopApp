using Caliburn.Micro;
using PSMDesktopApp.Library.Helpers;
using PSMDesktopApp.Library.Models;
using PSMDesktopApp.Views;
using System.Diagnostics;
using System.IO;

namespace PSMDesktopApp.ViewModels
{
    public sealed class ServiceInvoicePreviewViewModel : Screen
    {
        private readonly ISettingsHelper _settingsHelper;

        private ServiceInvoiceModel _invoiceModel;

        public ServiceInvoicePreviewViewModel(ISettingsHelper settings)
        {
            _settingsHelper = settings;
        }

        protected override void OnViewLoaded(object view)
        {
            ServiceInvoicePreviewView v = GetView() as ServiceInvoicePreviewView;

            string basePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string reportPath = basePath + @"\" + _settingsHelper.Settings.ReportPath.Replace("/", "\\").Trim();

            if (_invoiceModel != null)
            {
                v.SetInvoiceModel(_invoiceModel, reportPath, _settingsHelper.Settings.NoHpToko, _settingsHelper.Settings.AlamatToko);
            }

            base.OnViewLoaded(view);
        }

        public void SetInvoiceModel(ServiceInvoiceModel model)
        {
            _invoiceModel = model;
        }
    }
}
