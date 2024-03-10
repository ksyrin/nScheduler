using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Domain.ViewModels.Jobs;

namespace nScheduler.Imp.Events.JobLog;

public class JobLogSignle : IRequest<JobLogViewModel?>
{
    public string Id { get; set; }
}

public class JobLogSignleHandler : IRequestHandler<JobLogSignle, JobLogViewModel?>
{
    private readonly IJobLogRepository repository;

    public JobLogSignleHandler(IJobLogRepository repository)
    {
        this.repository = repository;
    }

    public async Task<JobLogViewModel?> Handle(JobLogSignle request,
        CancellationToken cancellationToken)
    {
        var item = await repository.Find(request.Id.ToGuid(), cancellationToken);
        return item == null ? null : new JobLogViewModel
        {
            Id = item.Id.ToStringN(),
            JobName = item.JobName,
            StartTime = item.BeginTime.ToString("yyyy/MM/dd HH:mm:ss"),
            EndTime = item.EndTime.HasValue ? item.EndTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
            Content = item.Content ?? "",
            State = item.JobState
        };
    }
}