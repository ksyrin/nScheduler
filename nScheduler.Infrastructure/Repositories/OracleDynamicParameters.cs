using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace nScheduler.Infrastructure.Repositories;

public class OracleDynamicParameters
{
    private static Dictionary<SqlMapper.Identity, Action<IDbCommand, object>> paranReaderCache = new();
    private Dictionary<string, ParamInfo> parameters = new Dictionary<string, ParamInfo>();
    private List<object> templates;

    private class ParamInfo
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public ParameterDirection ParameterDirection { get; set; }

        public OracleDbType? DbType { get; set; }

        public int? Size { get; set; }

        public IDbDataParameter AttachParam { get; set; }
    }

    public OracleDynamicParameters()
    {
    }

    public OracleDynamicParameters(object template)
    {
    }

    public void AddDynamicParams(object param)
    {
    }
}