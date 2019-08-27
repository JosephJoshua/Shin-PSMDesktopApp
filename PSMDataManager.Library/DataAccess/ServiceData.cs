using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;

namespace PSMDataManager.Library.DataAccess
{
    public class ServiceData
    {
        public List<ServiceModel> GetServices()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var data = sql.LoadData<ServiceModel, dynamic>("dbo.spGetAllServices", new { }, "PSMData");
            return data;
        }

        public void InsertService(ServiceModel service)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new
            {
                service.NamaPelanggan,
                service.NoHp,
                service.TipeHp,
                service.Imei,
                service.DamageId,
                service.YangBelumDicek,
                service.Kelengkapan,
                service.Warna,
                service.KataSandiPola,
                service.TechnicianId,
                service.StatusServisan,
                service.TanggalKonfirmasi,
                service.IsiKonfirmasi,
                service.Biaya,
                service.Discount,
                service.Dp,
                service.TambahanBiaya,
                service.HargaSparepart,
                service.LabaRugi,
                service.TanggalPengambilan
            };

            sql.SaveData<dynamic>("dbo.spInsertService", p, "PSMData");
        }

        public void UpdateService(ServiceModel service)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new
            {
                service.NomorNota,
                service.NamaPelanggan,
                service.NoHp,
                service.TipeHp,
                service.Imei,
                service.DamageId,
                service.YangBelumDicek,
                service.Kelengkapan,
                service.Warna,
                service.KataSandiPola,
                service.TechnicianId,
                service.StatusServisan,
                service.TanggalKonfirmasi,
                service.IsiKonfirmasi,
                service.Biaya,
                service.Discount,
                service.Dp,
                service.TambahanBiaya,
                service.HargaSparepart,
                service.TanggalPengambilan
            };

            sql.SaveData<dynamic>("dbo.spUpdateService", p, "PSMData");
        }

        public void DeleteService(int nomorNota)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { NomorNota = nomorNota };

            sql.SaveData<dynamic>("dbo.spDeleteService", p, "PSMData");
        }
    }
}
