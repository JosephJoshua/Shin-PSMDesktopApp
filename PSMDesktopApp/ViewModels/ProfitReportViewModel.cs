using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Helpers;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace PSMDesktopApp.ViewModels
{
    public sealed class ProfitReportViewModel : Screen
    {
        private readonly ILog _logger;
        private readonly IConnectionHelper _connectionHelper;
        private readonly IServiceEndpoint _serviceEndpoint;

        private bool _isLoading = false;
        private BindableCollection<ProfitResultModel> _profitResults;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private bool _isFirstLoad = true;

        public BindableCollection<ProfitResultModel> ProfitResults
        {
            get => _profitResults;

            set
            {
                _profitResults = value;

                NotifyOfPropertyChange(() => ProfitResults);
                NotifyOfPropertyChange(() => TotalRevenue);
                NotifyOfPropertyChange(() => TotalCost);
                NotifyOfPropertyChange(() => TotalProfit);
                NotifyOfPropertyChange(() => ShowInfo);
            }
        }

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public DateTime StartDate
        {
            get => _startDate;

            set
            {
                _startDate = value;
                NotifyOfPropertyChange(() => StartDate);

                LoadResults();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;

            set
            {
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);

                LoadResults();
            }
        }

        public bool ShowInfo => ProfitResults != null && ProfitResults.Count > 0;

        public decimal TotalRevenue => ProfitResults?.Sum(t => t.Biaya) ?? 0;

        public decimal TotalCost => ProfitResults?.Sum(t => t.HargaSparepart) ?? 0;

        public decimal TotalProfit => ProfitResults?.Sum(t => t.LabaRugi) ?? 0;

        public ProfitReportViewModel(IServiceEndpoint serviceEndpoint, IConnectionHelper connectionHelper)
        {
            DisplayName = "Laporan Laba/Rugi";

            _logger = LogManager.GetLog(typeof(ProfitReportViewModel));
            _serviceEndpoint = serviceEndpoint;
            _connectionHelper = connectionHelper;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadResults();
        }

        public void ExportToExcel()
        {
            Excel.Application xlApp = new Excel.Application();

            if (xlApp == null)
            {
                DXMessageBox.Show("Microsoft Excel tidak dapat ditemukan", "Laporan Laba/Rugi");
                return;
            }

            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlWorksheet;
            object missingVal = Missing.Value;

            xlWorkbook = xlApp.Workbooks.Add(missingVal);
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Worksheets[1];

            // Headers
            xlWorksheet.Cells[1, 1] = "Nomor Nota";
            xlWorksheet.Cells[1, 2] = "Tanggal Pengambilan";
            xlWorksheet.Cells[1, 3] = "Tipe Hp";
            xlWorksheet.Cells[1, 4] = "Kerusakan";
            xlWorksheet.Cells[1, 5] = "Biaya";
            xlWorksheet.Cells[1, 6] = "Harga Sparepart";
            xlWorksheet.Cells[1, 7] = "Laba/Rugi";

            // Rows
            for (int i = 0; i < ProfitResults.Count; i++)
            {
                xlWorksheet.Cells[i + 2, 1] = ProfitResults[i].NomorNota;
                xlWorksheet.Cells[i + 2, 2] = ProfitResults[i].TanggalPengambilan.ToString();
                xlWorksheet.Cells[i + 2, 3] = ProfitResults[i].TipeHp;
                xlWorksheet.Cells[i + 2, 4] = ProfitResults[i].Kerusakan;
                xlWorksheet.Cells[i + 2, 5] = ProfitResults[i].Biaya;
                xlWorksheet.Cells[i + 2, 6] = ProfitResults[i].HargaSparepart;
                xlWorksheet.Cells[i + 2, 7] = ProfitResults[i].LabaRugi;

                ((Excel.Range)xlWorksheet.Cells[i + 2, 5]).NumberFormat = "Rp#,##0";
                ((Excel.Range)xlWorksheet.Cells[i + 2, 6]).NumberFormat = "Rp#,##0";
            }

            // Total revenue
            xlWorksheet.Cells[ProfitResults.Count + 2, 1] = "Total biaya:";
            xlWorksheet.Cells[ProfitResults.Count + 2, 7] = TotalRevenue.ToString();

            ((Excel.Range)xlWorksheet.Cells[ProfitResults.Count + 2, 7]).NumberFormat = "Rp#,##0";

            // Total cost
            xlWorksheet.Cells[ProfitResults.Count + 3, 1] = "Total harga sparepart:";
            xlWorksheet.Cells[ProfitResults.Count + 3, 7] = TotalCost.ToString();

            ((Excel.Range)xlWorksheet.Cells[ProfitResults.Count + 3, 7]).NumberFormat = "Rp#,##0";

            // Total profit
            xlWorksheet.Cells[ProfitResults.Count + 4, 1] = "Total laba/rugi:";
            xlWorksheet.Cells[ProfitResults.Count + 4, 7] = TotalProfit.ToString();

            ((Excel.Range)xlWorksheet.Cells[ProfitResults.Count + 4, 7]).NumberFormat = "Rp#,##0";

            xlWorksheet.Columns.AutoFit();

            for (int i = 1; i <= 7; i++)
            {
                xlWorksheet.Columns[i].ColumnWidth = xlWorksheet.Columns[i].ColumnWidth + 5;
            }

            xlWorksheet.Range["A1"].Style.Font.Size = 15;

            xlApp.Visible = true;

            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);
        }

        public async void LoadResults()
        {
            if (IsLoading || (!_isFirstLoad && !_connectionHelper.WasConnectionSuccessful)) return;

            IsLoading = true;

            try
            {
                List<ProfitResultModel> resultList = await _serviceEndpoint.GetLabaRugiReport(StartDate.Date, EndDate.Date);

                ProfitResults = new BindableCollection<ProfitResultModel>(resultList);

                _isFirstLoad = false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
