using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Imp.Jobs;

namespace nScheduler.Imp.Events.JobInfo;

public class JobInfoDel : IRequest<BaseResult>
{
    public string Id { get; set; }
}

public class JobInfoDelHandler : IRequestHandler<JobInfoDel, BaseResult>
{
    private readonly IJobInfoRepository repository;
    private readonly JobEvent jobEvent;

    public JobInfoDelHandler(IJobInfoRepository repository,
        JobEvent jobEvent)
    {
        this.repository = repository;
        this.jobEvent = jobEvent;
    }

    public async Task<BaseResult> Handle(JobInfoDel request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await repository.Find(request.Id.ToGuid(), cancellationToken);
            if (item == null) throw new ArgumentNullException("找不到相关作业");

            await jobEvent.RemoveJob(request.Id, cancellationToken);
            var result = await repository.Delete(item.Id, cancellationToken);

            return new BaseResult
            {
                Code = result ? 0 : -1,
                Msg = result ? "" : "删除数据失败"
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