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

        public void InsertMember(MemberModel member)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { member.Nama, member.NoHp, member.Alamat, member.TipeHp1, member.TipeHp2, member.TipeHp3, member.TipeHp4, member.TipeHp5 };

            sql.SaveData<dynamic>("dbo.spInsertMember", p, "PSMData");
        }

        public void DeleteMember(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { id };

            sql.SaveData<dynamic>("dbo.spDeleteMember", p, "PSMData");
        }
    }
}
