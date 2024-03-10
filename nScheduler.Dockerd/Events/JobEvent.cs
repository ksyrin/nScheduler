using nScheduler.Common.Extensions;
using nScheduler.Dockerd.Jobs;
using nScheduler.Domain.Models.Jobs;
using Quartz;

namespace nScheduler.Dockerd.Events;

public class JobEvent
{
    private readonly IScheduler scheduler;

    public JobEvent(IScheduler scheduler)
    {
        this.scheduler = scheduler;
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task AddJob(JobInfoModel model)
    {
        // 创建作业
        var job = JobBuilder.Create<SchedulerJob>()
            .WithIdentity(model.GetId())
            .Build();

        // 创建触发器
        var triggerBuidler = TriggerBuilder.Create();
        // 开始时间，cron定时，结束时间
        triggerBuidler.StartAt(model.StartTime);
        if (!model.Cron.IsEmpty())
        {
            triggerBuidler.WithCronSchedule(model.Cron!, x =>
            {
                x.WithMisfireHandlingInstructionDoNothing();
            });
        }
        triggerBuidler.EndAt(model.EndTime);
        var trigger = triggerBuidler.Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    /// <summary>
    /// 启动作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task StartJob(string id)
    {
        var jobKey = JobKey.Create(id);

        var job = await scheduler.GetJobDetail(jobKey);
        if (job == null)
        {
            throw new ArgumentNullException("找不到相关作业");
        }

        await scheduler.TriggerJob(jobKey);
    }

    /// <summary>
    /// 停止作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task StopJob(string id)
    {
        await scheduler.PauseJob(JobKey.Create(id));
    }

    /// <summary>
    /// 移除作业
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task RemoveJob(string id)
    {
        await scheduler.DeleteJob(JobKey.Create(id));
    }
}