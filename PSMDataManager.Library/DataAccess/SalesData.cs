using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace PSMDataManager.Library.DataAccess
{
    public class SalesData
    {
        public List<SalesModel> GetSales()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var data = sql.LoadData<SalesModel, dynamic>("dbo.spGetAllSales", new { }, "PSMData");
            return data;
        }

        public SalesModel GetSalesById(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            SalesModel sales = sql.LoadData<SalesModel, dynamic>("dbo.spGetSalesById", new { Id = id }, "PSMData").FirstOrDefault();

            return sales;
        }

        public void InsertSales(SalesModel sales)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { sales.Nama };

            sql.SaveData<dynamic>("dbo.spInsertSales", p, "PSMData");
        }

        public void DeleteSales(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { id };

            sql.SaveData<dynamic>("dbo.spDeleteSales", p, "PSMData");
        }
    }
}
