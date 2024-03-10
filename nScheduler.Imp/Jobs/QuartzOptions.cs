using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;

namespace nScheduler.Imp.Jobs;

public class QuartzOptions
{
    public Scheduler Scheduler { get; set; }

    public ThreadPool ThreadPool { get; set; }

    public JobStore JobStore { get; set; }

    public string ConnectionString { get; set; }

    public string ConnectionType { get; set; }

    public QuartzOptions(IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("quartz");
        section.Bind(this);

        ConnectionString = configuration.GetConnectionString("DefaultConnection");
        ConnectionType = configuration.GetConnectionString("DefaultType");
    }

    public NameValueCollection ToProperties()
    {
        var properties = new NameValueCollection()
        {
            ["quartz.scheduler.instanceName"] = Scheduler?.InstanceName,
            //["quartz.scheduler.instanceId"] = Scheduler?.InstanceId,
            ["quartz.threadPool.type"] = ThreadPool?.Type,
            ["quartz.threadPool.threadPriority"] = ThreadPool?.ThreadPriority,
            ["quartz.threadPool.threadCount"] = ThreadPool?.ThreadCount.ToString(),
            ["quartz.jobStore.type"] = JobStore.Type,
            ["quartz.jobStore.dataSource"] = "myDS",
            ["quartz.jobStore.tablePrefix"] = JobStore.TablePrefix,
            ["quartz.jobStore.driverDelegateType"] = JobStore.DriverDelegateType,
            ["quartz.jobStore.clustered"] = JobStore.Clustered,
            ["quartz.dataSource.myDS.connectionString"] = ConnectionString,
            ["quartz.dataSource.myDS.provider"] = ConnectionType,
            ["quartz.serializer.type"] = "binary"
        };

        return properties;
    }
}

public class Scheduler
{
    public string InstanceName { get; set; }

    public string InstanceId { get; set; }
}

public class ThreadPool
{
    public string Type { get; set; }

    public string ThreadPriority { get; set; }

    public int ThreadCount { get; set; }
}

public class JobStore
{
    public string Type { get; set; }

    public string TablePrefix { get; set; }

    public string DriverDelegateType { get; set; }

    public string Clustered { get; set; }
}