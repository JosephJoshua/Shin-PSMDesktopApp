using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    public class SparepartController : ApiController
    {
        [HttpGet]
        [Route("api/Sparepart/{nomorNota}")]
        public List<SparepartModel> GetByService(int nomorNota)
        {
            SparepartData data = new SparepartData();
            return data.GetSparepartsByService(nomorNota);
        }

        [HttpPost]
        public IHttpActionResult Post(SparepartModel sparepart)
        {
            SparepartData data = new SparepartData();
            data.InsertSparepart(sparepart);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            SparepartData data = new SparepartData();
            data.DeleteSparepart(id);

            return Ok();
        }
    }
}