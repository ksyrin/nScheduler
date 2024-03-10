using Dapper;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using nScheduler.Domain.Repositories;
using Quartz.Impl.AdoJobStore.Common;
using System.Text;

namespace nScheduler.Infrastructure.Repositories;

public class RepositorySQLite : IRepository
{
    public RepositorySQLite(IDbProvider dbProvider)
    {
        DbProvider = dbProvider;
    }

    public IDbProvider DbProvider { get; }

    public async Task<int> InitTable()
    {
        if (!Directory.Exists("File")) Directory.CreateDirectory("File");

        using (var connection = new SqliteConnection(DbProvider.ConnectionString))
        {
            var check_sql = @"";
            var count = await connection.QueryFirstOrDefaultAsync<int>(check_sql);
            // 初始化 建表
            if (count == 0)
            {
                string init_sql = await File.ReadAllTextAsync("");
                return await connection.ExecuteAsync(init_sql);
            }
        }
        return 0;
    }

    public async Task<bool> RemoveErrLogAsync(string jobGroup, string jobName)
    {
        try
        {
            using (var connection = new MySqlConnection(DbProvider.ConnectionString))
            {
                string sql = "";
                var byteArray = await connection.ExecuteScalarAsync<byte[]>(sql, new { jobName, jobGroup });
                var jsonStr = Encoding.Default.GetString(byteArray);
                JObject source = JObject.Parse(jsonStr);
                source.Remove("Exception");
                var modifySql = "";
                await connection.ExecuteAsync(modifySql, new { jobName, jobGroup, jobData = source.ToString() });
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}