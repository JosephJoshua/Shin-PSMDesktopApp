using Caliburn.Micro;
using PSMDesktopUI.Library;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class AddMemberViewModel : Screen
    {
        private readonly IMemberEndpoint _memberEndpoint;

        private string _nama;
        private string _noHp;
        private string _alamat;

        private string _tipeHp1;
        private string _tipeHp2;
        private string _tipeHp3;
        private string _tipeHp4;
        private string _tipeHp5;

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

        public string NoHp
        {
            get => _noHp;

            set
            {
                _noHp = value;
                NotifyOfPropertyChange(() => NoHp);
            }
        }

        public string Alamat
        {
            get => _alamat;

            set
            {
                _alamat = value;
                NotifyOfPropertyChange(() => Alamat);
            }
        }

        public string TipeHp1
        {
            get => _tipeHp1;

            set
            {
                _tipeHp1 = value;
                NotifyOfPropertyChange(() => TipeHp1);
            }
        }

        public string TipeHp2
        {
            get => _tipeHp2;

            set
            {
                _tipeHp2 = value;
                NotifyOfPropertyChange(() => TipeHp2);
            }
        }

        public string TipeHp3
        {
            get => _tipeHp3;

            set
            {
                _tipeHp3 = value;
                NotifyOfPropertyChange(() => TipeHp3);
            }
        }

        public string TipeHp4
        {
            get => _tipeHp4;

            set
            {
                _tipeHp4 = value;
                NotifyOfPropertyChange(() => TipeHp4);
            }
        }

        public string TipeHp5
        {
            get => _tipeHp5;

            set
            {
                _tipeHp5 = value;
                NotifyOfPropertyChange(() => TipeHp5);
            }
        }

        public bool CanAdd
        {
            get => !string.IsNullOrWhiteSpace(Nama);
        }

        public AddMemberViewModel(IMemberEndpoint memberEndpoint)
        {
            _memberEndpoint = memberEndpoint;
        }

        public async Task Add()
        {
            MemberModel member = new MemberModel
            {
                Nama = AppValues.MEMBER_NAME_PREFIX + Nama,
                NoHp = NoHp,
                Alamat = Alamat,
                TipeHp1 = TipeHp1,
                TipeHp2 = TipeHp2,
                TipeHp3 = TipeHp3,
                TipeHp4 = TipeHp4,
                TipeHp5 = TipeHp5,
            };

            await _memberEndpoint.Insert(member);

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
