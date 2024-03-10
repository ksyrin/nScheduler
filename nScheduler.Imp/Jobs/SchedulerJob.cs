using MediatR;
using Microsoft.Extensions.DependencyInjection;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Events;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Imp.Events.Msg;
using Quartz;

namespace nScheduler.Imp.Jobs;

public class SchedulerJob : IJob
{
    private readonly IServiceProvider provider;

    public SchedulerJob(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = provider.CreateScope();
        var jobRepository = scope.ServiceProvider.GetRequiredService<IJobInfoRepository>();

        string id = context.JobDetail.Key.Name;
        var job = await jobRepository.Find(id.ToGuid(), context.CancellationToken);
        if (job == null)
        {
            throw new ArgumentNullException("找不到相关作业");
        }
        if (job.Status != 0)
        {
            throw new InvalidOperationException("当前作业已运行");
        }

        var @event = scope.ServiceProvider.GetRequiredService<ISchedulerEvent>();
        var logRepository = scope.ServiceProvider.GetRequiredService<IJobLogRepository>();

        // 获取配置参数
        Dictionary<string, string> envs = new();
        Dictionary<string, string> inputs = new();
        var configs = job.GetJobConfigs();
        foreach (var cfg in configs)
        {
            switch (cfg.ConfigType)
            {
                case ParameterType.param:
                    var parameterCfgRepository = scope.ServiceProvider.GetRequiredService<IParameterCfgRepository>();
                    var paramCfg = await parameterCfgRepository.Find(cfg.ConfigValue.ToGuid());
                    if (paramCfg != null)
                    {
                        envs.Add(cfg.ConfigName, paramCfg.Content);
                    }
                    break;

                case ParameterType.input:
                    inputs.Add(cfg.ConfigName, cfg.ConfigValue);
                    break;
            }
        }

        // 启动任务，并记录
        var log = await @event.StartJob(job, inputs, envs, context.CancellationToken);
        await logRepository.Edit(log, context.CancellationToken);
        await jobRepository.Edit(job, context.CancellationToken);

        try
        {
            // 检查任务运行状态
            while (true)
            {
                await Task.Delay(1000 * 60 * 1);

                await @event.UpdateState(log, context.CancellationToken);

                await logRepository.Edit(log, context.CancellationToken);

                if (context.CancellationToken.IsCancellationRequested)
                {
                    throw new JobExecutionException("作业已终止");
                }

                if (log.IsFinish)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            log.JobState = JobStatus.Error;
            log.Content += Environment.CommandLine + ex;
        }
        finally
        {
            // 更新作业状态
            await @event.RemoveJob(log.Id);
            await jobRepository.SetJobFinish(job,
                context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.LocalDateTime : job.EndTime.AddDays(1));
        }

        if (job.MessageId.HasValue)
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Publish(new MsgSend()
            {
                MessageId = job.MessageId.Value,
                Content = log.JobState == JobStatus.Error ? $"作业 {job.Name} 执行出错" : $"作业 {job.Name} 执行成功"
            });
        }
    }
}