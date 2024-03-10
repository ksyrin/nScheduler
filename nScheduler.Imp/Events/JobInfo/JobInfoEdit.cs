using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Jobs;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Domain.ViewModels.Jobs;
using nScheduler.Imp.Jobs;
using Quartz;

namespace nScheduler.Imp.Events.JobInfo;

public class JobInfoEdit : IRequest<BaseResult>
{
    public string Id { get; set; }

    public JobViewModel Model { get; set; }

    public string UserName { get; set; }
}

public class JobInfoEditHandler : IRequestHandler<JobInfoEdit, BaseResult>
{
    private readonly IJobInfoRepository repository;
    private readonly JobEvent jobEvent;

    public JobInfoEditHandler(IJobInfoRepository repository,
        JobEvent jobEvent)
    {
        this.repository = repository;
        this.jobEvent = jobEvent;
    }

    public async Task<BaseResult> Handle(JobInfoEdit request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Id.IsEmpty() && await jobEvent.FindJob(request.Id, cancellationToken))
            {
                await jobEvent.RemoveJob(request.Id, cancellationToken);
            }

            var jobInfo = new JobInfoModel(request.Id,
                request.Model.Name,
                request.Model.ImageId,
                request.Model.Configs.Select(x => new JobConfigModel() { ConfigName = x.ConfigName, ConfigType = x.ConfigType, ConfigValue = x.ConfigValue }),
                request.Model.StartTime,
                UpdateExecTime(request.Model.Cron, request.Model.StartTime),
                request.Model.EndTime,
                request.Model.Cron,
                request.Model.PreJobId,
                request.UserName);

            await jobEvent.AddJob(jobInfo, cancellationToken);
            var result = await repository.Edit(jobInfo, cancellationToken);

            return new BaseResult
            {
                Code = result ? 0 : -1,
                Msg = result ? "" : "编辑数据失败"
            };
        }
        catch (Exception ex)
        {
            return new BaseResult
            {
                Code = -1,
                Msg = ex.Message
            };
        }
    }

    private DateTime UpdateExecTime(string cron, DateTime startTime)
    {
        if (!cron.IsEmpty())
        {
            CronExpression expression = new(cron);
            DateTime exec = startTime;
            do
            {
                var time = expression.GetNextValidTimeAfter(exec);
                exec = time!.Value.LocalDateTime;
            } while (exec < DateTime.Now);
            return exec;
        }
        else
        {
            return startTime;
        }
    }
}