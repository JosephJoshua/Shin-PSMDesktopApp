using System.Collections.Generic;
using System.Threading.Tasks;
using PSMDesktopUI.Library.Models;

namespace PSMDesktopUI.Library.Api
{
    public interface IMemberEndpoint
    {
        Task<List<MemberModel>> GetAll();

        Task Insert(MemberModel member);

        Task Delete(int id);
    }
}