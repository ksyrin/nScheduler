using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Imp.Jobs;

namespace nScheduler.Imp.Events.JobInfo;

public class JobInfoExec : IRequest<BaseResult>
{
    public string Id { get; set; }
}

public class JobInfoExecHandler : IRequestHandler<JobInfoExec, BaseResult>
{
    private readonly IJobInfoRepository repository;
    private readonly JobEvent jobEvent;

    public JobInfoExecHandler(IJobInfoRepository repository,
        JobEvent jobEvent)
    {
        this.repository = repository;
        this.jobEvent = jobEvent;
    }

    public async Task<BaseResult> Handle(JobInfoExec request, CancellationToken cancellationToken)
    {
        try
        {
            var job = await repository.Find(request.Id.ToGuid(), cancellationToken) ?? throw new ArgumentNullException("找不到相关作业");

            job.ExecTime = DateTime.Now;
            if (job.ExecTime > job.EndTime)
            {
                job.Status = -1;
                throw new InvalidOperationException("作业执行时间超出结束时间");
            }
            job.Status = 0;

            await repository.Edit(job, cancellationToken);
            await jobEvent.StartJob(request.Id, cancellationToken);

            return new BaseResult
            {
                Code = 0,
                Msg = "作业启动成功"
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