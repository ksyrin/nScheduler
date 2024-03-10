using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Domain.ViewModels.Jobs;

namespace nScheduler.Imp.Events.JobInfo;

public class JobInfoPage : IRequest<(int, IEnumerable<JobListViewModel>)>
{
    public JobInfoSearchViewModel Model { get; set; }

    public int Page { get; set; }

    public int Size { get; set; }
}

public class JobInfoPageHandler : IRequestHandler<JobInfoPage, (int, IEnumerable<JobListViewModel>)>
{
    private readonly IJobInfoRepository repository;

    public JobInfoPageHandler(IJobInfoRepository repository)
    {
        this.repository = repository;
    }

    public async Task<(int, IEnumerable<JobListViewModel>)> Handle(JobInfoPage request, CancellationToken cancellationToken)
    {
        var result = await repository.Page(request.Model.GetExpression(),
            request.Page, request.Size, cancellationToken);

        return (result.Item1, result.Item2.Select(x =>
        {
            return new JobListViewModel()
            {
                Id = x.Id.ToStringN(),
                Name = x.Name,
                ImageName = x.Image.Name,
                State = x.Status,
                ExecTime = x.ExecTime.ToString("yyyy/MM/dd HH:mm:ss"),
                StartTime = x.StartTime.ToString("yyyy/MM/dd HH:mm:ss"),
                EndTime = x.EndTime.ToString("yyyy/MM/dd HH:mm:ss"),
            };
        }));
    }
}