using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Domain.ViewModels.Jobs;

namespace nScheduler.Imp.Events.JobInfo;

public class JobInfoSignle : IRequest<JobViewModel?>
{
    public string Id { get; set; }
}

public class JobInfoSignleHandler : IRequestHandler<JobInfoSignle, JobViewModel?>
{
    private readonly IJobInfoRepository repository;

    public JobInfoSignleHandler(IJobInfoRepository repository)
    {
        this.repository = repository;
    }

    public async Task<JobViewModel?> Handle(JobInfoSignle request,
        CancellationToken cancellationToken)
    {
        var item = await repository.Find(request.Id.ToGuid(), cancellationToken);
        return item == null ? null : new JobViewModel
        {
            Name = item.Name,
            ImageId = item.Image.Id.ToStringN(),
            StartTime = item.StartTime,
            EndTime = item.EndTime,
            Configs = item.GetJobConfigs().Select(x => new JobConfigViewModel() { ConfigName = x.ConfigName, ConfigType = x.ConfigType, ConfigValue = x.ConfigValue }).ToList(),
            Cron = item.Cron ?? "",
            PreJobId = item.PreJobId ?? ""
        };
    }
}