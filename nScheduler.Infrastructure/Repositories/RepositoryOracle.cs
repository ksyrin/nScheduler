using Dapper;
using Newtonsoft.Json.Linq;
using nScheduler.Domain.Repositories;
using Oracle.ManagedDataAccess.Client;
using Quartz.Impl.AdoJobStore.Common;
using System.Text;

namespace nScheduler.Infrastructure.Repositories;

public class RepositoryOracle : IRepository
{
    public RepositoryOracle(IDbProvider dbProvider)
    {
        DbProvider = dbProvider;
    }

    public IDbProvider DbProvider { get; }

    public Task<int> InitTable()
    {
        //TODO 待实现
        return Task.FromResult(-1);
    }

    public async Task<bool> RemoveErrLogAsync(string jobGroup, string jobName)
    {
        try
        {
            using (var connection = new OracleConnection(DbProvider.ConnectionString))
            {
                string sql = "";
                var byteArray = await connection.ExecuteScalarAsync<byte[]>(sql, new { jobName, jobGroup });
                var jsonStr = Encoding.Default.GetString(byteArray);
                JObject source = JObject.Parse(jsonStr);
                source.Remove("Exception");
                var modifySql = "";
                await connection.ExecuteAsync(modifySql, new { jobName, jobGroup, jobData = Encoding.UTF8.GetBytes(source.ToString()) });
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}