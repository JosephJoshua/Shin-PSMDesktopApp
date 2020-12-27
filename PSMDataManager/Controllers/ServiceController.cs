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
        public List<ServiceModel> Get()
        {
            ServiceData data = new ServiceData();
            return data.GetServices();
        }

        [HttpGet]
        [Route("api/Service/{nomorNota}")]
        public ServiceModel GetByNomorNota(int nomorNota)
        {
            ServiceData data = new ServiceData();
            return data.GetServiceByNomorNota(nomorNota);
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

            if (string.IsNullOrWhiteSpace(service.Kerusakan))
            {
                return BadRequest("The field 'Kerusakan' cannot be null");
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
        public IHttpActionResult Put(ServiceModel service)
        {
            if (service.TanggalKonfirmasi == DateTime.MinValue)
            {
                service.TanggalKonfirmasi = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            if (service.TanggalPengambilan == DateTime.MinValue)
            {
                service.TanggalPengambilan = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            ServiceData data = new ServiceData();
            data.UpdateService(service);

            return Ok();
        }

        [HttpDelete]
        [Route("api/Service/{nomorNota}")]
        public IHttpActionResult Delete(int nomorNota)
        {
            ServiceData data = new ServiceData();
            data.DeleteService(nomorNota);

            return Ok();
        }
    }
}