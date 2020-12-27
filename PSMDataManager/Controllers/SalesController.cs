using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class SalesController : ApiController
    {
        [HttpGet]
        [Route("api/Sales")]
        public List<SalesModel> Get()
        {
            SalesData data = new SalesData();
            return data.GetSales();
        }

        [HttpGet]
        [Route("api/Sales/{id}")]
        public SalesModel GetById(int id)
        {
            SalesData data = new SalesData();
            return data.GetSalesById(id);
        }

        [HttpPost]
        [Route("api/Sales")]
        public IHttpActionResult Post(SalesModel sales)
        {
            SalesData data = new SalesData();
            data.InsertSales(sales);

            return Ok();
        }

        [HttpDelete]
        [Route("api/Sales/{id}")]
        public IHttpActionResult Delete(int id)
        {
            SalesData data = new SalesData();
            data.DeleteSales(id);

            return Ok();
        }
    }
}