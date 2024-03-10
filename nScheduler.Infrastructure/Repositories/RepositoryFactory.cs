using nScheduler.Domain.Repositories;
using Quartz.Impl.AdoJobStore;
using Quartz.Impl.AdoJobStore.Common;

namespace nScheduler.Infrastructure.Repositories;

public class RepositoryFactory
{
    public static IRepository CreateRepository(string driverDelegateType, IDbProvider dbProvider)
    {
        if (driverDelegateType == typeof(SQLiteDelegate).AssemblyQualifiedName)
        {
            return new RepositorySQLite(dbProvider);
        }
        else if (driverDelegateType == typeof(MySQLDelegate).AssemblyQualifiedName)
        {
            return new RepositoryMySql(dbProvider);
        }
        else if (driverDelegateType == typeof(PostgreSQLDelegate).AssemblyQualifiedName)
        {
            return new RepositoryPostgreSQL(dbProvider);
        }
        else if (driverDelegateType == typeof(OracleDelegate).AssemblyQualifiedName)
        {
            return new RepositoryOracle(dbProvider);
        }
        else if (driverDelegateType == typeof(SqlServerDelegate).AssemblyQualifiedName)
        {
            return new RepositorySqlServer(dbProvider);
        }
        else
        {
            return null;
        }
    }
}