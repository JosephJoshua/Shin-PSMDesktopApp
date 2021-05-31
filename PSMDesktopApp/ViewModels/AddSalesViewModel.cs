using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System;
using System.Threading.Tasks;

namespace PSMDesktopApp.ViewModels
{
    public class AddSalesViewModel : Screen
    {
        private readonly ILog _logger;
        private ISalesEndpoint _salesEndpoint;

        private string _nama;

        public string Nama
        {
            get => _nama;

            set
            {
                _nama = value;

                NotifyOfPropertyChange(() => Nama);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanAdd
        {
            get => !string.IsNullOrWhiteSpace(Nama);
        }

        public AddSalesViewModel(ISalesEndpoint salesEndpoint)
        {
            _logger = LogManager.GetLog(typeof(AddSalesViewModel));
            _salesEndpoint = salesEndpoint;
        }

        public async Task Add()
        {
            SalesModel sales = new SalesModel { Nama = Nama, };

            try
            {
                await _salesEndpoint.Insert(sales);
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
