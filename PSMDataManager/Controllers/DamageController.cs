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
        public void Post([FromBody]string value)
        {
        }

        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}