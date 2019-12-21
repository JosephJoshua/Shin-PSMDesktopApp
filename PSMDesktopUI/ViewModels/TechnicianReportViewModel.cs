using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Helpers;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace PSMDesktopUI.ViewModels
{
    public sealed class TechnicianReportViewModel : Screen
    {
        private readonly IInternetConnectionHelper _internetConnectionHelper;
        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly IDamageEndpoint _damageEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _isLoading = false;
        private BindingList<TechnicianResultModel> _technicianResults;

        private BindingList<TechnicianModel> _technicians;
        private TechnicianModel _selectedTechnician;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private int _technicianRate;

        public BindingList<TechnicianResultModel> TechnicianResults
        {
            get => _technicianResults;

            set
            {
                _technicianResults = value;

                NotifyOfPropertyChange(() => TechnicianResults);
                NotifyOfPropertyChange(() => ShowInfo);
                NotifyOfPropertyChange(() => TotalRevenue);
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
        
        public BindingList<TechnicianModel> Technicians
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
                if (value < 0 || value > 100) return;

                _technicianRate = value;

                NotifyOfPropertyChange(() => TechnicianRate);
                NotifyOfPropertyChange(() => TotalRevenue);
                NotifyOfPropertyChange(() => Proceeds);
            }
        }

        public bool ShowInfo
        {
            get => TechnicianResults != null && TechnicianResults.Count > 0;
        }

        public decimal TotalRevenue
        {
            get => TechnicianResults.Sum(t => t.LabaRugi);
        }

        public decimal Proceeds
        {
            get
            {
                decimal proceeds = ((decimal)TechnicianRate / 100) * TotalRevenue;
                return proceeds;
            }
        }

        public TechnicianReportViewModel(IInternetConnectionHelper internetConnectionHelper, IServiceEndpoint serviceEndpoint, IDamageEndpoint damageEndpoint,
                ITechnicianEndpoint technicianEndpoint)
        {
            DisplayName = "Technician Report";

            _serviceEndpoint = serviceEndpoint;
            _damageEndpoint = damageEndpoint;
            _technicianEndpoint = technicianEndpoint;
            _internetConnectionHelper = internetConnectionHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadTechnicians();
            await LoadResults();
        }

        public void ExportToExcel()
        {
            Excel.Application xlApp = new Excel.Application();

            if (xlApp == null)
            {
                DXMessageBox.Show("Microsoft Excel is not properly installed");
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
            xlWorksheet.Cells[TechnicianResults.Count + 2, 1] = "Total:";
            xlWorksheet.Cells[TechnicianResults.Count + 2, 8] = TotalRevenue.ToString();

            ((Excel.Range)xlWorksheet.Cells[TechnicianResults.Count + 2, 8]).NumberFormat = "Rp#,##0.00";

            // Proceeds
            xlWorksheet.Cells[TechnicianResults.Count + 3, 1] = "Proceeds:";
            xlWorksheet.Cells[TechnicianResults.Count + 3, 8] = Proceeds.ToString();

            ((Excel.Range)xlWorksheet.Cells[TechnicianResults.Count + 3, 8]).NumberFormat = "Rp#,##0.00";

            // Rate
            xlWorksheet.Cells[TechnicianResults.Count + 4, 1] = "Rate:";
            xlWorksheet.Cells[TechnicianResults.Count + 4, 8] = TechnicianRate + "%";

            ((Excel.Range)xlWorksheet.Cells[TechnicianResults.Count + 4, 8]).NumberFormat = "Rp#,##0.00";

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
            List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
            Technicians = new BindingList<TechnicianModel>(technicianList);
        }

        public async Task LoadResults()
        {
            if (IsLoading || !_internetConnectionHelper.HasInternetConnection || SelectedTechnician == null) return;

            IsLoading = true;

            List<DamageModel> damageList = await _damageEndpoint.GetAll();

            List<TechnicianResultModel> resultList = (await _serviceEndpoint.GetAll()).Where((s) => s.TechnicianId == SelectedTechnician.Id && 
                (s.StatusServisan.ToLower() == "Jadi (Sudah diambil)".ToLower() || s.StatusServisan.ToLower() == "Tidak Jadi (Sudah diambil)".ToLower()))
                .Select(s => new TechnicianResultModel
                {
                    NomorNota = s.NomorNota,
                    TanggalPengambilan = s.TanggalPengambilan,
                    TipeHp = s.TipeHp,
                    Biaya = s.Biaya,
                    HargaSparepart = s.HargaSparepart,
                    LabaRugi = s.LabaRugi,
                    Kerusakan = damageList.Find(d => d.Id == s.DamageId).Kerusakan,
                    NamaTeknisi = Technicians.SingleOrDefault(t => t.Id == s.TechnicianId).Nama
                }).ToList();

            List<TechnicianResultModel> filteredResultList = new List<TechnicianResultModel>();

            foreach (TechnicianResultModel result in resultList)
            {
                if (result.TanggalPengambilan >= StartDate && result.TanggalPengambilan <= EndDate)
                {
                    filteredResultList.Add(result);
                }
            }

            IsLoading = false;
            TechnicianResults = new BindingList<TechnicianResultModel>(filteredResultList);
        }
    }
}
