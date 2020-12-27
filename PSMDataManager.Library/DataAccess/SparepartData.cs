using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;

namespace PSMDataManager.Library.DataAccess
{
    public class SparepartData
    {
        public List<SparepartModel> GetSpareparts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var data = sql.LoadData<SparepartModel, dynamic>("dbo.spGetAllSpareparts", new { }, "PSMData");
            return data;
        }

        public List<SparepartModel> GetSparepartByNomorNota(int nomorNota)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { nomorNota };

            var data = sql.LoadData<SparepartModel, dynamic>("dbo.spGetSparepartByNomorNota", p, "PSMData");
            return data;
        }

        public void InsertSparepart(SparepartModel sparepart)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { sparepart.NomorNota, sparepart.Nama, sparepart.Harga, sparepart.TanggalPembelian };

            sql.SaveData<dynamic>("dbo.spInsertSparepart", p, "PSMData");
        }

        public void DeleteSparepart(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { id };

            sql.SaveData<dynamic>("dbo.spDeleteSparepart", p, "PSMData");
        }
    }
}
