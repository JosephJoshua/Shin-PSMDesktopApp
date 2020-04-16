using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class RecalculateViewModel : Screen
    {
        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private string _currentAction;

        private List<ServiceModel> _services;

        public string CurrentAction
        {
            get => _currentAction;

            set
            {
                _currentAction = value;
                NotifyOfPropertyChange(() => CurrentAction);
            }
        }

        public RecalculateViewModel(IServiceEndpoint serviceEndpoint, ISparepartEndpoint sparepartEndpoint)
        {
            _serviceEndpoint = serviceEndpoint;
            _sparepartEndpoint = sparepartEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await GetServices();
            await CalculateServiceExpenses();
            await CalculateLabaRugi();

            CurrentAction = "Done";
            await Task.Delay(1500);

            TryClose(true);
        }

        private async Task GetServices()
        {
            CurrentAction = "Getting services..";

            if (_services == null || !_services.Any())
            {
                _services = await _serviceEndpoint.GetAll();
            }
        }

        private async Task CalculateServiceExpenses()
        {
            CurrentAction = "Calculating service expenses..";

            for (int i = 0; i < _services.Count; i++)
            {
                CurrentAction = $"Recalculating service expenses ({i + 1}/{_services.Count})";
                decimal hargaSparepart = 0;

                foreach (SparepartModel s in await _sparepartEndpoint.GetByService(_services[i].NomorNota))
                {
                    hargaSparepart += s.Harga;
                }

                ServiceModel newService = new ServiceModel
                {
                    NomorNota = _services[i].NomorNota,
                    NamaPelanggan = _services[i].NamaPelanggan,
                    NoHp = _services[i].NoHp,
                    TipeHp = _services[i].TipeHp,
                    Imei = _services[i].Imei,
                    DamageId = _services[i].DamageId,
                    KondisiHp = _services[i].KondisiHp,
                    YangBelumDicek = _services[i].YangBelumDicek,
                    Kelengkapan = _services[i].Kelengkapan,
                    Warna = _services[i].Warna,
                    KataSandiPola = _services[i].KataSandiPola,
                    TechnicianId = _services[i].TechnicianId,
                    StatusServisan = _services[i].StatusServisan,
                    TanggalKonfirmasi = _services[i].TanggalKonfirmasi,
                    IsiKonfirmasi = _services[i].IsiKonfirmasi,
                    Biaya = _services[i].Biaya,
                    Discount = _services[i].Discount,
                    Dp = _services[i].Dp,
                    TambahanBiaya = _services[i].TambahanBiaya,
                    HargaSparepart = hargaSparepart,
                    LabaRugi = _services[i].LabaRugi,
                    TanggalPengambilan = _services[i].TanggalPengambilan,
                };

                await _serviceEndpoint.Update(newService);
            }

            CurrentAction = "Done calculating service expenses..";
        }

        private async Task CalculateLabaRugi()
        {
            CurrentAction = "Calculating laba/rugi..";

            for (int i = 0; i < _services.Count; i++)
            {
                CurrentAction = $"Recalculating laba/rugi ({i + 1}/{_services.Count})";
                decimal labaRugi = _services[i].TotalBiaya - _services[i].HargaSparepart;

                ServiceModel newService = new ServiceModel
                {
                    NomorNota = _services[i].NomorNota,
                    NamaPelanggan = _services[i].NamaPelanggan,
                    NoHp = _services[i].NoHp,
                    TipeHp = _services[i].TipeHp,
                    Imei = _services[i].Imei,
                    DamageId = _services[i].DamageId,
                    YangBelumDicek = _services[i].YangBelumDicek,
                    Kelengkapan = _services[i].Kelengkapan,
                    Warna = _services[i].Warna,
                    KataSandiPola = _services[i].KataSandiPola,
                    TechnicianId = _services[i].TechnicianId,
                    StatusServisan = _services[i].StatusServisan,
                    TanggalKonfirmasi = _services[i].TanggalKonfirmasi,
                    IsiKonfirmasi = _services[i].IsiKonfirmasi,
                    Biaya = _services[i].Biaya,
                    Discount = _services[i].Discount,
                    Dp = _services[i].Dp,
                    TambahanBiaya = _services[i].TambahanBiaya,
                    HargaSparepart = _services[i].HargaSparepart,
                    LabaRugi = labaRugi,
                    TanggalPengambilan = _services[i].TanggalPengambilan,
                };

                await _serviceEndpoint.Update(newService);
            }
        }
    }
}
