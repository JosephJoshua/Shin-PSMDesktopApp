using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;

namespace PSMDataManager.Library.DataAccess
{
    public class SparepartData
    {
        public List<SparepartModel> GetSparepartsByService(int nomorNota)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { nomorNota };

            var data = sql.LoadData<SparepartModel, dynamic>("dbo.spGetSparepartsByService", p, "PSMData");
            return data;
        }

        public void InsertSparepart(SparepartModel sparepart)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { sparepart.NomorNota, sparepart.Nama, sparepart.Harga };

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
