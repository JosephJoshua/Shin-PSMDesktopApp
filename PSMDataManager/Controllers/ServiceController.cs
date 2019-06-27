using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class ServiceController : ApiController
    {
        [HttpGet]
        public IEnumerable<ServiceModel> Get()
        {
            ServiceData data = new ServiceData();
            return data.GetServices();
        }

        [HttpPost]
        public IHttpActionResult Post(ServiceModel service)
        {
            if (string.IsNullOrWhiteSpace(service.NamaPelanggan))
            {
                return BadRequest("The field 'NamaPelanggan' cannot be null");
            }

            if (string.IsNullOrWhiteSpace(service.TipeHp))
            {
                return BadRequest("The field 'TipeHp' cannot be null");
            }

            if (service.TanggalKonfirmasi == DateTime.MinValue)
            {
                service.TanggalKonfirmasi = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            if (service.TanggalPengambilan == DateTime.MinValue)
            {
                service.TanggalPengambilan = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            ServiceData data = new ServiceData();
            data.InsertService(service);

            return Ok();
        }

        [HttpPut]
        public void Put(ServiceModel service)
        {
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            ServiceData data = new ServiceData();
            data.DeleteService(id);

            return Ok();
        }
    }
}