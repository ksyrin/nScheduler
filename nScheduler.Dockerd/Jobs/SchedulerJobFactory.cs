using Quartz;
using Quartz.Spi;

namespace nScheduler.Dockerd.Jobs;

public class SchedulerJobFactory : IJobFactory
{
    private readonly IServiceProvider provider;

    public SchedulerJobFactory(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return provider.GetService(typeof(IJob)) as IJob;
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}