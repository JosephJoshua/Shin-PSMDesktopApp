using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class AddDamageViewModel : Screen
    {
        private readonly IDamageEndpoint _damageEndpoint;

        private string _kerusakan;

        public string Kerusakan
        {
            get => _kerusakan;

            set
            {
                _kerusakan = value;

                NotifyOfPropertyChange(() => Kerusakan);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanAdd
        {
            get => !string.IsNullOrWhiteSpace(Kerusakan);
        }

        public AddDamageViewModel(IDamageEndpoint damageEndpoint)
        {
            _damageEndpoint = damageEndpoint;
        }

        public async Task Add()
        {
            DamageModel damage = new DamageModel
            {
                Kerusakan = Kerusakan,
            };

            await _damageEndpoint.Insert(damage);

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
