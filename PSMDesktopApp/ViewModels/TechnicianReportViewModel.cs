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
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace PSMDesktopApp.ViewModels
{
    public sealed class TechnicianReportViewModel : Screen
    {
        private readonly ILog _logger;
        private readonly IConnectionHelper _connectionHelper;

        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _isLoading = false;
        private BindableCollection<TechnicianResultModel> _technicianResults;

        private BindableCollection<TechnicianModel> _technicians;
        private TechnicianModel _selectedTechnician;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private int _technicianRate;

        private bool _isFirstLoad = true;

        public BindableCollection<TechnicianResultModel> TechnicianResults
        {
            get => _technicianResults;

            set
            {
                _technicianResults = value;

                NotifyOfPropertyChange(() => TechnicianResults);
                NotifyOfPropertyChange(() => ShowInfo);
                NotifyOfPropertyChange(() => TotalRevenue);
                NotifyOfPropertyChange(() => TotalCost);
                NotifyOfPropertyChange(() => TotalProfit);
                NotifyOfPropertyChange(() => Proceeds);;
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
        
        public BindableCollection<TechnicianModel> Technicians
        {
            get => _technicians;

            set
            {
                _technicians = value;
                NotifyOfPropertyChange(() => Technicians);
            }
        }

        public TechnicianModel SelectedTechnician
        {
            get => _selectedTechnician;

            set
            {
                _selectedTechnician = value;
                NotifyOfPropertyChange(() => SelectedTechnician);

                LoadResults();
            }
        }

        public int TechnicianRate
        {
            get => _technicianRate;

            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                
                _technicianRate = value;

                NotifyOfPropertyChange(() => TechnicianRate);
                NotifyOfPropertyChange(() => TotalProfit);
                NotifyOfPropertyChange(() => Proceeds);
            }
        }

        public bool ShowInfo => TechnicianResults != null && TechnicianResults.Count > 0;

        public decimal TotalRevenue => TechnicianResults?.Sum(t => t.Biaya) ?? 0;

        public decimal TotalCost => TechnicianResults?.Sum(t => t.HargaSparepart) ?? 0;

        public decimal TotalProfit => TechnicianResults?.Sum(t => t.LabaRugi) ?? 0;

        public decimal Proceeds
        {
            get
            {
                decimal proceeds = (decimal)TechnicianRate / 100 * TotalProfit;
                return proceeds;
            }
        }

        public TechnicianReportViewModel(IServiceEndpoint serviceEndpoint, ITechnicianEndpoint technicianEndpoint, IConnectionHelper connectionHelper)
        {
            DisplayName = "Laporan Teknisi";

            _logger = LogManager.GetLog(typeof(TechnicianReportViewModel));

            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
            _connectionHelper = connectionHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadTechnicians();
            LoadResults();
        }

        public void ExportToExcel()
        {
            Excel.Application xlApp = new Excel.Application();

            if (xlApp == null)
            {
                DXMessageBox.Show("Microsoft Excel tidak dapat ditemukan", "Laporan Teknisi");
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
            xlWorksheet.Cells[1, 8] = "Teknisi";

            // Rows
            for (int i = 0; i < TechnicianResults.Count; i++)
            {
                xlWorksheet.Cells[i + 2, 1] = TechnicianResults[i].NomorNota;
                xlWorksheet.Cells[i + 2, 2] = TechnicianResults[i].TanggalPengambilan.ToString();
                xlWorksheet.Cells[i + 2, 3] = TechnicianResults[i].TipeHp;
                xlWorksheet.Cells[i + 2, 4] = TechnicianResults[i].Kerusakan;
                xlWorksheet.Cells[i + 2, 5] = TechnicianResults[i].Biaya;
                xlWorksheet.Cells[i + 2, 6] = TechnicianResults[i].HargaSparepart;
                xlWorksheet.Cells[i + 2, 7] = TechnicianResults[i].LabaRugi;
                xlWorksheet.Cells[i + 2, 8] = TechnicianResults[i].NamaTeknisi;

                ((Excel.Range)xlWorksheet.Cells[i + 2, 5]).NumberFormat = "Rp#,##0.00";
                ((Excel.Range)xlWorksheet.Cells[i + 2, 6]).NumberFormat = "Rp#,##0.00";
            }

            // Total revenue
            xlWorksheet.Cells[TechnicianResults.Count + 2, 1] = "Total biaya:";
            xlWorksheet.Cells[TechnicianResults.Count + 2, 8] = TotalRevenue.ToString();

            ((Excel.Range)xlWorksheet.Cells[TechnicianResults.Count + 2, 8]).NumberFormat = "Rp#,##0.00";

            // Total cost
            xlWorksheet.Cells[TechnicianResults.Count + 3, 1] = "Total harga sparepart:";
            xlWorksheet.Cells[TechnicianResults.Count + 3, 8] = TotalCost.ToString();

            ((Excel.Range)xlWorksheet.Cells[TechnicianResults.Count + 3, 8]).NumberFormat = "Rp#,##0.00";

            // Total profit
            xlWorksheet.Cells[TechnicianResults.Count + 4, 1] = "Total laba/rugi:";
            xlWorksheet.Cells[TechnicianResults.Count + 4, 8] = TotalProfit.ToString();

            ((Excel.Range)xlWorksheet.Cells[TechnicianResults.Count + 4, 8]).NumberFormat = "Rp#,##0.00";

            // Proceeds
            xlWorksheet.Cells[TechnicianResults.Count + 3, 1] = "Pendapatan teknisi:";
            xlWorksheet.Cells[TechnicianResults.Count + 3, 8] = Proceeds.ToString();

            ((Excel.Range)xlWorksheet.Cells[TechnicianResults.Count + 3, 8]).NumberFormat = "Rp#,##0.00";

            // Rate
            xlWorksheet.Cells[TechnicianResults.Count + 4, 1] = "Persentase pendapatan teknisi:";
            xlWorksheet.Cells[TechnicianResults.Count + 4, 8] = TechnicianRate + "%";

            xlWorksheet.Columns.AutoFit();

            for (int i = 1; i <= 8; i++)
            {
                xlWorksheet.Columns[i].ColumnWidth = xlWorksheet.Columns[i].ColumnWidth + 5;
            }

            xlWorksheet.Range["A1"].Style.Font.Size = 15;

            xlApp.Visible = true;

            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);
        }

        public async Task LoadTechnicians()
        {
            try
            {
                List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
                Technicians = new BindableCollection<TechnicianModel>(technicianList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public async void LoadResults()
        {
            if (IsLoading || SelectedTechnician == null || (!_isFirstLoad && !_connectionHelper.WasConnectionSuccessful)) return;

            IsLoading = true;

            try 
            {
                List<TechnicianResultModel> resultList = await _serviceEndpoint.GetTeknisiReport(SelectedTechnician.Id, StartDate.Date, EndDate.Date);

                TechnicianResults = new BindableCollection<TechnicianResultModel>(resultList);

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
