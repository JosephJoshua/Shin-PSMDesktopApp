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
        [Route("api/Technician")]
        public List<TechnicianModel> Get()
        {
            TechnicianData data = new TechnicianData();
            return data.GetTechnicians();
        }

        [HttpGet]
        [Route("api/Technician/{id}")]
        public TechnicianModel GetById(int id)
        {
            TechnicianData data = new TechnicianData();
            return data.GetTechnicianById(id);
        }

        [HttpPost]
        [Route("api/Technician")]
        public IHttpActionResult Post(TechnicianModel technician)
        {
            TechnicianData data = new TechnicianData();
            data.InsertTechnician(technician);

            return Ok();
        }

        [HttpDelete]
        [Route("api/Technician/{id}")]
        public IHttpActionResult Delete(int id)
        {
            TechnicianData data = new TechnicianData();
            data.DeleteTechnician(id);

            return Ok();
        }
    }
}
