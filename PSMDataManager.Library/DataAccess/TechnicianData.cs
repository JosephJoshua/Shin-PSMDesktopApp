using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;

namespace PSMDataManager.Library.DataAccess
{
    public class TechnicianData
    {
        public List<TechnicianModel> GetTechnicians()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var data = sql.LoadData<TechnicianModel, dynamic>("dbo.spGetAllTechnicians", new { }, "PSMData");
            return data;
        }

        public void InsertTechnician(TechnicianModel technician)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { technician.Nama };

            sql.SaveData<dynamic>("dbo.spInsertTechnician", p, "PSMData");
        }

        public void DeleteTechnician(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { id };

            sql.SaveData<dynamic>("dbo.spDeleteTechnician", p, "PSMData");
        }
    }
}
