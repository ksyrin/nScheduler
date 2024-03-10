using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Imp.Jobs;

namespace nScheduler.Imp.Events.JobInfo;

public class JobInfoInValid : IRequest<BaseResult>
{
    public string Id { get; set; }
}

public class JobInfoInValidHandler : IRequestHandler<JobInfoInValid, BaseResult>
{
    private readonly IJobInfoRepository repository;
    private readonly JobEvent jobEvent;

    public JobInfoInValidHandler(IJobInfoRepository repository,
        JobEvent jobEvent)
    {
        this.repository = repository;
        this.jobEvent = jobEvent;
    }

    public async Task<BaseResult> Handle(JobInfoInValid request, CancellationToken cancellationToken)
    {
        try
        {
            var job = await repository.Find(request.Id.ToGuid(), cancellationToken) ?? throw new ArgumentNullException("找不到相关作业");
            job.Status = -1;
            job.ExecTime = job.EndTime.AddDays(1);

            await repository.Edit(job, cancellationToken);
            await jobEvent.RemoveJob(request.Id, cancellationToken);

            return new BaseResult
            {
                Code = 0,
                Msg = "作业失效成功"
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
}