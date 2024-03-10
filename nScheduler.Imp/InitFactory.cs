using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Imp.Jobs;
using nScheduler.Imp.Repositories;
using nScheduler.Imp.Repositories.Configs;
using nScheduler.Imp.Repositories.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace nScheduler.Imp;

public static class InitFactory
{
    public static void Register(this IServiceCollection services)
    {
        services.AddScoped<IParameterCfgRepository, ParameterCfgRepository>();
        services.AddScoped<IImageCfgRepository, ImageCfgRepository>();
        services.AddScoped<IMessageCfgRepository, MessageCfgRepository>();
        services.AddScoped<IJobInfoRepository, JobInfoRepository>();
        services.AddScoped<IJobLogRepository, JobLogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // 注入MediatR
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(typeof(InitFactory).Assembly);
        });
    }

    public static void RegisterJob(this IServiceCollection services, IConfiguration configuration)
    {
        // Quartz作业相关
        services.AddSingleton<IJob, SchedulerJob>();
        services.AddSingleton<IJobFactory, SchedulerJobFactory>();
        services.AddSingleton(provider =>
        {
            var scheduler = configuration == null ?
                StdSchedulerFactory.GetDefaultScheduler().Result :
                new StdSchedulerFactory(new QuartzOptions(configuration).ToProperties()).GetScheduler().Result;
            scheduler.JobFactory = provider.GetRequiredService<IJobFactory>();
            return scheduler!;
        });
        services.AddScoped<JobEvent>();
    }

    public static async Task InitSeed(this DataContext context)
    {
        var userList = new List<UserModel>
        {
            new UserModel("admin","Admin", "123456",UserRole.Admin,"admin@123.com","1234567890", "Admin"),
            new UserModel("manager","Manager", "123456",UserRole.Manager,"manager@123.com","1234567890", "Admin"),
            new UserModel("user","User", "123456",UserRole.User,"user@123.com","1234567890", "Admin"),
        };

        await context.Users.AddRangeAsync(userList);
        await context.SaveChangesAsync();

        var sqlfile = Path.Combine(Environment.CurrentDirectory, "quartz_tables.sql");
        if (File.Exists(sqlfile))
        {
            var sql = File.ReadAllText(sqlfile);
            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}