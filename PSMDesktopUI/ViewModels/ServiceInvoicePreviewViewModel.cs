using Caliburn.Micro;
using PSMDesktopUI.Library.Helpers;
using PSMDesktopUI.Library.Models;
using PSMDesktopUI.Views;
using System.Diagnostics;
using System.IO;

namespace PSMDesktopUI.ViewModels
{
    public sealed class ServiceInvoicePreviewViewModel : Screen
    {
        private readonly ISettingsHelper _settings;

        private ServiceInvoiceModel _invoiceModel;

        public ServiceInvoicePreviewViewModel(ISettingsHelper settings)
        {
            _settings = settings;
        }

        protected override void OnViewLoaded(object view)
        {
            ServiceInvoicePreviewView v = GetView() as ServiceInvoicePreviewView;

            string basePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string reportPath = basePath + @"\" + _settings.Get("reportPath").Replace("/", "\\").Trim();

            if (_invoiceModel != null)
            {
                v.SetInvoiceModel(_invoiceModel, reportPath);
            }

            base.OnViewLoaded(view);
        }

        public void SetInvoiceModel(ServiceInvoiceModel model)
        {
            _invoiceModel = model;
        }
    }
}
