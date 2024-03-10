using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Domain.ViewModels.Jobs;

namespace nScheduler.Imp.Events.JobLog;

public class JobLogPage : IRequest<(int, IEnumerable<JobLogViewModel>)>
{
    public JobLogSearchViewModel Model { get; set; }

    public int Page { get; set; }

    public int Size { get; set; }
}

public class JobInfoPageHandler : IRequestHandler<JobLogPage, (int, IEnumerable<JobLogViewModel>)>
{
    private readonly IJobLogRepository repository;

    public JobInfoPageHandler(IJobLogRepository repository)
    {
        this.repository = repository;
    }

    public async Task<(int, IEnumerable<JobLogViewModel>)> Handle(JobLogPage request, CancellationToken cancellationToken)
    {
        var result = await repository.Page(request.Model.GetExpression(),
            request.Page, request.Size, cancellationToken);

        return (result.Item1, result.Item2.Select(x =>
        {
            return new JobLogViewModel()
            {
                Id = x.Id.ToStringN(),
                ImageId = x.ImageId.ToStringN(),
                JobName = x.JobName,
                StartTime = x.BeginTime.ToString("yyyy/MM/dd HH:mm:ss"),
                EndTime = x.EndTime.HasValue ? x.EndTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                Content = x.Content ?? "",
                State = x.JobState
            };
        }));
    }
}