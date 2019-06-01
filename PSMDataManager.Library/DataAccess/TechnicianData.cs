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
    }
}
