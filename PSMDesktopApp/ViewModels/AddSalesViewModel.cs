using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Models;
using System.Threading.Tasks;

namespace PSMDesktopApp.ViewModels
{
    public class AddSalesViewModel : Screen
    {
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
            _salesEndpoint = salesEndpoint;
        }

        public async Task Add()
        {
            SalesModel sales = new SalesModel
            {
                Nama = Nama,
            };

            await _salesEndpoint.Insert(sales);

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
