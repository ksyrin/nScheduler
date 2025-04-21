using MediatR;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Domain.ViewModels.Jobs;

namespace nScheduler.Imp.Events.JobLog;

public class JobLogDetail : IRequest<IEnumerable<JobLogDetailViewModel>>
{
    public JobLogSearchViewModel Model { get; set; }
}

public class JobLogDetailHandler : IRequestHandler<JobLogDetail, IEnumerable<JobLogDetailViewModel>>
{
    private readonly IJobLogRepository repository;

    public JobLogDetailHandler(IJobLogRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<JobLogDetailViewModel>> Handle(JobLogDetail request, CancellationToken cancellationToken)
    {
        var result = await repository.Page(request.Model.GetExpression(), 1, 30, cancellationToken);

        return result.Item2.OrderBy(x => x.BeginTime).Select(x =>
        {
            return new JobLogDetailViewModel()
            {
                StartTime = x.BeginTime.ToString("yyyy/MM/dd HH:mm:ss"),
                EndTime = x.EndTime.HasValue ? x.EndTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                IntervalTime = x.IntervalTime,
                State = x.JobState
            };
        });
    }
}