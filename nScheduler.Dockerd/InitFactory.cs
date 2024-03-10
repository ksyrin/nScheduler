using Docker.DotNet;
using Microsoft.Extensions.DependencyInjection;
using nScheduler.Dockerd.Events;
using nScheduler.Dockerd.Jobs;
using nScheduler.Dockerd.Repositories.Configs;
using nScheduler.Dockerd.Repositories.Jobs;
using nScheduler.Domain.Events;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.Repositories.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace nScheduler.Dockerd;

public static class InitFactory
{
    public static void Register(this IServiceCollection services)
    {
        services.AddScoped<IParameterCfgRepository, ParameterCfgRepository>();
        services.AddScoped<IImageCfgRepository, ImageCfgRepository>();
        services.AddScoped<IMessageCfgRepository, MessageCfgRepository>();
        services.AddScoped<IJobInfoRepository, JobInfoRepository>();
        services.AddScoped<IJobLogRepository, JobLogRepository>();
        // Docker相关的配置
        services.AddScoped<IDockerClient>(x => new DockerClientConfiguration().CreateClient());
        services.AddScoped<ISchedulerEvent, DockerEvent>();
        // Quartz作业相关
        services.AddSingleton<IJob, SchedulerJob>();
        services.AddSingleton<IJobFactory, SchedulerJobFactory>();
        services.AddSingleton(provider =>
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.JobFactory = provider.GetRequiredService<IJobFactory>();
            return scheduler;
        });
        services.AddScoped<JobEvent>();
    }
}