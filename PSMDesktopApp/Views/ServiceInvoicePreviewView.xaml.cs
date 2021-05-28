using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Models;
using System.Globalization;

namespace PSMDesktopApp.Views
{
    public partial class ServiceInvoicePreviewView : ThemedWindow
    {
        private ServiceInvoiceModel _invoiceModel;

        public ServiceInvoicePreviewView()
        {
            InitializeComponent();
            ReportViewer.Owner = GetWindow(this);
        }

        public void SetInvoiceModel(ServiceInvoiceModel model, string reportPath, string noHpToko, string alamatToko)
        {
            _invoiceModel = model;
            LoadReport(reportPath, noHpToko, alamatToko);
        }

        private void LoadReport(string reportPath, string noHpToko, string alamatToko)
        {
            CultureInfo culture = new CultureInfo("id-ID");

            ReportDocument report = new ReportDocument();
            report.Load(reportPath);

            // Default to a ' ' (space) so the border around the text don't disappear.
            if (string.IsNullOrEmpty(_invoiceModel.NoHp)) _invoiceModel.NoHp = " ";
            if (string.IsNullOrEmpty(_invoiceModel.Imei)) _invoiceModel.Imei = " ";
            if (string.IsNullOrEmpty(_invoiceModel.Kelengkapan)) _invoiceModel.Kelengkapan = " ";
            if (string.IsNullOrEmpty(_invoiceModel.YangBelumDicek)) _invoiceModel.YangBelumDicek = " ";
            if (string.IsNullOrEmpty(_invoiceModel.KondisiHp)) _invoiceModel.KondisiHp = " ";

            report.SetParameterValue("NomorNota", _invoiceModel.NomorNota);
            report.SetParameterValue("NamaPelanggan", _invoiceModel.NamaPelanggan);
            report.SetParameterValue("NoHp", _invoiceModel.NoHp);
            report.SetParameterValue("TipeHp", _invoiceModel.TipeHp);
            report.SetParameterValue("Imei", _invoiceModel.Imei);
            report.SetParameterValue("Kerusakan", _invoiceModel.Kerusakan);
            report.SetParameterValue("Biaya", _invoiceModel.TotalBiaya.ToString("C", culture));
            report.SetParameterValue("Dp", _invoiceModel.Dp.ToString("C", culture));
            report.SetParameterValue("Sisa", _invoiceModel.Sisa.ToString("C", culture));
            report.SetParameterValue("Kelengkapan", _invoiceModel.Kelengkapan);
            report.SetParameterValue("KondisiHp", _invoiceModel.KondisiHp);
            report.SetParameterValue("YangBelumDicek", _invoiceModel.YangBelumDicek);
            report.SetParameterValue("Tanggal", _invoiceModel.Tanggal);

            report.SetParameterValue("NoHpToko", noHpToko);
            report.SetParameterValue("AlamatToko", alamatToko);

            ReportViewer.ViewerCore.ReportSource = report;
        }
    }
}
