using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class TechnicianController : ApiController
    {
        [HttpGet]
        public List<TechnicianModel> Get()
        {
            TechnicianData data = new TechnicianData();
            return data.GetTechnicians();
        }
    }
}
