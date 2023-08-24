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
    public sealed class SisaReportViewModel: Screen
    {
        private readonly ILog _logger;
        private readonly IConnectionHelper _connectionHelper;
        private readonly IServiceEndpoint _serviceEndpoint;

        private bool _isLoading = false;
        private BindableCollection<SisaResultModel> _sisaResults;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private bool _isFirstLoad = true;

        public BindableCollection<SisaResultModel> SisaResults 
        {
            get => _sisaResults;

            set
            {
                _sisaResults = value;

                NotifyOfPropertyChange(() => SisaResults);
                NotifyOfPropertyChange(() => TotalRevenue);
                NotifyOfPropertyChange(() => TotalDp);
                NotifyOfPropertyChange(() => TotalSisa);
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

        public bool ShowInfo => SisaResults != null && SisaResults.Count > 0;

        public decimal TotalRevenue => SisaResults?.Sum(t => t.Biaya) ?? 0;

        public decimal TotalDp => SisaResults?.Sum(t => t.Dp) ?? 0;

        public decimal TotalSisa => SisaResults?.Sum(t => t.Sisa) ?? 0;

        public SisaReportViewModel(IServiceEndpoint serviceEndpoint, IConnectionHelper connectionHelper)
        {
            DisplayName = "Laporan Sisa";

            _logger = LogManager.GetLog(typeof(SisaReportViewModel));
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
            xlWorksheet.Cells[1, 6] = "DP";
            xlWorksheet.Cells[1, 7] = "Sisa";

            // Rows
            for (int i = 0; i < SisaResults.Count; i++)
            {
                xlWorksheet.Cells[i + 2, 1] = SisaResults[i].NomorNota;
                xlWorksheet.Cells[i + 2, 2] = SisaResults[i].TanggalPengambilan.ToString();
                xlWorksheet.Cells[i + 2, 3] = SisaResults[i].TipeHp;
                xlWorksheet.Cells[i + 2, 4] = SisaResults[i].Kerusakan;
                xlWorksheet.Cells[i + 2, 5] = SisaResults[i].Biaya;
                xlWorksheet.Cells[i + 2, 6] = SisaResults[i].Dp;
                xlWorksheet.Cells[i + 2, 7] = SisaResults[i].Sisa;

                ((Excel.Range)xlWorksheet.Cells[i + 2, 5]).NumberFormat = "Rp#,##0";
                ((Excel.Range)xlWorksheet.Cells[i + 2, 6]).NumberFormat = "Rp#,##0";
            }

            // Total revenue
            xlWorksheet.Cells[SisaResults.Count + 2, 1] = "Total biaya:";
            xlWorksheet.Cells[SisaResults.Count + 2, 7] = TotalRevenue.ToString();

            ((Excel.Range)xlWorksheet.Cells[SisaResults.Count + 2, 7]).NumberFormat = "Rp#,##0";

            // Total DP 
            xlWorksheet.Cells[SisaResults.Count + 3, 1] = "Total DP:";
            xlWorksheet.Cells[SisaResults.Count + 3, 7] = TotalDp.ToString();

            ((Excel.Range)xlWorksheet.Cells[SisaResults.Count + 3, 7]).NumberFormat = "Rp#,##0";

            // Total sisa 
            xlWorksheet.Cells[SisaResults.Count + 4, 1] = "Total sisa";
            xlWorksheet.Cells[SisaResults.Count + 4, 7] = TotalSisa.ToString();

            ((Excel.Range)xlWorksheet.Cells[SisaResults.Count + 4, 7]).NumberFormat = "Rp#,##0";

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
                List<SisaResultModel> resultList = await _serviceEndpoint.GetSisaReport(StartDate.Date, EndDate.Date);

                SisaResults = new BindableCollection<SisaResultModel>(resultList);

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
