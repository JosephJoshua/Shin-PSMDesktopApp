using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class MemberController : ApiController
    {
        public List<MemberModel> Get()
        {
            MemberData data = new MemberData();
            return data.GetMembers();
        }
    }
}
