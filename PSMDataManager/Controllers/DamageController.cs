using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class DamageController : ApiController
    {
        [HttpGet]
        [Route("api/Damage")]
        public IEnumerable<DamageModel> Get()
        {
            DamageData data = new DamageData();
            return data.GetDamages();
        }

        [HttpPost]
        [Route("api/Damage")]
        public IHttpActionResult Post([FromBody]DamageModel damage)
        {
            DamageData data = new DamageData();
            data.InsertDamage(damage);

            return Ok();
        }

        [HttpDelete]
        [Route("api/Damage/{id}")]
        public IHttpActionResult Delete(int id)
        {
            DamageData data = new DamageData();
            data.DeleteDamage(id);

            return Ok();
        }
    }
}