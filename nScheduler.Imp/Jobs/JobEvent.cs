using nScheduler.Common.Extensions;
using nScheduler.Domain.Models.Jobs;
using Quartz;

namespace nScheduler.Imp.Jobs;

public class JobEvent
{
    private readonly IScheduler scheduler;

    public JobEvent(IScheduler scheduler)
    {
        this.scheduler = scheduler;
    }

    public async Task<bool> FindJob(string id, CancellationToken cancellationToken = default)
    {
        return await scheduler.CheckExists(JobKey.Create(id), cancellationToken);
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task AddJob(JobInfoModel model, CancellationToken cancellationToken = default)
    {
        // 创建作业
        var job = JobBuilder.Create<SchedulerJob>()
            .WithIdentity(model.Id.ToStringN())
            .Build();

        // 创建触发器
        var triggerBuidler = TriggerBuilder.Create();
        // 开始时间，cron定时，结束时间
        triggerBuidler.StartAt(model.StartTime);
        if (model.Cron != null && !model.Cron.IsEmpty())
        {
            triggerBuidler.WithCronSchedule(model.Cron!, x =>
            {
                x.WithMisfireHandlingInstructionDoNothing();
            });
        }
        triggerBuidler.EndAt(model.EndTime);
        var trigger = triggerBuidler.Build();

        await scheduler.ScheduleJob(job, trigger, cancellationToken);
    }

    /// <summary>
    /// 启动作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task StartJob(string id, CancellationToken cancellationToken = default)
    {
        var jobKey = JobKey.Create(id);

        var job = await scheduler.GetJobDetail(jobKey, cancellationToken);
        if (job == null)
        {
            throw new ArgumentNullException("找不到相关作业");
        }

        await scheduler.TriggerJob(jobKey, cancellationToken);
    }

    /// <summary>
    /// 停止作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task StopJob(string id, CancellationToken cancellationToken = default)
    {
        await scheduler.Interrupt(JobKey.Create(id), cancellationToken);
    }

    /// <summary>
    /// 移除作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task RemoveJob(string id, CancellationToken cancellationToken = default)
    {
        await scheduler.DeleteJob(JobKey.Create(id), cancellationToken);
    }
}