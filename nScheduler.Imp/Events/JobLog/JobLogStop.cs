using MediatR;
using nScheduler.Common.Models;
using nScheduler.Imp.Jobs;

namespace nScheduler.Imp.Events.JobLog;

public class JobLogStop : IRequest<BaseResult>
{
    public string Id { get; set; }
}

public class JobLogStopHandler : IRequestHandler<JobLogStop, BaseResult>
{
    private readonly JobEvent jobEvent;

    public JobLogStopHandler(JobEvent jobEvent)
    {
        this.jobEvent = jobEvent;
    }

    public async Task<BaseResult> Handle(JobLogStop request, CancellationToken cancellationToken)
    {
        try
        {
            await jobEvent.StopJob(request.Id, cancellationToken);
            return new BaseResult
            {
                Code = 0,
                Msg = "停止作业成功"
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