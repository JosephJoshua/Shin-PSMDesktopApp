using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Threading.Tasks;

namespace PSMDesktopApp.ViewModels
{
    public class AddSparepartViewModel : Screen
    {
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
            }
        }

        public AddSparepartViewModel(ISparepartEndpoint sparepartEndpoint)
        {
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

            await _sparepartEndpoint.Insert(sparepart);

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
