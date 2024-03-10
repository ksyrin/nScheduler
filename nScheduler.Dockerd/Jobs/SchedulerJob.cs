using Microsoft.Extensions.DependencyInjection;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Events;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.Repositories.Jobs;
using Quartz;

namespace nScheduler.Dockerd.Jobs;

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
        var job = await jobRepository.Find(id.ToGuid());
        if (job == null)
        {
            throw new ArgumentNullException("找不到相关作业");
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
        var log = await @event.StartJob(job, inputs, envs);
        await logRepository.Edit(log);
        await jobRepository.Edit(job);

        // 检查任务运行状态
        do
        {
            await Task.Delay(1000 * 60 * 3);

            await @event.UpdateState(log);

            await logRepository.Edit(log);
        } while (!log.IsFinish);

        // 更新作业状态
        @event.RemoveJob(log.Id);
        await jobRepository.SetJobFinish(job, context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.LocalDateTime : job.EndTime.AddDays(1));
    }
}