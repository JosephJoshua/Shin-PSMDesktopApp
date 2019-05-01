using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;

namespace PSMDataManager.Library.DataAccess
{
    public class MemberData
    {
        public List<MemberModel> GetMembers()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var data = sql.LoadData<MemberModel, dynamic>("dbo.spGetAllMembers", new { }, "PSMData");
            return data;
        }
    }
}
