using Dapper;
using System.Data;

namespace PSMDataManager.Library
{
    public class DapperConfig
    {
        public void Initialize()
        {
            SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);
        }
    }
}
