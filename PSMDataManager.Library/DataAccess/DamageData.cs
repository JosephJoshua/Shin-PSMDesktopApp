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

        public void InsertDamage(DamageModel damage)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { damage.Kerusakan };

            sql.SaveData<dynamic>("dbo.spInsertDamage", p, "PSMData");
        }

        public void DeleteDamage(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { id };

            sql.SaveData<dynamic>("dbo.spDeleteDamage", p, "PSMData");
        }
    }
}
