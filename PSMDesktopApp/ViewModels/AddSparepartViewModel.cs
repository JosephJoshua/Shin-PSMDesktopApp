using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Threading.Tasks;

namespace PSMDesktopApp.ViewModels
{
    public class AddSparepartViewModel : Screen
    {
        private readonly ILog _logger;
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private int _nomorNota;
        private string _nama;
        private double _harga;

        public int NomorNota
        {
            get => _nomorNota;

            set
            {
                _nomorNota = value;
                NotifyOfPropertyChange(() => NomorNota);
            }
        }

        public string Nama
        {
            get => _nama;

            set
            {
                _nama = value;
                NotifyOfPropertyChange(() => Nama);
            }
        }

        public double Harga
        {
            get => _harga;

            set
            {
                _harga = value;

                NotifyOfPropertyChange(() => Harga);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanAdd => !string.IsNullOrWhiteSpace(Nama) && Harga > 0;

        public AddSparepartViewModel(ISparepartEndpoint sparepartEndpoint)
        {
            _logger = LogManager.GetLog(typeof(AddSparepartViewModel));
            _sparepartEndpoint = sparepartEndpoint;
        }

        public void SetNomorNota(int nomorNota)
        {
            NomorNota = nomorNota;
        }

        public async Task Add()
        {
            SparepartModel sparepart = new SparepartModel
            {
                NomorNota = NomorNota,
                Nama = Nama,
                Harga = (decimal)Harga,
                TanggalPembelian = DateTime.Today,
            };

            try
            {
                await _sparepartEndpoint.Insert(sparepart);
                TryClose(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
