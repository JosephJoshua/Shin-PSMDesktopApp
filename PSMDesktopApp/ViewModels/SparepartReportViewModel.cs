using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace PSMDesktopApp.ViewModels
{
    public sealed class SparepartReportViewModel : Screen
    {
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private bool _isLoading = false;
        private BindableCollection<SparepartModel> _spareparts;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        public BindableCollection<SparepartModel> Spareparts
        {
            get => _spareparts;

            set
            {
                _spareparts = value;

                NotifyOfPropertyChange(() => Spareparts);
                NotifyOfPropertyChange(() => ShowInfo);
                NotifyOfPropertyChange(() => TotalCost);
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
                
                LoadSpareparts();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;

            set
            {
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);

                LoadSpareparts();
            }
        }

        public bool ShowInfo
        {
            get => Spareparts != null && Spareparts.Count > 0;
        }

        public decimal TotalCost
        {
            get => Spareparts?.Sum(t => t.Harga) ?? 0;
        }

        public SparepartReportViewModel(ISparepartEndpoint sparepartEndpoint)
        {
            DisplayName = "Laporan Sparepart";

            _sparepartEndpoint = sparepartEndpoint;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadSpareparts();
        }

        public void ExportToExcel()
        {
            Excel.Application xlApp = new Excel.Application();

            if (xlApp == null)
            {
                DXMessageBox.Show("Microsoft Excel tidak dapat ditemukan", "Laporan Sparepart");
                return;
            }

            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlWorksheet;
            object missingVal = Missing.Value;

            xlWorkbook = xlApp.Workbooks.Add(missingVal);
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Worksheets[1];

            // Headers
            xlWorksheet.Cells[1, 1] = "Id";
            xlWorksheet.Cells[1, 2] = "Nomor Nota";
            xlWorksheet.Cells[1, 3] = "Nama";
            xlWorksheet.Cells[1, 4] = "Harga";
            xlWorksheet.Cells[1, 5] = "Tanggal Pembelian";

            // Rows
            for (int i = 0; i < Spareparts.Count; i++)
            {
                xlWorksheet.Cells[i + 2, 1] = Spareparts[i].Id;
                xlWorksheet.Cells[i + 2, 2] = Spareparts[i].NomorNota;
                xlWorksheet.Cells[i + 2, 3] = Spareparts[i].Nama;
                xlWorksheet.Cells[i + 2, 4] = Spareparts[i].Harga;
                xlWorksheet.Cells[i + 2, 5] = Spareparts[i].TanggalPembelian.ToString();

                ((Excel.Range)xlWorksheet.Cells[i + 2, 4]).NumberFormat = "Rp#,##0.00";
            }

            // Total revenue
            xlWorksheet.Cells[Spareparts.Count + 2, 1] = "Total:";
            xlWorksheet.Cells[Spareparts.Count + 2, 5] = TotalCost.ToString();

            ((Excel.Range)xlWorksheet.Cells[Spareparts.Count + 2, 5]).NumberFormat = "Rp#,##0.00";

            xlWorksheet.Columns.AutoFit();

            for (int i = 1; i <= 5; i++)
            {
                xlWorksheet.Columns[i].ColumnWidth = xlWorksheet.Columns[i].ColumnWidth + 5;
            }

            xlWorksheet.Range["A1"].Style.Font.Size = 15;

            xlApp.Visible = true;

            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);
        }

        public async void LoadSpareparts()
        {
            if (IsLoading) return;

            IsLoading = true;

            // Make sure the start date's time is set to the start of the day
            StartDate = StartDate.Date;

            // Make sure the end date's time is set to the end of the day
            EndDate = EndDate.Date.AddDays(1).AddTicks(-1);

            List<SparepartModel> sparepartList = await _sparepartEndpoint.GetAll(StartDate, EndDate);

            Spareparts = new BindableCollection<SparepartModel>(sparepartList);
            IsLoading = false;
        }
    }
}
