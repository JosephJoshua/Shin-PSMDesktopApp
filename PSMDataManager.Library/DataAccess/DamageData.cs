using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;

namespace PSMDataManager.Library.DataAccess
{
    public class DamageData
    {
        public List<DamageModel> GetDamages()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var data = sql.LoadData<DamageModel, dynamic>("dbo.spGetAllDamages", new { }, "PSMData");
            return data;
        }
    }
}
