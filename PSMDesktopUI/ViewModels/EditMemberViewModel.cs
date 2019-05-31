using Caliburn.Micro;
using PSMDesktopUI.Library;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class EditMemberViewModel : Screen
    {
        private readonly IMemberEndpoint _memberEndpoint;

        private int _id;
        private string _nama;
        private string _noHp;
        private string _alamat;

        private string _tipeHp1;
        private string _tipeHp2;
        private string _tipeHp3;
        private string _tipeHp4;
        private string _tipeHp5;

        public int Id
        {
            get => _id;

            set
            {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public string Nama
        {
            get => _nama;

            set
            {
                _nama = value;

                NotifyOfPropertyChange(() => Nama);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public string NamePrefix
        {
            get => AppValues.MEMBER_NAME_PREFIX;
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

        public bool CanSave
        {
            get => !string.IsNullOrEmpty(Nama);
        }

        public EditMemberViewModel(IMemberEndpoint memberEndpoint)
        {
            _memberEndpoint = memberEndpoint;
        }

        public void SetFieldValues(MemberModel member)
        {
            Id = member.Id;
            Nama = member.Nama.StartsWith(AppValues.MEMBER_NAME_PREFIX) ? member.Nama.Remove(0, 2) : member.Nama;
            NoHp = member.NoHp;
            Alamat = member.Alamat;

            TipeHp1 = member.TipeHp1;
            TipeHp2 = member.TipeHp2;
            TipeHp3 = member.TipeHp3;
            TipeHp4 = member.TipeHp4;
            TipeHp5 = member.TipeHp5;
        }

        public async Task Save()
        {
            MemberModel member = new MemberModel
            {
                Id = Id,
                Nama = Nama.StartsWith(AppValues.MEMBER_NAME_PREFIX) ? Nama : AppValues.MEMBER_NAME_PREFIX + Nama,
                NoHp = NoHp,
                Alamat = Alamat,
                TipeHp1 = TipeHp1,
                TipeHp2 = TipeHp2,
                TipeHp3 = TipeHp3,
                TipeHp4 = TipeHp4,
                TipeHp5 = TipeHp5,
            };

            await _memberEndpoint.Update(member);

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
