using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class AddTechnicianViewModel : Screen
    {
        private readonly ITechnicianEndpoint _technicianEndpoint;

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

        public AddTechnicianViewModel(ITechnicianEndpoint technicianEndpoint)
        {
            _technicianEndpoint = technicianEndpoint;
        }

        public async Task Add()
        {
            TechnicianModel technician = new TechnicianModel
            {
                Nama = Nama,
            };

            await _technicianEndpoint.Insert(technician);

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
